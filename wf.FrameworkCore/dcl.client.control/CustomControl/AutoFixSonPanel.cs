using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lis.CustomControls
{
    public class AutoFixSonPanel : Panel
    {
        private bool _autoSetFixSon = false;
        [Browsable(true)] 
        [DefaultValue(false), Description("是否平均分配子控件的大小")]
        public bool AutoSetFixSon
        {
            get { return _autoSetFixSon; }
            set
            {
                _autoSetFixSon = value;
                SetSonSize();
                base.Invalidate();
            }
        }

        Fixdirection _AutoFixdirection = Fixdirection.Vertical;
        [Browsable(true)]
        [Description("自动填充方向")]
        public Fixdirection AutoFixdirection
        {
            get { return _AutoFixdirection; }
            set
            {
                _AutoFixdirection = value;
                SetSonSize();
                base.Invalidate();
            }
        }


        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            SetSonSize();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            SetSonSize();
        }

        private void SetSonSize()
        {
            if (this.HasChildren
                && this.Parent != null
                && _autoSetFixSon)
            {
                foreach (Control cl in this.Controls)
                {
                    if (_AutoFixdirection == Fixdirection.Vertical)
                    {
                        cl.Width = this.Parent.Width;
                        cl.Height = this.Parent.Height / this.Controls.Count;
                    }
                    else
                    {
                        cl.Width = this.Parent.Width / this.Controls.Count;
                        cl.Height = this.Parent.Height;
                    }
                }
            }
        }


        public enum Fixdirection
        {
            Horizontal = 0,//水平
            Vertical = 1//垂直
        }
    }
}
