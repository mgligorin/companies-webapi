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
    public class KompanijaRepository : IDisposable, IKompanijaRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        public IEnumerable<Kompanija> GetAll()
        {
            return db.Kompanija;
        }

        public Kompanija GetById(int id)
        {
            return db.Kompanija.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<KompanijaDTO> GetStatistics()
        {
            var kompanije = (from t in db.Kompanija
                          join z in db.Zaposleni on t.Id equals z.KompanijaId
                          group t by new { t, z.Plata } into g
                          select new KompanijaDTO
                          {
                              Id = g.Key.t.Id,
                              Naziv = g.Key.t.Naziv,
                              GodinaOsnivanja = g.Key.t.GodinaOsnivanja,
                              Average = g.Average(x => g.Key.Plata)
                          }).ToList().OrderByDescending(x => x.Average);
            return kompanije;
        }
    }
}