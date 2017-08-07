using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLot.Data;
using ParkingLot.SQL;
using Ninject.Modules;
using System.Configuration;
using System.Reflection;
using ParkingLot.SQL.Repositories;
using Ninject.Parameters;

namespace ParkingLot.IOC
{
    public class TypesContainer : NinjectModule
    {
        public override void Load()
        {
            IKernel container = new StandardKernel();
            var repoMode = "Test";
            try
            {
                repoMode = ConfigurationManager.AppSettings["RepositoryMode"];
            }
            catch (Exception)
            {
            }
            if (string.IsNullOrEmpty(repoMode))
                repoMode = "Test";

            //SQL Binding
            this.Bind(typeof(IParkingLotRepository<Slot>)).To(typeof(SlotsRepository)).When(a => repoMode == "SQL");
            this.Bind(typeof(IParkingLotRepository<User>)).To(typeof(UsersRepository)).When(a => repoMode == "SQL");
            this.Bind(typeof(IParkingLotRepository<Vehicle>)).To(typeof(VehicleRepository)).When(a => repoMode == "SQL");
            this.Bind(typeof(IParkingLotRepository<VehicleSlot>)).To(typeof(VehicleSlotsRepository)).When(a => repoMode == "SQL");
            this.Bind(typeof(IParkingLotContext)).To(typeof(ParkingLotContext)).When(a => repoMode == "SQL");
        }

        public static IParkingLotRepository<T> GetRepository<T>() where T : class
        {
            IKernel _Kernal = new StandardKernel();
            _Kernal.Load(Assembly.GetExecutingAssembly());
            var obj = _Kernal.Get<IParkingLotRepository<T>>();
            return obj;
        }
        public static IParkingLotContext GetContext()
        {
            IKernel _Kernal = new StandardKernel();
            _Kernal.Load(Assembly.GetExecutingAssembly());
            var obj = _Kernal.Get<IParkingLotContext>();
            return obj;
        }
    }
}
