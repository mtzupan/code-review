using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Dtos.Modbus;

namespace Zupan.CodeReview.EventBusContracts.Modbus
{
    public interface DeleteMoteAndContactsRequest
    {
        ModbusDevicesDto ModbusDeviceDto { get; set; }
    }
}
