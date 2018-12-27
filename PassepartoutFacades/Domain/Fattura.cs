using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PassepartoutFacade.Domain
{
    public class Fattura
    {
        public Fattura(Cliente cliente, string targa)
        {
            this.cliente = cliente;
            this.cliente.Targa = targa;
        }

        readonly IList<FatturaItem> items = new List<FatturaItem>();
        public Cliente cliente { get; set; }


        public void AddItem(Articolo articolo, double totale)
        {

            if (articolo == null)
                return;

            if (totale == 0)
                return;

            items.Add(new FatturaItem(articolo, totale));
        }

        public double GetTotal()
        {
            return items.Sum(a => a.Totale);
        }

    }
}
