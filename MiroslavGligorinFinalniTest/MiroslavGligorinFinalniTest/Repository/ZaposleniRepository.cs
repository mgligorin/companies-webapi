using MiroslavGligorinFinalniTest.Interfaces;
using MiroslavGligorinFinalniTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MiroslavGligorinFinalniTest.Repository
{
    public class ZaposleniRepository : IDisposable, IZaposleniRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Add(Zaposleni zaposleni)
        {
            db.Zaposleni.Add(zaposleni);
            db.SaveChanges();
        }

        public void Delete(Zaposleni zaposleni)
        {
            db.Zaposleni.Remove(zaposleni);
            db.SaveChanges();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Zaposleni> GetAll()
        {
            return db.Zaposleni.Include(k => k.Kompanija);
        }

        public Zaposleni GetById(int id)
        {
            return db.Zaposleni.Include(k => k.Kompanija).FirstOrDefault(k => k.Id == id);
        }

        public void Update(Zaposleni zaposleni)
        {
            db.Entry(zaposleni).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}