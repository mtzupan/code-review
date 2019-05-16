using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zupan.CodeReview.Domains.Core;
using Zupan.CodeReview.Domains.Modbus;
using Zupan.CodeReview.Dtos.Core;
using Zupan.CodeReview.Dtos.Modbus;

namespace Zupan.CodeReview.WebApi.Configurations
{
    public class AutoMapperProfiler : AutoMapper.Profile
    {
        public AutoMapperProfiler()
        {
            CreateMap<Users, UsersDto>();
            CreateMap<UsersDto, Users>();

            CreateMap<ModbusContacts, ModbusContactsDto>();
            CreateMap<ModbusContactsDto, ModbusContacts>();

            CreateMap<ModbusDevices, ModbusDevicesDto>();
            CreateMap<ModbusDevicesDto, ModbusDevices>();
        }
    }
}