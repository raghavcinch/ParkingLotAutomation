
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLot.Data;
using ParkingLot.IOC;
using Wintellect.PowerCollections;

namespace ParkingLot.Core
{

    public class SlotManager : IDisposable
    {
        private static object slotLock = new object();
        private IParkingLotRepository<Slot> repo;
        private IParkingLotRepository<VehicleSlot> vehicleSlotRepo;
        private IParkingLotContext context;
        public SlotManager()
        {
            //context = TypesContainer.GetContext();
            repo = TypesContainer.GetRepository<Slot>();
            vehicleSlotRepo = TypesContainer.GetRepository<VehicleSlot>();
            context = vehicleSlotRepo.Context = repo.Context;

        }

        public Slot Create(Slot Slot)
        {
            return repo.Add(Slot);
        }

        public Slot Update(Slot Slot)
        {
            var existing = repo.GetAll().SingleOrDefault(a => a.Id == Slot.Id);
            existing.UserId = Slot.UserId;
            existing.SlotTypeId = Slot.SlotTypeId;
            return existing;
        }

        public Slot FreeUpSlot(Slot slot)
        {
            var mappedSlots = vehicleSlotRepo.GetAll().Where(a => a.SlotId == slot.Id && a.EndTime == null);
            foreach (var item in mappedSlots)
            {
                item.EndTime = DateTime.Now;
            }
            return slot;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        public Slot Get(int id)
        {
            return repo.GetAll().Where(a => a.Id == id).SingleOrDefault();
        }
        public IQueryable<Slot> GetAll()
        {
            return repo.GetAll();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Slot Park(Vehicle vehicle)
        {

            try
            {
                var freeSlots = repo.GetAll().Where(a => a.VehicleSlots.Count(vs => vs.EndTime == null) == 0 && 
                                a.SlotTypeId >= vehicle.SlotTypeId && 
                                a.ReservationTypeId <= vehicle.User.ReservationTypeId && 
                                (a.UserId == null || a.UserId == vehicle.UserId)).ToList();

                var comparisionA = new Comparison<Slot>((slot1, slot2) =>
                {
                    int comparer = 0;
                    if (slot1.ReservationTypeId == vehicle.User.ReservationTypeId) comparer = 0;
                    if (slot1.ReservationTypeId <= slot2.ReservationTypeId) comparer = 1;
                    return comparer;
                });
                var comparisionB = new Comparison<Slot>((slot1, slot2) =>
                {
                    int comparer = 0;
                    if (slot1.UserId == vehicle.UserId) comparer = 0;
                    if (slot1.ReservationTypeId == vehicle.User.ReservationTypeId) comparer = 0;
                    if (slot1.ReservationTypeId <= slot2.ReservationTypeId) comparer = 1;
                    return comparer;
                });
                OrderedSet<Slot> slotsA = new OrderedSet<Slot>(freeSlots, comparisionA);
                OrderedSet<Slot> slots = new OrderedSet<Slot>(slotsA, comparisionB);
                var slotMatched = slots.FirstOrDefault();
                var isBooked = false;
                if (slotMatched == null)
                    return new Slot() { Id = -1 };
                lock (slotLock)
                {
                    //Dual check after lock
                    var isOccupied = vehicleSlotRepo.GetAll().Any(vs => vs.SlotId == slotMatched.Id && vs.EndTime == null);
                    if (!isOccupied)
                    {
                        var vs = vehicleSlotRepo.Create();
                        vs.SlotId = slotMatched.Id;
                        vs.VehicleId = vehicle.Id;
                        vs.StartTime = DateTime.Now;
                        vehicleSlotRepo.Add(vs);
                        context.SaveChanges();
                        isBooked = true;
                    }
                }
                if (!isBooked)
                    Park(vehicle);

                //Eager load 
                return slotMatched;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
