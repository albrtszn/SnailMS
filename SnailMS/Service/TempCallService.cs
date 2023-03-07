using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class TempCallService
    {
        private Data data;
        public TempCallService(Data _data)
        {
            data = _data;
        }

        public TempCallDto ConvertTempCallToDto(TempCall tempCall)
        {
            return new TempCallDto
            {
                Id = tempCall.Id,
                UserId = tempCall.UserId,
                Number = tempCall.Number,
                StartTime = tempCall.StartTime,
                EndTime = tempCall.EndTime
            };
        }
        public TempCall ConvertDtoToTempCall(TempCallDto tempCallDto)
        {
            return new TempCall
            {
                Id = tempCallDto.Id,
                UserId = tempCallDto.UserId,
                Number = tempCallDto.Number,
                StartTime = tempCallDto.StartTime,
                EndTime = tempCallDto.EndTime
            };
        }

        public IEnumerable<TempCallDto> GetAllTempCallDto()
        {
            List<TempCallDto> tempCallDtos = new List<TempCallDto>();
            List<TempCall> tempCalls = data.TempCalls.GetAllTempCalls().ToList();
            if (tempCalls != null)
            {
                foreach (TempCall tempCall in tempCalls)
                {
                    tempCallDtos.Add(ConvertTempCallToDto(tempCall));
                }
            }
            return tempCallDtos;
        }
        public TempCallDto GetTempCallDtoById(string id)
        {
            return ConvertTempCallToDto(data.TempCalls.GetTempCallById(id));
        }
        public void DeleteTempCallById(string id)
        {
            data.TempCalls.DeleteTempCallById(id);
        }
        public void SaveTempCallDto(TempCallDto tempCallDtoToSave)
        {
            data.TempCalls.SaveTempCall(ConvertDtoToTempCall(tempCallDtoToSave));
        }
    }
}
