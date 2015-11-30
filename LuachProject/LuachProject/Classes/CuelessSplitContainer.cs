using System.Windows.Forms;

namespace LuachProject
{
    public partial class CuelessSplitContainer : SplitContainer
    {
        private bool _painting;

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        protected override bool ShowKeyboardCues
        {
            get
            {
                return false;
            }
        }

        public override bool Focused
        {
            get { return _painting ? false : base.Focused; }
        }

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