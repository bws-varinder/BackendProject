using System;
using System.Linq;
using SqlDapper;
using BusinessObject.Admin;
using DataRepository.Common;
using Dapper;
using System.Data;
using System.Collections.Generic;

namespace DataRepository.Admin
{
    public class UsersRepository : DapperRepository
    {
        public UsersRepository(IDatabasecontext dbcontext)
            : base(dbcontext)
        { }

        public ResultObject CreateUpdateEvent(EventObject obj)
        {
            ResultObject _returnObj = new ResultObject();
            try
            {
                DynamicParameters _list_params = CommonRepository.GetLogParameters();

                _list_params.Add("@EventId", obj.EventId);
                _list_params.Add("@EventName", obj.EventName);
                _list_params.Add("@EventTime", obj.EventTime);
                _list_params.Add("@CategoryName", obj.CategoryName);
                _list_params.Add("@Address1", obj.Address1);
                _list_params.Add("@Address2", obj.Address2);
                _list_params.Add("@Address3", obj.Address3);
                _list_params.Add("@Address4", obj.Address4);
                _list_params.Add("@EventDescription", obj.EventDescription);
                _list_params.Add("@YoutubeUrl", obj.YoutubeUrl);

                var result = databaseContext.Query<int>("CreateUpdateEvents", _list_params, null, CommandType.StoredProcedure);
                if (result != null)
                {
                    _returnObj.Status = _list_params.Get<int>("@Stat");
                    _returnObj.Message = _list_params.Get<string>("@Message");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _returnObj;
        }

        public List<EventObject> fetchEvents(int EventId)
        {
            List<EventObject> _list = new List<EventObject>();
            try
            {
                DynamicParameters _list_params = CommonRepository.GetLogParameters();

                _list_params.Add("@EventId", EventId);

                _list = (List<EventObject>)databaseContext.Query<EventObject>("fetchEvents", _list_params, null, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw;
            }
            return _list;
        }


        public ResultObject DeleteEvent(int EventId)
        {
            ResultObject _returnObj = new ResultObject();
            try
            {
                DynamicParameters _list_params = CommonRepository.GetLogParameters();

                _list_params.Add("@EventId", EventId);

                var result = databaseContext.Query<int>("deleteEvent", _list_params, null, CommandType.StoredProcedure);
                if (result != null)
                {
                    _returnObj.Status = _list_params.Get<int>("@Stat");
                    _returnObj.Message = _list_params.Get<string>("@Message");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _returnObj;
        }
    }
}
