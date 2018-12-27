using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PassepartoutMicroservice.Domain
{
    public class Fattura
    {

        public string Codice { get; set; }
        public IList<FatturaItem> items = new List<FatturaItem>();
        public Cliente Cliente { get; set; }
        public string TipoDocumento { get; set; }


        //contanti 0, pagamento con carta 1

        public int TipoPagamento { get; set; }


        public double GetTotal()
        {
            return items.Sum(a => a.Totale);
        }

    }
}
