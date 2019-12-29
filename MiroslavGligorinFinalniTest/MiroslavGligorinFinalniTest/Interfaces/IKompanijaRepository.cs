using MiroslavGligorinFinalniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroslavGligorinFinalniTest.Interfaces
{
    public interface IKompanijaRepository
    {
        IEnumerable<Kompanija> GetAll();
        Kompanija GetById(int id);
        IEnumerable<KompanijaDTO> GetStatistics();
    }
}
