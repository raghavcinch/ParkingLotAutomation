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
    public class VehicleController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Vehicle> Get()
        {
            using (var manager = new VehicleManager())
            {
                return manager.GetAll().ToList();
            }
        }

        // GET api/<controller>/5
        public Vehicle Get(int id)
        {
            using (var manager = new VehicleManager())
            {
                return manager.Get(id);
            }
        }

        [HttpPost]
        public Slot Park(int id, [FromBody]string value)
        {
            Vehicle vehicle;
            using (var manager = new VehicleManager())
            {
                vehicle = manager.Get(id);
            }
            using (var manager = new SlotManager())
            {
                var allocatedSlot = manager.Park(vehicle);
               var json = JsonConvert.SerializeObject(allocatedSlot, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });
                return JsonConvert.DeserializeObject<Slot>(json);
            }
        }
        [HttpPost]
        public Slot UnPark(int id, [FromBody]string value)
        {
            
            using (var manager = new SlotManager())
            {
                var freedUpSlot = manager.FreeUpSlot(new Slot() { Id = id });
                manager.SaveChanges();
                var json = JsonConvert.SerializeObject(freedUpSlot, Formatting.Indented,
                                     new JsonSerializerSettings
                                     {
                                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                     });
                return JsonConvert.DeserializeObject<Slot>(json);
            }
        }

        // POST api/<controller>
        public Vehicle Post([FromBody]string value)
        {
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(value);
            using (var manager = new VehicleManager())
            {
                return manager.Create(vehicle);
            }
        }

        // PUT api/<controller>/5
        public Vehicle Put(int id, [FromBody]string value)
        {
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(value);
            using (var manager = new VehicleManager())
            {
                return manager.Update(vehicle);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}