using System;
using System.Linq;

namespace WebApplication3.Models.Fatturazione
{
    public class ClienteDto
    {
        private AnagraficaDto anag = new AnagraficaDto();
        private SdtDto sdi = new SdtDto();
        private long? dtGenQr;
        private string targa;

        
        public AnagraficaDto Anag
        {
            get
            {
                return anag;
            }
            set
            {
                anag = value;
            }
        }
        public SdtDto Sdi
        {
            get
            {
                return sdi;
            }

            set
            {
                sdi = value;
            }
        }
        public long? DtGenQr
        {
            get
            {
                return dtGenQr;
            }

            set
            {
                dtGenQr = value;
            }
        }
        public string Targa
        {
            get
            {
                return targa;
            }

            set
            {
                targa = value;
            }
        }
    }
}
