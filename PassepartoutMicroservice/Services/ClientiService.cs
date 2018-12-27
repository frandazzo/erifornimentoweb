
using PassepartoutMicroservice.DB;
using PassepartoutMicroservice.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace PassepartoutMicroservice.Services
{
    public class ClientiService
    {
        readonly PassepartoutDbContext context;
        
      
        public ClientiService(PassepartoutDbContext context)
        {
            this.context = context;
        }


        public Cliente GetClienteByPartitaIvaOrCreateIt(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.PartitaIva))
                throw new ArgumentException("Partita iva cliente");

            Task<List<Cliente>> task = context.Database.SqlQuery<Cliente>(IntegrationsQueries.RicercaClienteByPartitaIva(), new SqlParameter("@partita", cliente.PartitaIva)).ToListAsync();
            task.Wait();

            Cliente c = task.Result.FirstOrDefault();

            if (c == null)
                return Passepartout.PassepartoutFacade.Instance.CreateCliente(cliente);


            return c;
        }


    }
}
