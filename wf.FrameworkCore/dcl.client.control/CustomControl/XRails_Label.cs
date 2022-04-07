using System.Windows.Forms;
using System.Drawing;

namespace Lis.CustomControls
{
    public class XRails_Label : Label
    {
        public XRails_Label()
        {
            Font = new Font("Segoe UI", 9, FontStyle.Regular);
            BackColor = Color.Transparent;
            ForeColor = ColorTranslator.FromHtml("#72767F");
            Cursor = Cursors.Arrow;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }
    }
}