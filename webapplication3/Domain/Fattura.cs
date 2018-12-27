using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication3.Domain
{
    public class Fattura
    {
        public string Codice { get; set; }
        public int TipoPagamento { get; set; }
        public Fattura(Cliente cliente, string targa, string tipoDocumento, int tipoPagamento)
        {
            this.Cliente = cliente;
            this.Cliente.Targa = targa;
            this.TipoDocumento = tipoDocumento;
            this.TipoPagamento = tipoPagamento;
        }

        public readonly IList<FatturaItem> items = new List<FatturaItem>();
        public Cliente Cliente { get; set; }
        public string TipoDocumento { get; private set; }

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
