using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Lib.DAC.Function
{
    class SqlFunctionTemplate : ISqlFunction
    {
        private const int InvalidArgumentIndex = -1;
        private static readonly Regex SplitRegex = new Regex("(\\?[0-9]+)");

        private struct TemplateChunk
        {
            public string Text; // including prefix if parameter
            public int ArgumentIndex;

            public TemplateChunk(string chunk, int argIndex)
            {
                Text = chunk;
                ArgumentIndex = argIndex;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        //private readonly IType returnType = null;
        private readonly bool hasArguments;
        private readonly bool hasParenthesesIfNoArgs;

        private readonly string template;
        private TemplateChunk[] chunks;

        public SqlFunctionTemplate(string template)
            : this(template, true)
        {
        }

        public SqlFunctionTemplate(string template, bool hasParenthesesIfNoArgs)
        {
            //returnType = type;
            this.template = template;
            this.hasParenthesesIfNoArgs = hasParenthesesIfNoArgs;

            InitFromTemplate();
            hasArguments = chunks.Length > 1;
        }

        private void InitFromTemplate()
        {
            string[] stringChunks = SplitRegex.Split(template);
            chunks = new TemplateChunk[stringChunks.Length];

            for (int i = 0; i < stringChunks.Length; i++)
            {
                string chunk = stringChunks[i];
                if (i % 2 == 0)
                {
                    // Text part.
                    chunks[i] = new TemplateChunk(chunk, InvalidArgumentIndex);
                }
                else
                {
                    // Separator, i.e. argument
                    int argIndex = int.Parse(chunk.Substring(1), CultureInfo.InvariantCulture);
                    chunks[i] = new TemplateChunk(stringChunks[i], argIndex);
                }
            }
        }

        #region ISQLFunction Members

        //public IType ReturnType(IType columnType, IMapping mapping)
        //{
        //    return (returnType == null) ? columnType : returnType;
        //}

        public bool HasArguments
        {
            get { return hasArguments; }
        }

        public bool HasParenthesesIfNoArguments
        {
            get { return hasParenthesesIfNoArgs; }
        }

        /// <summary>
        /// Applies the template to passed in arguments.
        /// </summary>
        /// <param name="args">args function arguments</param>
        /// <param name="factory">generated SQL function call</param>
        /// <returns></returns>
        public string Build(List<object> args)
        {
            StringBuilder buf = new StringBuilder();
            foreach (TemplateChunk tc in chunks)
            {
                if (tc.ArgumentIndex != InvalidArgumentIndex)
                {
                    int adjustedIndex = tc.ArgumentIndex - 1; // Arg indices are one-based
                    object arg = adjustedIndex < args.Count ? args[adjustedIndex] : null;
                    // TODO: if (arg == null) QueryException is better ?
                    if (arg != null)
                    {
                        //if (arg is Parameter || arg is SqlString)
                        //{
                        //    buf.AddObject(arg);
                        //}
                        //else
                        //{
                        buf.Append(arg.ToString());
                        //}
                    }
                }
                else
                {
                    buf.Append(tc.Text);
                }
            }
            return buf.ToString();
        }

        #endregion

        public override string ToString()
        {
            return template;
        }
    }
}
