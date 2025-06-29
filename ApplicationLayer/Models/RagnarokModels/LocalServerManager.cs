using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using ApplicationLayer.Dto.RagnarokDto;
using System.Linq;
using ApplicationLayer.Singleton.RagnarokSingleton;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class LocalServerManager
    {
        private static readonly string localServerName = "supported_servers.json";

        public static void AddServer(string hpAddress, string nameAddress, string processName)
        {
            if (!isValid(hpAddress))
            {
                throw new ArgumentException("HP Address is Invalid. Please type a valid Hex value.");
            }

            if (!isValid(nameAddress))
            {
                throw new ArgumentException("Name Address is Invalid. Please type a valid Hex value.");
            }
            ClientDto dto = new ClientDto(processName, null, hpAddress, nameAddress);
            ClientListSingleton.AddClient(new Client(dto));

            List<ClientDto> clients = GetLocalClients();
            clients.Add(dto);
            OverwriteLocalFile(clients);

            /**
             * Cases
             * 1. Local file don't exists
             *  Solution: Create a empty file
             * 2. Local file exists with wrong syntax
             *  Solution: Remove invalid file and create new one
             * 3. Arquivo Local existe e é válido
             */
        }

        public static void RemoveClient(ClientDto dto)
        {
            List<ClientDto> clients = GetLocalClients();
            clients.RemoveAt(dto.index);
            OverwriteLocalFile(clients);
            ClientListSingleton.RemoveClient(Client.FromDTO(dto));
        }

        private static void OverwriteLocalFile(List<ClientDto> clients)
        {
            string output = JsonConvert.SerializeObject(clients, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(localServerName, string.Empty);
            File.WriteAllText(localServerName, output);
        }


        private static string LoadLocalServerFile()
        {
            if (!File.Exists(localServerName))
            {
                string startJson = "[]";
                FileStream f = File.Create(localServerName);
                f.Close();
                File.WriteAllText(localServerName, startJson);
                return startJson;
            }
            string json = File.ReadAllText(localServerName);
            return json;
        }

        public static List<ClientDto> GetLocalClients()
        {
            string localServers = LoadLocalServerFile();

            if (string.IsNullOrEmpty(localServers)) return new List<ClientDto>();

            try
            {
                return JsonConvert.DeserializeObject<List<ClientDto>>(localServers);
            }
            catch
            {
                return new List<ClientDto>();
            }
        }

        private static bool isValid(IEnumerable<char> chars)
        {
            return IsHex(chars) && chars.Count() == 8;
        }

        public static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }
    }
}
