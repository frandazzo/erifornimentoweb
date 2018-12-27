using System;
using System.Linq;

namespace WebApplication3.Models
{
    public class ConfigurazioneDTO
    {
        public int Id { get; set; }

        public string Benzina { get; set; }
        public string Diesel { get; set; }
        public string Gpl { get; set; }
        public string Metano { get; set; }
        public string Gestionale { get; set; }

        
    }
}
