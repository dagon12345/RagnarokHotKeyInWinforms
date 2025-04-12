using RagnarokHotKeyInWinforms.Utilities;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class AHKForm : Form, IObserver
    {
        public AHKForm(Subject subject)
        {
            InitializeComponent();
        }

        public void Update(ISubject subject)
        {
            throw new System.NotImplementedException();
        }
    }
}
