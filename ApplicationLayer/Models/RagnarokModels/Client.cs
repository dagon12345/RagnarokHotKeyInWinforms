using System.Diagnostics;
using System.Windows.Forms;
using System;
using ApplicationLayer.Dto.RagnarokDto;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationLayer.Singleton.RagnarokSingleton;
using Infrastructure.Utilities;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class Client
    {
        public Process process { get; }
        public string processName { get; private set; }
        private ProcessMemoryReader ProcessMemoryReader { get; set; }
        public int currentNameAddress { get; set; }
        public int currentHPBaseAddress { get; set; }
        private int statusBufferAddress { get; set; }
        private int _num = 0;

        public Client(string processName, int currentHPBaseAddress, int currentNameAddress)
        {
            this.currentNameAddress = currentNameAddress;
            this.currentHPBaseAddress = currentHPBaseAddress;
            this.processName = processName;
            this.statusBufferAddress = currentHPBaseAddress + 0x474;
        }

        public Client(ClientDto dto)
        {
            this.processName = dto.name;
            this.currentHPBaseAddress = Convert.ToInt32(dto.hpAddress, 16);
            this.currentNameAddress = Convert.ToInt32(dto.nameAddress, 16);
            this.statusBufferAddress = this.currentHPBaseAddress + 0x474;
        }

        public Client(string processName)
        {
            ProcessMemoryReader = new ProcessMemoryReader();
            string rawProcessName = processName.Split(new string[] { ".exe - " }, StringSplitOptions.None)[0];
            int choosenPID = int.Parse(processName.Split(new string[] { ".exe - " }, StringSplitOptions.None)[1]);

            foreach (Process process in Process.GetProcessesByName(rawProcessName))
            {
                if (choosenPID == process.Id)
                {
                    this.process = process;
                    ProcessMemoryReader.ReadProcess = process;
                    ProcessMemoryReader.OpenProcess();

                    try
                    {
                        Client c = GetClientByProcess(rawProcessName);

                        if (c == null) throw new Exception();

                        this.currentHPBaseAddress = c.currentHPBaseAddress;
                        this.currentNameAddress = c.currentNameAddress;
                        this.statusBufferAddress = c.statusBufferAddress;
                    }
                    catch
                    {
                        MessageBox.Show("This client is not supported. Only Spammers and macro will works.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.currentHPBaseAddress = 0;
                        this.currentNameAddress = 0;
                        this.statusBufferAddress = 0;
                    }

                    //Do not block spammer for non supported Versions

                }
            }
        }
        private string ReadMemoryAsString(int address)
        {
            byte[] bytes = ProcessMemoryReader.ReadProcessMemory((IntPtr)address, 40u, out _num);
            List<byte> buffer = new List<byte>(); //Need a list with dynamic size 
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0) break; //Check Nullability based ON ASCII Table

                buffer.Add(bytes[i]); //Add only bytes needed
            }

            return Encoding.Default.GetString(buffer.ToArray());

        }
        private uint ReadMemory(int address)
        {
            return BitConverter.ToUInt32(ProcessMemoryReader.ReadProcessMemory((IntPtr)address, 4u, out _num), 0);
        }
        public void WriteMemory(int address, uint intToWrite)
        {
            ProcessMemoryReader.WriteProcessMemory((IntPtr)address, BitConverter.GetBytes(intToWrite), out _num);
        }

        public void WriteMemory(int address, byte[] bytesToWrite)
        {
            ProcessMemoryReader.WriteProcessMemory((IntPtr)address, bytesToWrite, out _num);
        }

        public bool IsHpBelow(int percent)
        {
            return ReadCurrentHp() * 100 < percent * ReadMaxHp();
        }

        public bool IsSpBelow(int percent)
        {
            return ReadCurrentSp() * 100 < percent * ReadMaxSp();
        }

        public uint ReadCurrentHp()
        {
            return ReadMemory(this.currentHPBaseAddress);
        }

        public uint ReadCurrentSp()
        {
            return ReadMemory(this.currentHPBaseAddress + 8);
        }

        public uint ReadMaxHp()
        {
            return ReadMemory(this.currentHPBaseAddress + 4);
        }

        public string ReadCharacterName()
        {
            return ReadMemoryAsString(this.currentNameAddress);
        }

        public uint ReadMaxSp()
        {
            return ReadMemory(this.currentHPBaseAddress + 12);
        }

        public uint CurrentBuffStatusCode(int effectStatusIndex)
        {
            return ReadMemory(this.statusBufferAddress + effectStatusIndex * 4);
        }

        public Client GetClientByProcess(string processName)
        {

            foreach(Client c in ClientListSingleton.GetAll())
            {
                if (c.processName == processName)
                {
                    uint hpBaseValue = ReadMemory(c.currentHPBaseAddress);
                    if (hpBaseValue > 0) return c;
                }
            }
            return null;
        }

        public static Client FromDTO(ClientDto dto)
        {
            return ClientListSingleton.GetAll()
                .Where(c => c.processName == dto.name)
                .Where(c => c.currentHPBaseAddress == dto.hpAddressPointer)
                .Where(c => c.currentNameAddress == dto.nameAddressPointer).FirstOrDefault();
        }
    }
}
