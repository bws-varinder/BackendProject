using BusinessObject.Admin;
using DataManager.Admin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RealEstate.Controllers
{
    public class UsersController : ApiController
    {
        UsersManager _objUserManager = new UsersManager();

        [HttpPost]
        [Route("CreateUpdateEvent/")]
        public IHttpActionResult CreateUpdateEvent(EventObject _obj)
        {
            var _return_view_model = _objUserManager.CreateUpdateEvent(_obj);
            var json = JToken.FromObject(_return_view_model);
            return Ok(json);
        }

        [HttpGet]
        [Route("fetchEvents/{EventId}/")]
        public IHttpActionResult fetchUsers(int EventId)
        {
            var _return_view_model = _objUserManager.fetchEvents(EventId);
            var json = JToken.FromObject(_return_view_model);
            return Ok(json);
        }

        [HttpGet]
        [Route("DeleteEvent/{EventId}/")]
        public IHttpActionResult DeleteUser(int EventId)
        {
            var _return_view_model = _objUserManager.DeleteEvent(EventId);
            var json = JToken.FromObject(_return_view_model);
            return Ok(json);
        }
    }
}
