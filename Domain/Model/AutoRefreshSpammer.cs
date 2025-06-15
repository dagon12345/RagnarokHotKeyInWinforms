using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace RagnarokHotKeyInWinforms.Model
{
    public class AutoRefreshSpammer: Action
    {
        private string ACTION_NAME = "AutoRefreshSpammer";
        private _4RThread thread;
        public int refreshDelay { get; set; } = 5;
        public Key refreshKey { get; set; }

        public AutoRefreshSpammer()
        {

        }

        public void Start()
        {
        }

        public void AutorefreshThreadExecution(Client roClient)
        {
            if(this.refreshKey != Key.None)
            {
                Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_HOTKEY_MSG_ID, (Keys)Enum.Parse(typeof(Keys), this.refreshKey.ToString()), 0);
            }
            Task.Delay(50).Wait(); 
        }

        public void Stop()
        {
            if (thread != null)
            {
                thread.Stop();
                thread = null; // Optionally set to null after stopping
            }
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return ACTION_NAME;
        }
    }
}
