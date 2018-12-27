using System;
using System.Linq;

namespace WebApplication3.Models.Fatturazione
{
    public class DomicilioDto
    {
        private String prov = "";
        private String cap = "";
        private String com = "";
        private String ind = "";
        private String naz = "";

        public string Prov
        {
            get
            {
                return prov;
            }

            set
            {
                prov = value;
            }
        }
        public string Cap
        {
            get
            {
                return cap;
            }

            set
            {
                cap = value;
            }
        }
        public string Com
        {
            get
            {
                return com;
            }

            set
            {
                com = value;
            }
        }
        public string Ind
        {
            get
            {
                return ind;
            }

            set
            {
                ind = value;
            }
        }
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

    }
}
