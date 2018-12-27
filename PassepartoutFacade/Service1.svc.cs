using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PassepartoutFacade
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice, nel file svc e nel file di configurazione contemporaneamente.
    // NOTA: per avviare il client di prova WCF per testare il servizio, selezionare Service1.svc o Service1.svc.cs in Esplora soluzioni e avviare il debug.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private MSprixDn.MxSpxDotNet mexalCnn;
        private int value = 0;

        public string GetData(int value)
        {
            if (mexalCnn == null)
            {
                mexalCnn = new MSprixDn.MxSpxDotNet(this);
                mexalCnn.SETUTILIZZATORE(this);
                DateTime date = DateTime.Now;
                string data = date.ToString("ddMMyyyy");
                mexalCnn.PORTA = 9000;
                mexalCnn.INDIRIZZO = "192.168.1.50";
                mexalCnn.PASSWORDPASS = "FATTEL:FATTEL"; //USERNAME : PASSWORD (MEXAL)
                mexalCnn.TERMINALE = "T0";
                mexalCnn.DATAAPTERM = data;
                mexalCnn.AZIENDA = "IMP";

                mexalCnn.AVVIACONNESSIONE();
            }
           

            this.value = this.value + value;
            return string.Format("You entered: {0}", this.value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
