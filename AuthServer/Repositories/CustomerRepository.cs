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

            // log history
            LogHistory(customer);

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

            //Clear history
            ClearHistory(cus);

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

        public IEnumerable<Customer> GetByRestaurant(int restaurantId, bool includeDeleted=false)
        {
            var cus = db.Customers.Where(x => x.RestaurantId == restaurantId);
            if (!includeDeleted)
                cus = cus.Where(c => !c.Deleted);
            var customers = cus.ToList();
            return customers;
        }

        public Customer Update(int id, Customer updated)
        {
            var cus = db.Customers.Find(id);
            if (cus == null) throw new NullReferenceException();

            // log history
            LogHistory(cus);


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


        //*** DEALING WITH HISTORY
        public IEnumerable<CustomerHistory> GetHistory(int id)
        {
            var history = db.CustomerHistories.Where(ch => ch.CustomerId == id).ToList();
            return history;
        }

        private void LogHistory(Customer c)
        {
            var h = new CustomerHistory()
            {
                Address = c.Address,
                AddressFound = c.AddressFound,
                Cell = c.Cell,
                City = db.Cities.Single(ci => ci.Id == c.CityId).Name,
                CustomerId = c.Id,
                Deleted = c.Deleted,
                Email = c.Email,
                Home = c.Home,
                Lat = c.Lat,
                Lon = c.Lon,
                Name = c.Name,
                NoAddress = c.NoAddress,
                Notes = c.Notes,
                OtherPhone = c.OtherPhone,
                PostalCode = c.PostalCode,
                Restaurant = db.Restaurants.Single(r => r.Id == c.RestaurantId).Name,
                UpdatedAt = c.UpdatedAt,
                UpdatedBy = c.UpdatedBy,
                Work = c.Work
            };
            db.CustomerHistories.Add(h);
            db.SaveChanges();
        }

        private void ClearHistory(Customer c)
        {
            var h = db.CustomerHistories.Where(ch => ch.CustomerId == c.Id).ToList();
            db.CustomerHistories.RemoveRange(h);
            db.SaveChanges();
        }

        
        // SUMMARIES 

        public dynamic OrderSummary(int id)
        {
            var orders = db.Orders.Where(o => o.CustomerId == id)
                                  .Where(o => !o.Deleted)
                                  .OrderByDescending(o => o.Date);
            var count = orders.Count() > 0 ? orders.Count() : 0;
            var total = count > 0 ? orders.Sum(o => o.Price) : 0;
            var lastOrderDate = count > 0 ? orders.First().Date.ToString() : "N/A";
            dynamic result = new
            {
                count = count,
                total = total,
                lastDate = lastOrderDate
            };
            return result;
        }

        public dynamic GiftCardSummary(int id)
        {
            var gifts = db.GiftCards.Where(o => o.CustomerId == id)
                                  .Where(o => !o.Deleted)
                                  .OrderByDescending(o => o.IssueDate);
            var count = gifts.Count() > 0 ? gifts.Count() : 0;
            var total = count > 0 ? gifts.Sum(o => o.Amount) : 0;
            var lastGiftDate = count > 0 ? gifts.First().IssueDate.ToString() : "N/A";
            dynamic result = new
            {
                count = count,
                total = total,
                lastDate = lastGiftDate
            };
            return result;
        }

        public dynamic ReservationSummary(int id)
        {
            var reservations = db.Reservations.Where(o => o.CustomerId == id)
                                  .Where(o => !o.Deleted)
                                  .OrderByDescending(o => o.Date);
            var count = reservations.Count() > 0 ? reservations.Count() : 0;
            var total = count > 0 ? reservations.Sum(o => o.Revenue) : 0;
            var lastReservationDate = count > 0 ? reservations.First().Date.ToString() : "N/A";
            dynamic result = new
            {
                count = count,
                total = total,
                lastDate = lastReservationDate
            };
            return result;
        }

    }
}
