using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PassepartoutMicroservice.Controllers
{
    [RoutePrefix("api/passepartout")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        [Route("test")]
        [HttpGet]
        public string Test()
        {
            return "ok";
        }
    }
}
