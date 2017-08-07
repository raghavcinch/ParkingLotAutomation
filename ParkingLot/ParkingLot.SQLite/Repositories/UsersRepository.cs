using ParkingLot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.SQL.Repositories
{
    public class UsersRepository : EntityRepository<User>
    {
        public UsersRepository(IParkingLotContext context) : base(context)
        {
        }
    }
}
