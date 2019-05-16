using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Modbus.Combinations
{
    public class ModbusDeviceAndContactsDto
    {
        public ModbusDevicesDto ModbusDeviceDto { get; set; }
        public IEnumerable<ModbusContactsDto> ModbusContactDtos { get; set; }
    }
}
