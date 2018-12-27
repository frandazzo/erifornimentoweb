using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PassepartoutFacade.Domain
{
    public class FatturaItem
    {
        public FatturaItem(Articolo articolo, double totale)
        {
            
            if (Articolo == null)
                throw new ArgumentException("Articolo nullo per item fattura");
            this.Articolo = articolo;
            this.Totale = totale;
            //calcolo il totale senza iva
            Imponibile = Math.Round(Convert.ToDouble((100 * Totale) / (100 + articolo.Iva)), 2);
            ImportoIva = Totale - Imponibile;

            //poichè il prezzo unitario sull'articolo è già ivato la quantità la calcolo come totale/prezzo unitario
            Quantita = Math.Round(Convert.ToDouble(Totale / articolo.Costo), 2);
        }
        public Articolo Articolo
        { get; set; }

        public double ImportoIva { get; private set; }
        public double Quantita { get; private set; }
        public double Totale { get; private set; }
        public double Imponibile { get; private set; }



    }
}
