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
    public class ZaposleniController : ApiController
    {
        IZaposleniRepository _repository { get; set; }

        public ZaposleniController(IZaposleniRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Zaposleni> Get()
        {
            return _repository.GetAll().OrderByDescending(x => x.Plata);
        }

        [Authorize]
        public IHttpActionResult Get(int id)
        {
            var zaposleni = _repository.GetById(id);
            if (zaposleni == null)
            {
                return NotFound();
            }
            return Ok(zaposleni);
        }

        [Authorize]
        public IHttpActionResult Post(Zaposleni zaposleni)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(zaposleni);
            return CreatedAtRoute("DefaultApi", new { id = zaposleni.Id }, zaposleni);
        }

        [Authorize]
        public IHttpActionResult Put(int id, Zaposleni zaposleni)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != zaposleni.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(zaposleni);
            }
            catch
            {
                throw;
            }

            return Ok(zaposleni);
        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            var zaposleni = _repository.GetById(id);
            if (zaposleni == null)
            {
                return NotFound();
            }

            _repository.Delete(zaposleni);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<Zaposleni> Search(int godiste)
        {
            return _repository.GetAll().Where(x => x.GodinaRodjenja > godiste).OrderByDescending(x => x.GodinaRodjenja);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Zaposlenje")]
        public IEnumerable<Zaposleni> SearchPost([FromBody] ZaposleniPretraga zaposleniPretraga)
        {
            return _repository.GetAll().Where(x => x.GodinaZaposlenja >= zaposleniPretraga.Pocetak && x.GodinaZaposlenja <= zaposleniPretraga.Kraj).OrderBy(x => x.GodinaZaposlenja);
        }
    }
}
