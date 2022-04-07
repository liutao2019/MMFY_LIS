using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;
using lis.client.control.LISControl;

namespace lis.client.control.LISEditor
{
    public class LISTextEdit : DevExpress.XtraEditors.TextEdit, ILISControl
    {
        string prevText;

        protected override void OnLoaded()
        {
            base.OnLoaded();
            prevText = string.Empty;
        }

        protected override void OnLeave(EventArgs e)
        {
            if (prevText != this.Text)
            {
                OnTextLeaveChanged();
            }

            base.OnLeave(e);

        }

        public override object EditValue
        {
            get
            {
                return base.EditValue;
            }
            set
            {
                if (base.EditValue != value)
                {
                    if (base.EditValue != null && base.EditValue != DBNull.Value)
                    {
                        prevText = base.EditValue.ToString();
                    }
                    else
                    {
                        prevText = string.Empty;
                    }
                }

                //if (value != null && value != DBNull.Value)
                //{
                //    prevText = value.ToString();
                //}
                //else
                //{
                //    prevText = string.Empty;
                //}
                base.EditValue = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    if (base.Text != null)
                    {
                        prevText = base.Text;
                    }
                    else
                    {
                        prevText = string.Empty;
                    }
                }

                base.Text = value;
            }
        }

        #region Event
        public delegate void TextLeaveChangedEventHandler(object sender, TextChangedEventArgs args);

        [Category("Events")]
        [Browsable(true)]
        public event TextLeaveChangedEventHandler TextLeaveChanged;
        protected virtual void OnTextLeaveChanged()
        {
            if (TextLeaveChanged != null)
            {
                TextLeaveChanged(this, new TextChangedEventArgs(this.prevText, this.Text));
                this.prevText = this.Text;
            }
        }

        public class TextChangedEventArgs : EventArgs
        {
            public TextChangedEventArgs(string prev, string curr)
            {
                this.PrevText = prev;
                this.CurrentText = curr;
            }

            public string PrevText { get; private set; }
            public string CurrentText { get; private set; }
        }
        #endregion


        public delegate void EnterKeyDownEventHandler(object sender, EventArgs args);

        [Category("Events")]
        [Browsable(true)]
        public event EnterKeyDownEventHandler EnterKeyDown;

        protected virtual void OnEnterKeyDown()
        {
            if (EnterKeyDown != null)
            {
                EnterKeyDown(this, EventArgs.Empty);
            }
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                OnEnterKeyDown();
            }
        }

        public object Value
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value != null && value != DBNull.Value)
                {
                    this.Text = value.ToString();
                }
                else
                {
                    this.Text = string.Empty;
                }
            }
        }
    }
}
