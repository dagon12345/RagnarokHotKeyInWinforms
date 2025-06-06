using Newtonsoft.Json;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms.Model
{
    public class UserPreferences : Action
    {
        public string ACTION_NAME = "UserPreferences";
        public string toggleStateKey { get; set; } //= Keys.End.ToString();
        public UserPreferences()
        {

        }

        public string GetActionName()
        {
            return ACTION_NAME;
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
