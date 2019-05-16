using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Dtos.Common;
using Zupan.CodeReview.Dtos.Modbus.Combinations;

namespace Zupan.CodeReview.Services.Interfaces.Modbus
{
    public interface IModbusDeviceService
    {
        PagedSet<ModbusDeviceAndContactsDto> GetPagedMoteAndContactDtos(Paging properties);

        ModbusDeviceAndContactsDto InsertAndReturnMoteAndContacts(ModbusDeviceAndContactsDto moteAndContactsDto);

        ModbusDeviceAndContactsDto UpdateMoteAndContactDtosRemote(ModbusDeviceAndContactsDto moteAndContactsDto);

        bool DeleteMoteAndContacts(int moteId);
    }
}