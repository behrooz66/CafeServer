using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Repositories
{
    public class CustomerRepository: ICustomerRepository
    {
        private ApplicationDbContext db;
        public CustomerRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public bool Archive(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null) throw new NullReferenceException();
            customer.Deleted = true;
            db.SaveChanges();
            return true;
        }

        public int Create(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.Id;
        }

        public bool Delete(int id)
        {
            var cus = db.Customers.Find(id);
            if (cus == null) throw new NullReferenceException();
            db.Customers.Remove(cus);
            db.SaveChanges();
            return true;
        }

        public Customer Get(int id)
        {
            var cus = db.Customers.Find(id);
            db.Entry(cus).Reference("City").Load();
            db.Entry(cus.City).Reference("Province").Load();
            db.Entry(cus.City.Province).Reference("Country").Load();
            if (cus == null) throw new NullReferenceException();
            return cus;
        }

        public IEnumerable<Customer> GetByRestaurant(int restaurantId)
        {
            var cus = db.Customers.Where(x => x.RestaurantId == restaurantId);
            var customers = cus.ToList();
            return customers;
        }

        public Customer Update(int id, Customer updated)
        {
            var cus = db.Customers.Find(id);
            if (cus == null) throw new NullReferenceException();
            cus.Address = updated.Address;
            cus.Cell = updated.Cell;
            cus.CityId = updated.CityId;
            //cus.Deleted = updated.Deleted;
            cus.Email = updated.Email;
            cus.Home = updated.Home;
            cus.Lat = updated.Lat;
            cus.Lon = updated.Lon;
            cus.Name = updated.Name;
            cus.NoAddress = updated.NoAddress;
            cus.Notes = updated.Notes;
            cus.Work = updated.Work;
            cus.OtherPhone = updated.OtherPhone;
            cus.PostalCode = updated.PostalCode;
            cus.UpdatedAt = updated.UpdatedAt;
            cus.UpdatedBy = updated.UpdatedBy;
            cus.AddressFound = updated.AddressFound;
            //cus.ProvinceId = updated.ProvinceId;
            //cus.RestaurantId = updated.RestaurantId;
            db.SaveChanges();
            return cus;
        }
    }
}
