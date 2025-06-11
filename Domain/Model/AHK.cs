using Newtonsoft.Json;
using RagnarokHotKeyInWinforms.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace RagnarokHotKeyInWinforms.Model
{
    public class KeyConfig
    {
        public Key key { get; set; }
        public bool ClickActive { get; set; }

        public KeyConfig(Key key, bool clickActive)
        {
            this.key = key;
            ClickActive = clickActive;
        }

    }

    public class KeyConfigOthers
    {
        public bool ClickActive { get; set; }

        public KeyConfigOthers(bool clickActive)
        {
            ClickActive = clickActive;
        }

    }

    public class AHK : Action
    {
        //Import the mouse_event functio from the Windows API
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        //Keybinding event
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const string ACTION_NAME = "AHK20";
        private _4RThread thread;
        public const string COMPATABILITY = "ahkCompatability";
        public const string SPEED_BOOST = "ahkSpeedBoost";
        public Dictionary<string, KeyConfig> AhkEntries { get; set; } = new Dictionary<string, KeyConfig>();
        public Dictionary<string, KeyConfigOthers> AhkEntriesOthers { get; set; } = new Dictionary<string, KeyConfigOthers>();
        public int AhkDelay { get; set; }
        public bool mouseFlick { get; set; }
        public bool noShift { get; set; } 
        public string ahkMode { get; set; }

        public AHK()
        {

        }



        public void Stop()
        {
            if (thread != null)
            {
                thread.Stop();
            }

        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (thread != null)
                {
                    thread.Stop();
                }
                this.thread = new _4RThread(_ => AHKThreadExecution(roClient));
            }
        }

        private int AHKThreadExecution(Client roClient)
        {
            if (ahkMode.Equals(COMPATABILITY))
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.key.ToString());
                    if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        if (config.ClickActive && Keyboard.IsKeyDown(config.key))
                        {
                            if (noShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY, 0);
                            _AHKCompatability(roClient, config, thisk);
                            if (noShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY | Constants.KEYEVENTF_KEYUP, 0);
                        }
                        else
                        {
                            this._AHKNoClick(roClient, config, thisk);
                        }
                    }
                }
            }
            else
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.key.ToString());
                    this._AHKSpeedBoost(roClient, config, thisk);
                }
            }
            return 0;
        }

        private void _AHKCompatability(Client roClient, KeyConfig config, Keys thisk)
        {
            Func<int, int> send_click;

            //Send Event Directly to Window Via PostMessage
            send_click = (evt) =>
            {
                Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                return 0;
            };
            if (this.mouseFlick)
            {
                while (Keyboard.IsKeyDown(config.key))
                {
                    Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                    send_click(0);
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                    Thread.Sleep(this.AhkDelay);
                }
            }
            else
            {
                while (Keyboard.IsKeyDown(config.key))
                {
                    Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                    send_click(0);
                    Thread.Sleep(this.AhkDelay);
                }
            }
        }
        private void _AHKNoClick(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.key))
            {
                Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                Thread.Sleep(this.AhkDelay);
            }
        }
        private void _AHKSpeedBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.key))
            {
                Point cursorPos = System.Windows.Forms.Cursor.Position;
                Interop.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                mouse_event(Constants.MOUSEEVENTF_LEFTDOWN, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                Thread.Sleep(1);
                mouse_event(Constants.MOUSEEVENTF_LEFTUP, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                Thread.Sleep(this.AhkDelay);
            }
        }

        public void AddAHKEntry(string chkboxName, KeyConfig value)
        {
            if (this.AhkEntries.ContainsKey(chkboxName))
            {
                RemoveAHKEntry(chkboxName);
            }
            this.AhkEntries.Add(chkboxName, value);
        }
        public void RemoveAHKEntry(string chkboxName)
        {
            this.AhkEntries.Remove(chkboxName);
        }


        public void AddAHKEntryKeyConfig(string chkboxName, KeyConfigOthers value)
        {
            if (this.AhkEntries.ContainsKey(chkboxName))
            {
                RemoveAHKEntry(chkboxName);
            }
            this.AhkEntriesOthers.Add(chkboxName, value);
        }
        public void RemoveAHKEntryKeyConfig(string chkboxName)
        {
            this.AhkEntriesOthers.Remove(chkboxName);
        }


        public string GetActionName()
        {
            return ACTION_NAME;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }



    }
}
