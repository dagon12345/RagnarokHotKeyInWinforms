using System;

namespace ApplicationLayer.Dto.RagnarokDto
{
    public class ClientDto
    {
        public int index { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string hpAddress { get; set; }
        public string nameAddress { get; set; }
        public int hpAddressPointer { get; set; }
        public int nameAddressPointer { get; set; }

        public ClientDto()
        { 
        }

        public ClientDto(string name, string description, string hpAddress, string nameAddress)
        {
            this.name = name;
            this.description = description;
            this.hpAddress = hpAddress;
            this.nameAddress = nameAddress;

            this.hpAddressPointer = Convert.ToInt32(hpAddress, 16);
            this.nameAddressPointer = Convert.ToInt32(nameAddress, 16);
        }


    }


}
