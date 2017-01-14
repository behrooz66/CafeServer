using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Repositories
{
    public class ProvinceRepository: IProvinceRepository
    {
        private ApplicationDbContext db;
        public ProvinceRepository(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public int Create(Province province)
        {
            db.Provinces.Add(province);
            db.SaveChanges();
            return province.Id;
        }

        public bool Delete(int id)
        {
            var p = db.Provinces.Where(pr => pr.Id == id).FirstOrDefault();
            if (p == null) throw new NullReferenceException();
            db.Provinces.Remove(p);
            db.SaveChanges();
            return true;
        }

        public Province Get(int id)
        {
            var p = db.Provinces.Where(pr => pr.Id == id).FirstOrDefault();
            if (p == null) throw new NullReferenceException();
            return p;
        }

        public IEnumerable<Province> GetAll()
        {
            var p = db.Provinces.ToList();
            return p;
        }

        public Province Update(int id, Province updatedProvince)
        {
            var p = db.Provinces.Where(pr => pr.Id == id).FirstOrDefault();
            p.Name = updatedProvince.Name;
            db.SaveChanges();
            return p;
        }
    }
}
