using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLot.Data;
using ParkingLot.IOC;

namespace ParkingLot.Core
{
    public class UserManager : IDisposable
    {
        private IParkingLotRepository<User> userRepo;
        private IParkingLotContext context;
        public UserManager()
        {
            context = TypesContainer.GetContext();
            userRepo = TypesContainer.GetRepository<User>();
            
        }
        
        public User Create(User user)
        {
            return userRepo.Add(user);
        }

        public User Update(User user)
        {
            var existing = userRepo.GetAll().SingleOrDefault(a => a.Id == user.Id);
            existing.Username = user.Username;
            existing.Password = user.Password;
            existing.Firstname = user.Firstname;
            existing.Lastname = user.Lastname;
            return existing;
        }

        public User MapVehicle(User user, Vehicle vehicle)
        {
            var existing = userRepo.GetAll().SingleOrDefault(a => a.Id == user.Id);
            existing.Vehicles.Add(vehicle);
            return existing;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        public IQueryable<User> Get(string userName)
        {
            return userRepo.GetAll().Where(a => a.Username.Contains(userName));
        }
        public User Get(int id)
        {
            return userRepo.GetAll().Where(a => a.Id == id).SingleOrDefault();
        }
        public IQueryable<User> GetAll()
        {
            return userRepo.GetAll();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
