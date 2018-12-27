using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Facades
{
    public class ConfigurazioneProvider
    {
        private static ConfigurazioneProvider _instance;
        private ConfigurazioneProvider() { }

        private ConfigurazioneDTO _dto = new ConfigurazioneDTO()
        {
            Benzina = "",
            Gpl = "",
            Metano = "",
            Diesel = "",
            Gestionale = ""
        };

        public ConfigurazioneDTO Configurazione
        {
            get
            {
                return _dto;
            }
        }

        public static ConfigurazioneProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigurazioneProvider();
                return _instance;
            }
        }
    }
}
