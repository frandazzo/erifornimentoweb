using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PassepartoutMicroservice.Domain
{
    public class FatturaItem
    {
        public Articolo Articolo{ get; set; }
        public double ImportoIva { get;  set; }
        public double Quantita { get;  set; }
        public double Totale { get;  set; }
        public double Imponibile { get;  set; }
        public double PrezzoUnitarioImponibile { get; set; }
    }
}
