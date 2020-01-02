using MiroslavGligorinFinalniTest.Interfaces;
using MiroslavGligorinFinalniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiroslavGligorinFinalniTest.Controllers
{
    public class KompanijeController : ApiController
    {
        IKompanijaRepository _repository { get; set; }

        public KompanijeController(IKompanijaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Kompanija> Get()
        {
            return _repository.GetAll();
        }
        
        public IHttpActionResult Get(int id)
        {
            var kompanija = _repository.GetById(id);
            if (kompanija == null)
            {
                return NotFound();
            }
            return Ok(kompanija);
        }
        
        [HttpGet]
        [Route("api/tradicija")]
        public IEnumerable<Kompanija> GetTradition()
        {
            var kompanije = new List<Kompanija>();
            kompanije.Add(_repository.GetAll().OrderBy(x => x.GodinaOsnivanja).FirstOrDefault());
            kompanije.Add(_repository.GetAll().OrderBy(x => x.GodinaOsnivanja).LastOrDefault());
            return kompanije;
        }

        [Authorize]
        [HttpGet]
        [Route("api/statistika")]
        public IEnumerable<KompanijaDTO> GetStatistics()
        {
            var kompanije = _repository.GetStatistics();
            return kompanije;
        }
    }
}
