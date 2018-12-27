using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Db;
using WebApplication3.Domain;

namespace erifornimento.Services
{
    public class ArticoliService
    {
        readonly IntegrationContext integrationContext;
        readonly ApplicationContext context;
        public ArticoliService(ApplicationContext context, IntegrationContext integrationContext)
        {
            this.context = context;
            this.integrationContext = integrationContext;
        }

        public Articolo GetBenzina()
        {
            Configurazione configurazione = context.Configurations.First();
            string id = configurazione.Benzina;

            return FindArticolo(id);
        }

        public Articolo GetDiesel()
        {
            Configurazione configurazione = context.Configurations.First();
            string id = configurazione.Diesel;

            return FindArticolo(id);
        }

        public Articolo GetGpl()
        {
            Configurazione configurazione = context.Configurations.First();
            string id = configurazione.Gpl;

            return FindArticolo(id);
        }


        public Articolo GetMetano()
        {
            Configurazione configurazione = context.Configurations.First();
            string id = configurazione.Metano;

            return FindArticolo(id);
        }

        private Articolo FindArticolo(string id)
        {
            if (String.IsNullOrEmpty(id))
                return null;


            Task<IList<Articolo>> d1 = integrationContext.Database.SqlQuery<Articolo>(IntegrationsQueries.RicercaArticoloById(), new SqlParameter("@id", id)).ToListAsync();
            d1.Wait();

            return d1.Result.FirstOrDefault();
        }
    }
}
