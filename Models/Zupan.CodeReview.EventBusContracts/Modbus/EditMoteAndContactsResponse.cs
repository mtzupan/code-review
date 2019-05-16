using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.EventBusContracts.Modbus
{
    public interface EditMoteAndContactsResponse
    {
        bool Success { get; set; }
        int ErrorCode { get; set; }
        string ErrorMessage { get; set; }
    }
}
