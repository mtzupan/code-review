using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Modbus
{
    public class ModbusContactsDto
    {
        public string Guid { get; set; }
        public int ModbusDeviceId { get; set; }
        public int ChannelNumber { get; set; }
        public int MessageTypeId { get; set; }
    }
}
