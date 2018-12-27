using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication3.Domain
{
    public class FatturaItem
    {
        public FatturaItem(Articolo articolo, double totale)
        {
            
            if (articolo == null)
                throw new ArgumentException("Articolo nullo per item fattura");
            this.Articolo = articolo;
            this.Totale = totale;
            //calcolo il totale senza iva
            Imponibile = Math.Round(Convert.ToDouble((100 * Totale) / (100 + articolo.Iva)), 5);
            PrezzoUnitarioImponibile = Math.Round(Convert.ToDouble((100 * articolo.Costo) / (100 + articolo.Iva)), 5);
            ImportoIva = Math.Round(Totale - Imponibile,2);

            //poichè il prezzo unitario sull'articolo è già ivato la quantità la calcolo come totale/prezzo unitario
            Quantita = Math.Round(Convert.ToDouble(Totale / articolo.Costo), 5);
        }
        public Articolo Articolo
        { get; set; }

        public double ImportoIva { get; private set; }
        public double Quantita { get; private set; }
        public double Totale { get; private set; }
        public double Imponibile { get; private set; }
        public double PrezzoUnitarioImponibile { get; set; }




    }
}
