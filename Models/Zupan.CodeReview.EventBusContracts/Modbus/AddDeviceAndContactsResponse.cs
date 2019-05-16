using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Dtos.Modbus.Combinations;

namespace Zupan.CodeReview.EventBusContracts.Modbus
{
    public interface AddDeviceAndContactsResponse
    {
        bool Success { get; set; }
        int ErrorCode { get; set; }
        string ErrorMessage { get; set; }
    }
}
