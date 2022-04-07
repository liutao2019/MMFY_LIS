using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Diagnostics;

namespace dcl.root.logon
{
    /// <summary>
    /// 异常记录类
    /// </summary>
    public class Logger
    {
        public static void Trace(string message)
        {
            try
            {
                StackTrace ss = new StackTrace(true);
                Type t = ss.GetFrame(1).GetMethod().DeclaringType;
                int lineNo = ss.GetFrame(1).GetFileLineNumber();
                String sName = ss.GetFrame(1).GetMethod().Name;


                System.Diagnostics.Trace.WriteLine(message, string.Format("{0}:Line{2} {1}", t.Name, sName, lineNo));
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine(message);
            }
        }

        public static void Debug(string message)
        {
            try
            {
                StackTrace ss = new StackTrace(true);
                Type t = ss.GetFrame(1).GetMethod().DeclaringType;
                int lineNo = ss.GetFrame(1).GetFileLineNumber();
                String sName = ss.GetFrame(1).GetMethod().Name;


                System.Diagnostics.Debug.WriteLine(message, string.Format("{0}:Line{2} {1}", t.Name, sName, lineNo));
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }


        /// <summary>
        /// 记录调试信息，不写日志
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="message"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteInfo(string moduleName, string title, string details)
        {
            Write(ExceptionLogType.Info, moduleName, title, details);
        }

        /// <summary>
        /// 记录异常，包括捕捉到的各种未预期的异常
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="title"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteException(string moduleName, string title, string details)
        {
            Write(ExceptionLogType.Exception, moduleName, title, details);
        }

        /// <summary>
        /// 记录业务出错
        /// 数据库数据跟预期不符或不合逻辑
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="title"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteBussError(string moduleName, string title, string details)
        {
            Write(ExceptionLogType.BusinessError, moduleName, title, details);
        }

        private static void WriteTrace(ExceptionLogType logType, string moduleName, string title, ExceptionLogEntity entity)
        {
            WriteTrace(logType, moduleName, title, entity == null ? string.Empty : entity.ToString());
        }

        private static void WriteTrace(ExceptionLogType logType, string moduleName, string details, string message)
        {
            if (message == null)
            {
                message = string.Empty;
            }
            string traceFormat = "[{0}][{1}]{2}";
            //Trace.WriteLine(string.Format(traceFormat, logType, moduleName, message));
            string[] splits = message.Split(new char[] { '\n' });
            foreach (var line in splits)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(traceFormat, logType, moduleName, line));
            }
        }

        /// <summary>
        ///  记录致命异常，如未处理异常，导至程序无法执行下去或直接退出的异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="moduleName"></param>
        /// <param name="message"></param>
        public static void WriteFetal(Exception ex, string moduleName, string message)
        {
            try
            {
                //准备日志对象
                string processName = ApplicationName;
                var entry = new ExceptionLogEntity()
                {
                    logType = ExceptionLogType.Fetal,
                    Module = moduleName,
                    Title = message,
                    //Message = string.Format(format, args),
                    timestamp = DateTime.Now,
                    ProcessName = processName
                };

                //写Trace
                WriteTrace(ExceptionLogType.Fetal, moduleName, "写志时出错", string.Format("目标路径:{0},异常信息:{1}", BasePath, ex.ToString()));

                //写文件

                //目标目录           
                string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                //文件名
                //每天每种类型的日志一个文件
                string fileName = string.Format("{0}_{1}_{2}.log", DateTime.Now.ToString("yyyyMMdd"), ExceptionLogType.Fetal.ToString(), moduleName);
                string fullPath = basePath + fileName;
                string exception = ex.ToString();

                if (!synLogFile.ContainsKey(fullPath))
                {
                    lock (synLogFile)
                    {
                        synLogFile.Add(fullPath, new object());
                    }
                }

                lock (synLogFile[fullPath])
                {
                    using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.Default))
                    {

                        sw.WriteLine(string.Format("{0}\n{1}", exception, entry.ToString()));
                    }
                }
            }
            catch (Exception ex1)
            {
                WriteTrace(ExceptionLogType.Fetal, "Log", "程序没有权限写日志，请联系管理！", ex1.ToString());
                throw new Exception("程序没有权限写日志，请联系管理！");
            }
        }

        private static void Write(ExceptionLogType logType, string module, string title, string details)
        {
            try
            {
                //准备日志对象
                string processName = ApplicationName;
                var entity = new ExceptionLogEntity()
                {
                    logType = logType,
                    Module = module,
                    Title = title,
                    Message = details,
                    timestamp = DateTime.Now,
                    ProcessName = processName
                };

                //写Trace
                WriteTrace(logType, module, title, entity);

                if (logType != ExceptionLogType.Info)
                {
                    //文件名
                    //每天每种类型的日志一个文件
                    string fileName = string.Format("{0}_{1}_{2}.log", DateTime.Now.ToString("yyyyMMdd"), logType.ToString(), module);
                    string fullPath = BasePath + fileName;
                    if (!synLogFile.ContainsKey(fullPath))
                    {
                        lock (synLogFile)
                        {
                            synLogFile.Add(fullPath, new object());
                        }
                    }
                    lock (synLogFile[fullPath])
                    {
                        using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.Default))
                        {
                            sw.WriteLine(entity.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteFetal(ex, module, title);
            }
        }

        private static string _ApplicationName;

        /// <summary>
        /// 应用程序名
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                if (_ApplicationName == null)
                {
                    _ApplicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                    if (_ApplicationName == null)
                    {
                        _ApplicationName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    }
                }
                return _ApplicationName;

            }
        }

        /// <summary>
        /// 文件并发锁
        /// </summary>
        private static Dictionary<string, object> synLogFile = new Dictionary<string, object>();
        private static string _basePath;
        private static object synObject = new object();

        /// <summary>
        /// 记录异常记录的路径
        /// </summary>
        private static string BasePath
        {
            get
            {
                if (_basePath == null)
                {
                    lock (synObject)
                    {
                        string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                        if (basePath == null)
                        {
                            basePath = AppDomain.CurrentDomain.BaseDirectory;
                        }
                        basePath = basePath + @"\log\";
                        basePath = basePath.Replace(@"\\", @"\");
                        if (!Directory.Exists(basePath))
                        {
                            Directory.CreateDirectory(basePath);
                        }
                        _basePath = basePath;
                    }
                }
                return _basePath;
            }
        }
    }
}
