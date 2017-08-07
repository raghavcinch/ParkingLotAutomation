using ParkingLot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.SQL.Repositories
{
    public class VehicleSlotsRepository : EntityRepository<VehicleSlot>
    {
        public VehicleSlotsRepository(IParkingLotContext context) : base(context)
        {
        }
        public override VehicleSlot Add(VehicleSlot item)
        {
            return Context.EntityContainer.VehicleSlots.Add(item);
        }
    }
}
