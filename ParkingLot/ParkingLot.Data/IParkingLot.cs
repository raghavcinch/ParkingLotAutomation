using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Data
{
    public interface IParkingLotRepository<T> where T : class
    {
        T Create();
        IQueryable<T> GetAll();
        T Add(T item);
        T Delete(T item);
        IParkingLotContext Context { get; set; }
    }
    public interface IParkingLotContext : IDisposable
    {
        ParkingLotEntities EntityContainer { get; }
        int SaveChanges();

    }

}
