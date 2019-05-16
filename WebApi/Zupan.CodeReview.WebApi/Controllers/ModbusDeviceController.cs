using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zupan.CodeReview.Dtos.Common;
using Zupan.CodeReview.Dtos.Modbus.Combinations;
using Zupan.CodeReview.Services.Interfaces.Modbus;
using Zupan.CodeReview.WebApi.Filters;

namespace Zupan.CodeReview.WebApi.Controllers
{
    [Route("api/ModbusDevice")]
    [Authorize]
    public class ModbusDeviceController : Controller
    {
        private readonly IModbusDeviceService _deviceService;

        public ModbusDeviceController(IModbusDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost]
        [ValidateAttributes]
        [Route("InsertAndReturnMoteAndContactsRemote")]
        [SwaggerResponse(typeof(TalosResult<ModbusDeviceAndContactsDto>))]
        public IActionResult InsertAndReturnMoteAndContactsRemote([FromBody] ModbusDeviceAndContactsDto moteAndContactsDto)
        {
            var addedObject = _deviceService.InsertAndReturnMoteAndContacts(moteAndContactsDto);
            var response = new TalosResult<ModbusDeviceAndContactsDto>
            {
                Result = addedObject,
                Success = addedObject != null
            };
            return new ObjectResult(response);
        }
    }
}
