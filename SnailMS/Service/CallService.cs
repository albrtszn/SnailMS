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

        public CallDto ConvertCallToDto(Call Call)
        {
            return new CallDto
            {
                Id = Call.Id,
                UserId = Call.UserId,
                Number = Call.Number,
                StartTime = Call.StartTime,
                EndTime = Call.EndTime
            };
        }
        public Call ConvertDtoToCall(CallDto CallDto)
        {
            return new Call
            {
                Id = CallDto.Id,
                UserId = CallDto.UserId,
                Number = CallDto.Number,
                StartTime = CallDto.StartTime,
                EndTime = CallDto.EndTime
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
        public CallDto GetCallDtoById(string id)
        {
            return ConvertCallToDto(data.Calls.GetCallById(id));
        }
        public void DeleteCallById(string id)
        {
            data.Calls.DeleteCallById(id);
        }
        public void SaveCallDto(CallDto CallDtoToSave)
        {
            data.Calls.SaveCall(ConvertDtoToCall(CallDtoToSave));
        }
    }
}
