using ParkingLot.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ParkingLot.Data;

namespace ParkingLot.Test
{
    public class FakeParkingLotContext : IParkingLotContext

    {
        private ParkingLotEntities _entityContainer;

        public FakeParkingLotContext()
        {
            _entityContainer = new Mock<ParkingLotEntities>().Object;
        }
        public ParkingLotEntities EntityContainer { get { return _entityContainer; } }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
