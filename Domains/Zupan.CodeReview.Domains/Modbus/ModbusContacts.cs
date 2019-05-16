using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zupan.CodeReview.Domains.Modbus
{
    public class ModbusContacts : BaseEntity
    {
        public string Guid { get; set; }
        public int ModbusDeviceId { get; set; }
        public int ChannelNumber { get; set; }
        public int MessageTypeId { get; set; }

        [ForeignKey("ModbusDeviceId")]
        public virtual ModbusDevices ModbusDevice { get; set; }

        [ForeignKey("MessageTypeId")]
        public virtual ModbusContactConfigurations MessageType { get; set; }

    }
}
