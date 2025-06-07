using RagnarokHotKeyInWinforms.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

namespace Domain.Model.JsonModels
{
    public class StatusRecoverySetting
    {
        public Dictionary<EffectStatusIDs, Key> buffMapping { get; set; } = new Dictionary<EffectStatusIDs, Key>();
        public int delay { get; set; } = 1;

        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }
            if (FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }
    }

}
