using DevExpress.XtraEditors;

namespace dcl.client.control
{
    public partial class CustomForm : XtraForm
    {
        public CustomForm()
        {
            InitializeComponent();
        }

        public virtual void HotKeyProc(PawayHotKeyMessage KeyMessage)
        { }

        public virtual void Init(object Sender)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        /// <summary>
        /// 热键
        /// </summary>
        public enum PawayHotKeyMessage
        {
            Control = 17,
            Up = 38,
            Down = 40,
            F1 = 112,
            F2 = 113,
            F3 = 114,
            F4 = 115,
            F5 = 116,
            F6 = 117,
            F7 = 118,
            F8 = 119,
            F9 = 120,
            F10 = 121,

            Control_ = 1000,
            Control_F1 = 1112,
            Control_F2 = 1113,
        }

        /// <summary>
        /// 输入状态
        /// </summary>
        public enum InputStatus
        {
            /// <summary>
            /// 查询状态
            /// </summary>
            Query = 0,

            /// <summary>
            /// 扫描状态
            /// </summary>
            Scanning = 1,

            /// <summary>
            /// 编辑状态
            /// </summary>
            Edit = 2,

            /// <summary>
            /// 查看状态
            /// </summary>
            View = 3,

            /// <summary>
            /// 插入状态
            /// </summary>
            Insert = 4,

            /// <summary>
            /// 只读状态
            /// </summary>
            Read = 5
        }

    }
}
