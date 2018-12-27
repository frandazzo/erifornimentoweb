using PassepartoutMicroservice.Domain;
using PassepartoutMicroservice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PassepartoutMicroservice.Controllers
{
    [RoutePrefix("api/passepartout/clienti")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClientiController: ApiController
    {
        


        [HttpPost]
        [Route("ifnotexist")]
        public Cliente GetClienteByPartitaIvaOrCreateItIfnotExist([FromBody] Cliente cliente)
        {
            using(var context = new DB.PassepartoutDbContext())
            {
                ClientiService c = new ClientiService(context);
                return c.GetClienteByPartitaIvaOrCreateIt(cliente);
            }
            
        }

    }
}
