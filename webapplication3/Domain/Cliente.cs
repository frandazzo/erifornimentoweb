using System;
using System.Linq;

namespace WebApplication3.Domain
{
    public class Cliente
    {
        public string CodiceCliente { get; set; }
        public string RagioneSociale { get; set; }
        public string PartitaIva { get; set; }

        public string CodiceFiscale { get; set; }
        public string Nazione { get; set; }
        public string Provincia { get; set; }
        public string Comune { get; set; }
        public string Indirizzo { get; set; }


        public int Cap { get; set; }
        public string Cee { get; set; }
        public string CodiceSdi { get; set; }
        public string Pec { get; set; }
        public string  Targa { get; set; }
    }
}
