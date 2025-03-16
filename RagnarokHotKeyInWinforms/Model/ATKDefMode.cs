using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagnarokHotKeyInWinforms.Model
{
    public enum ATKDEFEnum
    {
        ATK = 0,
        DEF = 1
    }
    public class ATKDefMode : Action
    {
        public static string ACTION_NAME_ATKDEF = "ATKDEFMode";
        private _4RThread thread;

        public string GetActionName()
        {
            throw new NotImplementedException();
        }

        public string GetConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
