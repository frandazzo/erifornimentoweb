using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassepartoutFacade.Domain
{
    public class Articolo
    {
        public string CodiceArticolo { get; set; }
        public string Descrizione { get; set; }
        public double Iva { get; set; }
        public double Costo { get; set; }

    }
}
