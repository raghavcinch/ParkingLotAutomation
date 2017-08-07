using ParkingLot.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ParkingLot.SQL
{



    public class ParkingLotContext : IParkingLotContext
    {
        private ParkingLotEntities _enitityContainer;
        private static object _lock = new object();


        private void SetContextInfo(ParkingLotEntities enityConttainer)
        {
            var connection = enityConttainer.Database.Connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();


        }
      
        public ParkingLotEntities EntityContainer
        {
            get
            {
                if (this._enitityContainer == null)
                {
                    lock (_lock)
                    {
                        if (this._enitityContainer == null)
                        {
                            var container = new ParkingLotEntities();
                            SetContextInfo(container);
                            SetEntityContainer(container);

                        }
                    }
                }


                return this._enitityContainer;
            }
        }

        protected void SetEntityContainer(ParkingLotEntities container)
        {

            this._enitityContainer = container;
        }


        public int SaveChanges()
        {

            //hackich since order of saving changes probably matter in practice
            //hopefully we move to unified backend db before this is really needed.
            int ret = 0;
            bool success = false;

            //certain M5 Entities map directly to C9 Entities. We want to mirror the M5 changes in C9

            try
            {


                if (this._enitityContainer != null)
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                    {
                        ret += this._enitityContainer.SaveChanges();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            finally
            {

            }
            return ret;
        }

        public void Dispose()
        {

            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._enitityContainer != null)
                {
                    this._enitityContainer.Dispose();
                }
            }
        }
    }
}
