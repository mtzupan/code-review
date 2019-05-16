using AutoMapper;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zupan.CodeReview.Domains.Core;
using Zupan.CodeReview.Domains.Modbus;
using Zupan.CodeReview.Dtos.Common;
using Zupan.CodeReview.Dtos.Modbus;
using Zupan.CodeReview.Dtos.Modbus.Combinations;
using Zupan.CodeReview.EventBusContracts.Modbus;
using Zupan.CodeReview.Services.Interfaces.Modbus;
using Zupan.CodeReview.UnitOfWork;

namespace Zupan.CodeReview.Services.Modbus
{
    public class ModbusDeviceService : IModbusDeviceService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper Mapper { get; set; }
        public IBusControl Bus { get; set; }

        private int RPC_TIMEOUT_MS = 30000;
        private string RABBIT_CLUSTER = "RabbitMQ Cluster";

        public ModbusDeviceService(IUnitOfWork unitOfWork, IMapper mapper, IBusControl busControl)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            Bus = busControl;
        }

        #region Insert
        public ModbusDeviceAndContactsDto InsertAndReturnMoteAndContacts(ModbusDeviceAndContactsDto moteAndContactsDto)
        {
            var addedDeviceDto = InsertNoSaveAndReturnDevice(moteAndContactsDto.ModbusDeviceDto);
            var addedContactDtos = InsertNoSaveAndReturnContacts(addedDeviceDto.Id, moteAndContactsDto.ModbusContactDtos);
            var address = GetAddressOfEdge(addedDeviceDto.EdgeId);
            var timeout = TimeSpan.FromMilliseconds(RPC_TIMEOUT_MS);

            try
            {
                ExecuteAddMoteAndContactsReqAsync(addedDeviceDto, addedContactDtos, address, timeout).Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            UnitOfWork.GetModbusRepository<ModbusDevices>().Commit();
            UnitOfWork.GetModbusRepository<ModbusContacts>().Commit();

            var deviceAndContacts = new ModbusDeviceAndContactsDto()
            {
                ModbusDeviceDto = addedDeviceDto,
                ModbusContactDtos = addedContactDtos
            };

            return deviceAndContacts;
        }

        private async Task ExecuteAddMoteAndContactsReqAsync(ModbusDevicesDto deviceDto, IEnumerable<ModbusContactsDto> contactDtos, Uri address, TimeSpan timeOut)
        {
            var deviceAndContacts = new ModbusDeviceAndContactsDto()
            {
                ModbusDeviceDto = deviceDto,
                ModbusContactDtos = contactDtos
            };
            IRequestClient<AddDeviceAndContactsRequest, AddDeviceAndContactsResponse> client = new MessageRequestClient<AddDeviceAndContactsRequest, AddDeviceAndContactsResponse>(Bus, address, timeOut);
            var result = await client.Request(new
            {
                ModbusDeviceAndContactsDto = deviceAndContacts
            });
            if (!result.Success)
            {
                throw new Exception("Something failed on edge. Error Code: " + result.ErrorCode + ". Message:" + result.ErrorMessage + ". ");
            }
        }

        private ModbusDevicesDto InsertNoSaveAndReturnDevice(ModbusDevicesDto deviceDto)
        {
            deviceDto.DeviceGuid = Guid.NewGuid().ToString();
            var device = Mapper.Map<ModbusDevices>(deviceDto);
            var insertedDevice = UnitOfWork.GetModbusRepository<ModbusDevices>()
                .InsertAndReturnEntity(device, null, false);
            return Mapper.Map<ModbusDevicesDto>(insertedDevice);
        }

        private IEnumerable<ModbusContactsDto> InsertNoSaveAndReturnContacts(int deviceId, IEnumerable<ModbusContactsDto> contactDtos)
        {
            foreach(var contactDto in contactDtos)
            {
                contactDto.Guid = Guid.NewGuid().ToString();
                contactDto.ModbusDeviceId = deviceId;
            }
            var contacts = Mapper.Map<IEnumerable<ModbusContacts>>(contactDtos);
            var returnedContacts = UnitOfWork.GetModbusRepository<ModbusContacts>()
                .InsertAndReturnBatchEntities(contacts, null, false);
            return Mapper.Map<IEnumerable<ModbusContactsDto>>(returnedContacts);
        }
        #endregion Insert


        #region Delete
        public bool DeleteMoteAndContacts(int moteId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get
        public PagedSet<ModbusDeviceAndContactsDto> GetPagedMoteAndContactDtos(Paging properties)
        {

            throw new NotImplementedException();
        }
        #endregion

        #region
        public ModbusDeviceAndContactsDto UpdateMoteAndContactDtosRemote(ModbusDeviceAndContactsDto moteAndContactsDto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region RPC Helpers
        private Uri GetAddressOfEdge(int edgeId)
        {
            var edgeGuid = UnitOfWork.GetCoreRepository<Edges>()
                .GetSingle(x => x.Id == edgeId)
                ?.EdgeGuid;
            if (edgeGuid == null)
            {
                throw new Exception("Given EdgeId not present in the database.");
            }
            var brokerEndpoint = UnitOfWork.GetCoreRepository<DistributedServices>()
                .GetSingle(x => x.Name == RABBIT_CLUSTER)
                ?.Endpoint;
            if(brokerEndpoint == null)
            {
                throw new Exception("Unable to find information on RabbitMQ Cluster Endpoint. Contact system administrator.");
            }
            return new Uri(brokerEndpoint + edgeGuid);
        }
        #endregion
    }
}