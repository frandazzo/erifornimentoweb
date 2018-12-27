using System;
using System.Linq;

namespace WebApplication3.Models.Fatturazione
{
    public class AnagraficaDto
    {
        private String naz = "";
        private String cf = "";
        private String piva = "";
        private String denom = "";
        private DomicilioDto domFisc = new DomicilioDto();
        public string Naz
        {
            get
            {
                return naz;
            }

            set
            {
                naz = value;
            }
        }
        public string Cf
        {
            get
            {
                return cf;
            }

            set
            {
                cf = value;
            }
        }
        public string Piva
        {
            get
            {
                return piva;
            }

            set
            {
                piva = value;
            }
        }
        public string Denom
        {
            get
            {
                return denom;
            }

            set
            {
                denom = value;
            }
        }
        public DomicilioDto DomFisc
        {
            get
            {
                return domFisc;
            }

            set
            {
                domFisc = value;
            }
        }

    }
}
