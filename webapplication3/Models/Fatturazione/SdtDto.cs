using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models.Fatturazione
{
    public class SdtDto
    {
        private String pec;
        private String cod;
        public string Pec
        {
            get
            {
                return pec;
            }

            set
            {
                pec = value;
            }
        }
        public string Cod
        {
            get
            {
                return cod;
            }

            set
            {
                cod = value;
            }
        }
    }
}
