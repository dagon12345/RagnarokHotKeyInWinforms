﻿using Domain.Constants;
using Domain.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using Infrastructure.Utilities;
using ApplicationLayer.Singleton.RagnarokSingleton;
using System.Drawing;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class Ahk : IAction
    {
        //Import the mouse_event functio from the Windows API
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        //Keybinding event
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const string ACTION_NAME = "AHK20";
        private ThreadUtility ThreadUtility;
        public const string COMPATABILITY = "ahkCompatability";
        public const string SPEED_BOOST = "ahkSpeedBoost";
        public Dictionary<string, KeyConfig> AhkEntries { get; set; } = new Dictionary<string, KeyConfig>();
        public Dictionary<string, KeyConfigOthers> AhkEntriesOthers { get; set; } = new Dictionary<string, KeyConfigOthers>();
        public int AhkDelay { get; set; }
        public bool mouseFlick { get; set; }
        public bool noShift { get; set; }
        public string ahkMode { get; set; }

        public Ahk()
        {
            //For account new account to avoid errors
            ahkMode = COMPATABILITY;
        }



        public void Stop()
        {
            if (ThreadUtility != null)
            {
                ThreadUtility.Stop();
            }

        }
        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (ThreadUtility != null)
                {
                    ThreadUtility.Stop();
                }
                this.ThreadUtility = new ThreadUtility(_ => AHKThreadExecution(roClient)); // ✅ Passes token and roClient
            }
        }

        public void AHKThreadExecution(Client roClient)
        {
            if (roClient == null)
            {
                Console.WriteLine("Client not found. Exiting thread.");
                return;
            }

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

            Task.Delay(50).Wait();

        }

        private void _AHKCompatability(Client roClient, KeyConfig config, Keys thisk)
        {
            Func<int, int> send_click;

            //Send Event Directly to Window Via PostMessage
            send_click = (evt) =>
            {
                InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                return 0;
            };
            if (this.mouseFlick)
            {
                while (Keyboard.IsKeyDown(config.key))
                {
                    InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
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
                    InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                    send_click(0);
                    Thread.Sleep(this.AhkDelay);
                }
            }
        }
        private void _AHKNoClick(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.key))
            {
                InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                Thread.Sleep(this.AhkDelay);
            }
        }
        private void _AHKSpeedBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.key))
            {
                Point cursorPos = System.Windows.Forms.Cursor.Position;
                InteropUtility.PostMessage(roClient.process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
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
