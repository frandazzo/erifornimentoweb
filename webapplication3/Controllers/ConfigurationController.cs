using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication3.Db;
using WebApplication3.Domain;
using WebApplication3.Facades;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {

        readonly ApplicationContext dbContext;



        public ConfigurationController(ApplicationContext dbContext )
        {
            this.dbContext = dbContext;
        }


        // GET: api/Configuration
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<ConfigurazioneDTO> Get()
        {
            Configurazione c = dbContext.Configurations.First();
            return new ConfigurazioneDTO()
            {
                Id = c.Id,
                Metano = c.Metano,
                Diesel = c.Diesel,
                Benzina = c.Benzina,
                Gpl = c.Gpl,
                Gestionale = c.Gestionale
            };
        }



        // PUT: api/Configuration/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put(int id, [FromBody] ConfigurazioneDTO value)
        {
            Configurazione c = dbContext.Configurations.First();

            c.Metano = value.Metano;
            c.Diesel = value.Diesel;
            c.Benzina = value.Benzina;
            c.Gpl = value.Gpl;
            c.Gestionale = value.Gestionale;

            dbContext.SaveChanges();

            return NoContent();
        }


    }
}
