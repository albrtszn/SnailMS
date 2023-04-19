using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class CallService
    {
        private Data data;
        public CallService(Data _data)
        {
            data = _data;
        }

        public CallDto? ConvertCallToDto(Call Call)
        {
            if (Call == null) return null;
            return new CallDto
            {
                Id = Call.Id,
                UserId = Call.UserId,
                Number = Call.Number,
                StartTime = Call.StartTime,
                EndTime = Call.EndTime,
                ManagerId = Call.ManagerId
            };
        }
        public Call? ConvertDtoToCall(CallDto CallDto)
        {
            if (CallDto == null) return null;
            return new Call
            {
                Id = CallDto.Id,
                UserId = CallDto.UserId,
                Number = CallDto.Number,
                StartTime = CallDto.StartTime,
                EndTime = CallDto.EndTime,
                ManagerId = CallDto.ManagerId
            };
        }

        public IEnumerable<CallDto> GetAllCallDto()
        {
            List<CallDto> CallDtos = new List<CallDto>();
            List<Call> Calls = data.Calls.GetAllCalls().ToList();
            if (Calls != null)
            {
                foreach (Call Call in Calls)
                {
                    CallDtos.Add(ConvertCallToDto(Call));
                }
            }
            return CallDtos;
        }
        public CallDto? GetCallDtoById(string id)
        {
            var callDto = ConvertCallToDto(data.Calls.GetCallById(id));
            if (callDto != null)
            {
                return callDto;
            }
            return null;
        }
        public void DeleteCallById(string id)
        {
            data.Calls.DeleteCallById(id);
        }
        public void SaveCallDto(CallDto CallDtoToSave)
        {
            if (GetCallDtoById(CallDtoToSave.Id) != null)
            {
                DeleteCallById(CallDtoToSave.Id);
            }
            var call = ConvertDtoToCall(CallDtoToSave);
            call.Manager = data.Managers.GetManagerById(call.ManagerId);
            data.Calls.SaveCall(call);
        }
    }
}
