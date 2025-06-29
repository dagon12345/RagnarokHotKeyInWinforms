using Domain.Constants;
using Domain.Interface;
using Infrastructure.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class AutoRefreshSpammer : IAction
    {
        private string ACTION_NAME = "AutoRefreshSpammer";
        private ThreadUtility ThreadUtility;
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
            if (this.refreshKey != Key.None)
            {
                InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_HOTKEY_MSG_ID, (Keys)Enum.Parse(typeof(Keys), this.refreshKey.ToString()), 0);
            }
            Task.Delay(50).Wait();
        }

        public void Stop()
        {
            if (ThreadUtility != null)
            {
                ThreadUtility.Stop();
                ThreadUtility = null; // Optionally set to null after stopping
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
