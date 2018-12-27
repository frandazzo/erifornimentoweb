using PassepartoutFacade.Domain;
using System;

namespace PassepartoutFacade
{
    public class PassepartoutFacade
    {

        private MSprixDn.MxSpxDotNet mexalCnn;

        readonly PassepartoutConfiguration configuration;
        public PassepartoutFacade(PassepartoutConfiguration configuration)
        {
            this.configuration = configuration;

            ConfiguraConnessione();

            mexalCnn.AVVIACONNESSIONE();
            if (mexalCnn.ERRORE != "")
            {
                throw new Exception(mexalCnn.ERRORE);
            }
        }

        private void ConfiguraConnessione()
        {
            mexalCnn = new MSprixDn.MxSpxDotNet(this);
            mexalCnn.SETUTILIZZATORE(this);
            DateTime date = DateTime.Now;
            string data = date.ToString("ddMMyyyy");
            mexalCnn.PORTA = Convert.ToDouble(configuration.Porta);
            mexalCnn.INDIRIZZO = configuration.Indirizzo;
            mexalCnn.PASSWORDPASS = configuration.Password;
            mexalCnn.TERMINALE = configuration.Terminale;
            mexalCnn.DATAAPTERM = data;
            mexalCnn.AZIENDA = configuration.Azienda;
        }

        public void VerificaConnessioneAttiva()
        {
            //
            if (mexalCnn.DIZ("sxter", 0, "") == "")
            {
                mexalCnn.CHIUDICONNESSIONE();
                ConfiguraConnessione();
                mexalCnn.AVVIACONNESSIONE();

            }

        }

        public Cliente CreateCliente(Cliente cliente)
        {

            VerificaConnessioneAttiva();

            cliente.CodiceCliente = "XXXXXXXX";
            return cliente;
        }
        public void CreateFattura(Fattura f)
        {
            VerificaConnessioneAttiva();

        }


    }
}
