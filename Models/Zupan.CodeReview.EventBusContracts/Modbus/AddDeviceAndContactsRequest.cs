using System;
using Zupan.CodeReview.Dtos.Modbus.Combinations;

namespace Zupan.CodeReview.EventBusContracts.Modbus
{
    public interface AddDeviceAndContactsRequest
    {
        ModbusDeviceAndContactsDto ModbusDeviceAndContactsDto { get; set; }
    }
}
