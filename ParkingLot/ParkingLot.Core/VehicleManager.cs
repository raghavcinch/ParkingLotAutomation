
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLot.Data;
using ParkingLot.IOC;

namespace ParkingLot.Core
{
    public class VehicleManager : IDisposable
    {
        private IParkingLotRepository<Vehicle> repo;
        private IParkingLotContext context;
        public VehicleManager()
        {
            context = TypesContainer.GetContext();
            repo = TypesContainer.GetRepository<Vehicle>();
            
        }
        
        public Vehicle Create(Vehicle vehicle)
        {
            return repo.Add(vehicle);
        }

        public Vehicle Update(Vehicle vehicle)
        {
            var existing = repo.GetAll().SingleOrDefault(a => a.Id == vehicle.Id);
            existing.VehicleNumber = vehicle.VehicleNumber;
            existing.SlotTypeId = vehicle.SlotTypeId;
            return existing;
        }

        public Vehicle MapSlot(Slot slot, Vehicle vehicle)
        {
            var existing = repo.GetAll().SingleOrDefault(a => a.Id == vehicle.Id);
            existing.VehicleSlots.Add(new VehicleSlot()
            {
                VehicleId = vehicle.Id,
                SlotId = slot.Id
            });
            return existing;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        public Vehicle Get(int id)
        {
            return repo.GetAll().Where(a => a.Id == id).SingleOrDefault();
        }
        public IQueryable<Vehicle> GetAll()
        {
            return repo.GetAll();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
