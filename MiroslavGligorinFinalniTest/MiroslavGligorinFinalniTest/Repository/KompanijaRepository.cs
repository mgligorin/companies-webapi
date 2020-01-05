using MiroslavGligorinFinalniTest.Interfaces;
using MiroslavGligorinFinalniTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
            var kompanije = (from kompanija in db.Kompanija
                             join zaposleni in db.Zaposleni on kompanija.Id equals zaposleni.KompanijaId
                             group new { kompanija, zaposleni } by new { kompanija.Id, kompanija.Naziv, kompanija.GodinaOsnivanja } into g
                             select new KompanijaDTO
                             {
                                 Id = g.Key.Id,
                                 Naziv = g.Key.Naziv,
                                 GodinaOsnivanja = g.Key.GodinaOsnivanja,
                                 Average = g.Average(x => x.zaposleni.Plata)
                             }).ToList().OrderByDescending(x => x.Average);

            return kompanije;
        }
    }
}