using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Modbus
{
    public class ModbusDevicesDto
    {
        public int Id { get; set;  }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string MACAdress { get; set; }
        public string IpAddress { get; set; }
        public int EdgeId { get; set; }
        public int MachineId { get; set; }
        public int PollInterval { get; set; }
        public bool IsOnline { get; set; }
        public int Port { get; set; }
        public string DeviceGuid { get; set; }
    }
}
