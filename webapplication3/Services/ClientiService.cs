
using erifornimento.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication3.Db;
using WebApplication3.Domain;

namespace erifornimento.Services
{
    public class ClientiService
    {
        readonly ApplicationContext context;
        readonly IntegrationContext integrationContext;
        readonly IConfiguration cofiguration;

        public ClientiService(ApplicationContext context, IntegrationContext integrationContext, IConfiguration cofiguration)
        {
            this.cofiguration = cofiguration;

            this.integrationContext = integrationContext;
            this.context = context;
        }


        public Cliente GetClienteByPartitaIvaOrCreateIt(Cliente cliente)
        {
            string urlService = this.cofiguration["IntegrationPath"] + "api/passepartout/clienti/ifnotexist";

            ApiClient<Cliente> client = new ApiClient<Cliente>(urlService);
            Task<Cliente> resultTask = client.PostStreamAsync(cliente);
            resultTask.Wait();

            return resultTask.Result;
        }
        public void SendFattura(Fattura f)
        {
            try
            {
                string urlService = this.cofiguration["IntegrationPath"] + "api/passepartout/documenti";

                String ser = JsonConvert.SerializeObject(f);

                ApiClient<Fattura> client = new ApiClient<Fattura>(urlService);

                Task<Fattura> resultTask = client.PostStreamAsync(f);
                resultTask.Wait();

                Fattura f1 = resultTask.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


    }
}
