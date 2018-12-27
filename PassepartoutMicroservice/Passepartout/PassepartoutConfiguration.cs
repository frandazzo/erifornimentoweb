using System;
using System.Collections.Generic;
using System.Text;

namespace PassepartoutMicroservice.Passepartout
{
    public class PassepartoutConfiguration
    {
        public PassepartoutConfiguration()
        {
            Azienda = PassepartoutMicroservice.Properties.Settings.Default.Azienda;
            Porta = PassepartoutMicroservice.Properties.Settings.Default.Porta;
            Indirizzo = PassepartoutMicroservice.Properties.Settings.Default.Indirizzo;
            Password = PassepartoutMicroservice.Properties.Settings.Default.Password;
            LoginMexal = PassepartoutMicroservice.Properties.Settings.Default.LoginMexal;
            PasswordMexal = PassepartoutMicroservice.Properties.Settings.Default.PasswordMexal;
            Terminale = PassepartoutMicroservice.Properties.Settings.Default.Terminale;
            MastrinoCliente = PassepartoutMicroservice.Properties.Settings.Default.MastrinoCliente;

            ListinoCliente = PassepartoutMicroservice.Properties.Settings.Default.ListinoCliente;
            ValutaCliente = PassepartoutMicroservice.Properties.Settings.Default.ValutaCliente;

        }


        public string IntegrationDatabase { get; set; }
        public string Porta { get; set; }
        public string Indirizzo { get; set; }
        public string Password { get; set; }
        public string LoginMexal { get; set; }
        public string PasswordMexal { get; set; }
        public string Terminale { get; set; }
        public string Azienda { get; set; }
        public string MastrinoCliente { get; set; }
        public int ListinoCliente { get; set; }
        public int ValutaCliente { get; set; }
    }
}
