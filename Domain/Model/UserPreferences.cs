using Newtonsoft.Json;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Model
{
    public class UserPreferences : Action
    {
        public string actionName { get; set; } = "UserPreferences";
        public string toggleStateKey { get; set; }

        public UserPreferences()
        {

        }

        public string GetActionName()
        {
            return actionName;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Start()
        {
            //Leave empty
        }

        public void Stop()
        {
            //Leave empty
        }
    }
}
