using AutoMapper;
using erifornimento.Domain;
using erifornimento.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Db;
using WebApplication3.Domain;
using WebApplication3.Models.Fatturazione;

namespace WebApplication3.Controllers
{
    [Route("api/mobile")]
    [ApiController]

    public class MobileApiController : ControllerBase
    {

        readonly IntegrationContext integrationContext;

       
        readonly ApplicationContext appContext;
        readonly ArticoliService articoliService;
        readonly ClientiService clientiService;
        readonly IMapper mapper;

        public MobileApiController(ApplicationContext appContext,
            IntegrationContext integrationContext,
           
            ArticoliService articoliService,
            ClientiService clientiService, IMapper mapper)
        {
            this.mapper = mapper;
            this.clientiService = clientiService;
            this.articoliService = articoliService;
            this.appContext = appContext;
         

            this.integrationContext = integrationContext;
        }


        [HttpGet]
        [Route("ping")]
        public ActionResult<string> Ping()
        {



            return "ok";
        }

        [HttpGet]
        [Route("clienti/{token}")]
        [Authorize]
        public async Task<ActionResult<ClienteDto>> GetCliente([FromRoute] string token)
        {
            string query = IntegrationsQueries.RicercaClienteByPartitaIva();
            Cliente item = (await integrationContext.Database.SqlQuery<Cliente>(query, new SqlParameter("@partita", token)).ToListAsync()).SingleOrDefault();


            DateTime baseDate = new DateTime(1970, 1, 1);
            TimeSpan diff = DateTime.Now - baseDate;

            if (item != null)
            {

                var el = new ClienteDto()
                {
                    Anag = new AnagraficaDto()
                    {
                        Denom = item.RagioneSociale != null ? item.RagioneSociale.Trim() : string.Empty,
                        Piva = item.PartitaIva != null ? item.PartitaIva.Trim().Replace("NIT", string.Empty) : string.Empty,
                        Cf = item.CodiceFiscale != null ? item.CodiceFiscale.Trim() : string.Empty,
                        Naz = item.Cee != null ? item.Cee.Trim() : string.Empty,
                        DomFisc = new DomicilioDto()
                        {
                            Cap = item.Cap.ToString(),
                            Naz = item.Nazione != null ? item.Nazione.Trim() : string.Empty,
                            Com = item.Comune != null ? item.Comune.Trim() : string.Empty,
                            Ind = item.Indirizzo != null ? item.Indirizzo.Trim() : string.Empty,
                            Prov = item.Provincia != null ? item.Provincia.Trim() : string.Empty
                        }
                    },
                    Sdi = new SdtDto() { Cod = item.CodiceSdi != null ? item.CodiceSdi.Trim() : string.Empty, Pec = item.Pec != null ? item.Pec.Trim() : string.Empty },
                    DtGenQr = (long)diff.TotalMilliseconds,
                    Targa = string.Empty
                };
                return el;

            }

            return null;
            //string query = IntegrationsQueries.RicercaClienteById();
            //IList <Cliente> d = await integrationContext.Database.SqlQuery<Cliente>(query, new SqlParameter("@id", token)).ToListAsync();

            //string query = IntegrationsQueries.RicercaArticoloById();
            //IList<Articolo> d1 = await integrationContext.Database.SqlQuery<Articolo>(query, new SqlParameter("@id", token)).ToListAsync();

        }


        [HttpGet]
        [Route("clienti")]
        [Authorize]
        public async Task<ActionResult<List<ClienteDto>>> GetClienti([FromQuery] string targa)
        {
            string query = IntegrationsQueries.RicercaClientiTargheView();
            IList<TargaClienteView> d = await integrationContext.Database.SqlQuery<TargaClienteView>(query, new SqlParameter("@token", targa)).ToListAsync();

            List<ClienteDto> result = new List<ClienteDto>();
            DateTime baseDate = new DateTime(1970, 1, 1);
            TimeSpan diff = DateTime.Now - baseDate;


            foreach (var item in d)
            {
                var el = new ClienteDto()
                {
                    Anag = new AnagraficaDto()
                    {
                        Denom = item.RagioneSociale != null ? item.RagioneSociale.Trim() : string.Empty,
                        Piva = item.PartitaIva != null ? item.PartitaIva.Trim().Replace("NIT", string.Empty) : string.Empty,
                        Cf = item.CodiceFiscale != null ? item.CodiceFiscale.Trim() : string.Empty,
                        Naz = item.Cee != null ? item.Cee.Trim() : string.Empty,
                        DomFisc = new DomicilioDto()
                        {
                            Cap = item.Cap.ToString(),
                            Naz = item.Nazione != null ? item.Nazione.Trim() : string.Empty,
                            Com = item.Comune != null ? item.Comune.Trim() : string.Empty,
                            Ind = item.Indirizzo != null ? item.Indirizzo.Trim() : string.Empty,
                            Prov = item.Provincia != null ? item.Provincia.Trim() : string.Empty
                        }
                    },
                    Sdi = new SdtDto() { Cod = item.CodiceSdi != null ? item.CodiceSdi.Trim() : string.Empty, Pec = item.Pec != null ? item.Pec.Trim() : string.Empty },
                    DtGenQr = (long)diff.TotalMilliseconds,
                    Targa = item.Targa != null ? item.Targa.Trim() : string.Empty
                };
                result.Add(el);
            }

            return result;


        }

        [HttpPost]
        [Route("fatture")]
        [Authorize]
        public ActionResult<string> SaveFattura([FromBody] FatturaDto fattura)
        {


            //recupero la lista degli articooli
            try
            {
                Configurazione c = appContext.Configurations.First();

                //recupero tutti gli articoli 
                Articolo benzina = articoliService.GetBenzina();
                Articolo diesel = articoliService.GetDiesel();
                Articolo gpl = articoliService.GetGpl();
                Articolo metano = articoliService.GetMetano();

                //recupero il cliente
                Cliente cliente = clientiService.GetClienteByPartitaIvaOrCreateIt(mapper.Map<ClienteDto, Cliente>(fattura.Cliente));



                Fattura f = new Fattura(cliente, fattura.Targa, "FT", fattura.TipoPagamento);
                f.AddItem(benzina, fattura.Benzina);
                f.AddItem(gpl, fattura.Gpl);
                f.AddItem(metano, fattura.Metano);
                f.AddItem(diesel, fattura.Diesel);
                clientiService.SendFattura(f);
                RegistraTargaCliente(fattura, cliente);
                //this.passpartout.CreateFattura(f);

                //come ultima cosa da fare verificare che esista la coppia targa - cliente nella tabella puddytag....
                //se non esiste crearla
            }
            catch (Exception ex)
            {
                return ex.Message;
            }






            return "ok";
        }

        private void RegistraTargaCliente(FatturaDto fattura, Cliente cliente)
        {
            try
            {
                String query = IntegrationsQueries.RicercaClienteTarga(cliente.CodiceCliente, fattura.Targa);
                Task<IList<TargaCliente>> t = integrationContext.Database.SqlQuery<TargaCliente>(query).ToListAsync();
                t.Wait();
                TargaCliente t1 = t.Result.FirstOrDefault();
                if (t1 == null)
                {
                    String updateQuery = IntegrationsQueries.UpdateClienteTarga(cliente.CodiceCliente, fattura.Targa);
                    integrationContext.Database.ExecuteSqlCommand(updateQuery);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                
            }
            
        }
    }
}