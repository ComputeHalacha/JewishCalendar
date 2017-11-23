using System.Windows.Forms;

namespace LuachProject
{
    public partial class CuelessSplitContainer : SplitContainer
    {
        private bool _painting;

        protected override bool ShowFocusCues => false;

        protected override bool ShowKeyboardCues => false;

        public override bool Focused => _painting ? false : base.Focused;

        protected override void OnPaint(PaintEventArgs e)
        {
            _painting = true;

            try
            {
                base.OnPaint(e);
            }
            finally
            {
                _painting = false;
            }
        }
    }
}