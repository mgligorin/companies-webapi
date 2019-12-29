using MiroslavGligorinFinalniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroslavGligorinFinalniTest.Interfaces
{
    public interface IZaposleniRepository
    {
        void Add(Zaposleni zaposleni);
        void Delete(Zaposleni zaposleni);
        IEnumerable<Zaposleni> GetAll();
        Zaposleni GetById(int id);
        void Update(Zaposleni zaposleni);
    }
}
