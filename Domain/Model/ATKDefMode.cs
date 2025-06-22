using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;


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
        public int ahkDelay { get; set; } = 10;
        public int switchDelay { get; set; } = 50;
        public Key keySpammer { get; set; }
        public bool keySpammerWithClick { get; set; } = true;
        public Dictionary<string, Key> defKeys { get; set; } = new Dictionary<string, Key>();
        public Dictionary<string, Key> atkKeys { get; set; } = new Dictionary<string, Key>();
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out System.Drawing.Point lpPoint);
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        private int PX_MOV = Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK;

        public string GetActionName()
        {
            return ACTION_NAME_ATKDEF;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                this.thread = new _4RThread(_ => AttacKDefAHKThreadExecution(roClient));
            }
        }

        private void MoveMouseInCircle(int radius, double angle)
        {
            GetCursorPos(out Point pos);
            int x = pos.X + (int)(radius * Math.Cos(angle));
            int y = pos.Y + (int)(radius * Math.Sin(angle));
            SetCursorPos(x, y);
        }
        public void AttacKDefAHKThreadExecution(Client roClient)
        {
            Keys thisk = toKeys(keySpammer);
            //Is not equal to none and iskey is clicked
            if(this.keySpammer != Key.None && Keyboard.IsKeyDown(this.keySpammer))
            {
                foreach(Key key in atkKeys.Values)
                {
                    Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0);
                    Thread.Sleep(this.switchDelay);
                }
                //This is circle spammer
                if (this.keySpammerWithClick)
                {
                    double angle = 0;
                    double increment = Math.PI / 15; // adjust smoothness
                    int radius = 2; // subtle shake radius

                    while (Keyboard.IsKeyDown(keySpammer))
                    {
                        Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                        Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                        Thread.Sleep(1);
                        Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);

                        MoveMouseInCircle(radius, angle);
                        angle += increment;

                        Thread.Sleep(this.ahkDelay);
                    }


                }
                else
                {
                    while(Keyboard.IsKeyDown(keySpammer))
                    {
                        Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYUP_MSG_ID, thisk, 0);
                        Thread.Sleep(this.ahkDelay);
                    }
                }
                foreach(Key key in defKeys.Values)
                {
                    //Equip def items
                    Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0);
                    Thread.Sleep(this.switchDelay);
                }
            }
        }
        public void AddSwitchItem(string dictKey, Key k, ATKDEFEnum type)
        {
            Dictionary<string, Key> copy = type == ATKDEFEnum.DEF? this.defKeys: this.atkKeys;
            if(copy.ContainsKey(dictKey))
            {
                RemoveSwitchEntry(dictKey, type);
            }
            if(k != Key.None)
            {
                copy.Add(dictKey, k);
            }
        }
        public void RemoveSwitchEntry(string dictKey, ATKDEFEnum type)
        {
            Dictionary<string, Key> copy = type == ATKDEFEnum.DEF ? this.defKeys : this.atkKeys;
            copy.Remove(dictKey);
        }
        private Keys toKeys(Key k)
        {
            return (Keys)Enum.Parse(typeof(Keys), k.ToString());
        }

        public void Stop()
        {
            thread.Stop();
        }
    }
}
