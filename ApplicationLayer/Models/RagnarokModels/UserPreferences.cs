using Domain.Interface;
using Newtonsoft.Json;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class UserPreferences : IAction
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
