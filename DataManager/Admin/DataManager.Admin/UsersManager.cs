using BusinessObject.Admin;
using DataRepository.Admin;
using EncryptHelper;
using SqlDapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataManager.Admin
{
    public class UsersManager
    {
        UsersRepository _objUserRepository = new UsersRepository(BaseDapper.CreateInstance());

        public ResultObject CreateUpdateEvent(EventObject obj)
        {
            ResultObject _returnObj = new ResultObject();
            EncryptHelperObj _objEncryptHelperObj = new EncryptHelperObj();
            try
            {
                #region CheckRequiredFields

                if (string.IsNullOrEmpty(obj.EventName) || string.IsNullOrEmpty(obj.EventTime) || string.IsNullOrEmpty(obj.Address1))
                {
                    _returnObj.Status = 0;
                    _returnObj.Message = "Please check all required fields";
                }
                else
                {
                    _returnObj = _objUserRepository.CreateUpdateEvent(obj);
                    return _returnObj;
                }
                #endregion CheckRequiredFields
            }
            catch (Exception ex)
            {
                _returnObj.Message = ex.Message.ToString();
                _returnObj.Status = 0;
            }
            return _returnObj;
        }


        public ResultObject DeleteEvent(int EventId)
        {
            ResultObject _returnObj = new ResultObject();
            try
            {
                _returnObj = _objUserRepository.DeleteEvent(EventId);
                return _returnObj;
            }
            catch (Exception ex)
            {
                _returnObj.Message = ex.Message.ToString();
                _returnObj.Status = 0;
            }
            return _returnObj;
        }

        public UserListReturn fetchEvents(int EventId)
        {
            UserListReturn _returnObj = new UserListReturn();
            List<EventObject> _obj = new List<EventObject>();
            try
            {
                _obj = _objUserRepository.fetchEvents(EventId);
                if (_obj != null && _obj.Count > 0)
                {
                    _returnObj.Status = 1;
                    _returnObj.Message = "User(s) returned successfully!";
                    _returnObj.details = _obj;
                }
                else
                {
                    _returnObj.Status = 0;
                    _returnObj.Message = "Sorry! No User found.";
                    _returnObj.details = null;
                }
            }
            catch
            {
            }
            return _returnObj;
        }
    }
}
