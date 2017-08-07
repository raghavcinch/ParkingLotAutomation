using System;
using System.Data.Entity;
using System.Linq;
using ParkingLot.Data;

namespace ParkingLot.SQL
{
    public abstract class EntityRepository<T, TKey> : IParkingLotRepository<T> where T : class
    {
        private IParkingLotContext _context { get; set; }

        public IParkingLotContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        protected virtual DbSet<T> Items
        {
            get { return _context.EntityContainer.Set<T>(); }
        }

        public EntityRepository(IParkingLotContext context)
        {
            if (context == null)
            {
                throw new NullReferenceException("ITransactionContext not set");
            }
            _context = context as IParkingLotContext;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Items;
        }

        public virtual T Add(T item)
        {
            return Items.Add(item);
        }

        public virtual T Delete(T item)
        {
            return Items.Remove(item);
        }

        public virtual T Create()
        {
            return Items.Create<T>();
        }
    }

    public abstract class EntityRepository<T> : EntityRepository<T, int> where T : class
    {
        public EntityRepository(IParkingLotContext context)
            : base(context)
        {
        }
    }

}
