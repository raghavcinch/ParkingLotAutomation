using Newtonsoft.Json;
using ParkingLot.Core;
using ParkingLot.Data;
using ParkingLot.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Cors;

namespace ParkingLot
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            using (var userManager = new UserManager())
            {
                return userManager.GetAll().ToList();
            }
        }

        // GET api/<controller>/5
        public User Get(int id)
        {
            using (var userManager = new UserManager())
            {
                var user =  userManager.Get(id);
                foreach(var veh in user.Vehicles)
                {
                    veh.VehicleSlots = veh.VehicleSlots.Where(a => a.EndTime == null).ToList();
                }
                return user;
            }
        }

        [HttpGet]
        public IEnumerable<User> Search(string id)
        {
            using (var userManager = new UserManager())
            {
                var list = userManager.Get(id.ToString()).ToList();
                foreach(var user in list)
                {
                    foreach (var veh in user.Vehicles)
                    {
                        veh.VehicleSlots = veh.VehicleSlots.Where(a => a.EndTime == null).ToList();
                    }
                }
                return list;
            }
        }

        // POST api/<controller>
        public User Post([FromBody]string value)
        {
            var user = JsonConvert.DeserializeObject<User>(value);
            using (var userManager = new UserManager())
            {
                return userManager.Create(user);
            }
        }

        // PUT api/<controller>/5
        public User Put(int id, [FromBody]string value)
        {
            var user = JsonConvert.DeserializeObject<User>(value);
            using (var userManager = new UserManager())
            {
                return userManager.Update(user);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}