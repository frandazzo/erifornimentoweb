using PassepartoutMicroservice.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PassepartoutMicroservice.Controllers
{
    [RoutePrefix("api/passepartout/documenti")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocumentiController : ApiController
    {

        [HttpPost]
        [Route("")]
        public Fattura CreateFattura([FromBody] Fattura documento)
        {
            return Passepartout.PassepartoutFacade.Instance.CreateFattura(documento);

        }


    }
}
