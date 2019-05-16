using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Dtos.Modbus.Combinations;

namespace Zupan.CodeReview.EventBusContracts.Modbus
{
    public interface EditMoteAndContactsRequest
    {
        ModbusDeviceAndContactsDto ModbusDeviceAndContactsDto { get; set; }
    }
}
