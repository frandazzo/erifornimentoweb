using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models.Fatturazione
{
    public class FatturaDto
    {
        private String targa;
        private Double benzina;
        private Double diesel;
        private Double gpl;
        private Double metano;
        private ClienteDto cliente;
        private int tipoPagamento = 0;

        public int TipoPagamento
        {
            get
            {
                return tipoPagamento;
            }
            set
            {
                tipoPagamento = value;
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
        public double Benzina
        {
            get
            {
                return benzina;
            }

            set
            {
                benzina = value;
            }
        }
        public double Diesel
        {
            get
            {
                return diesel;
            }

            set
            {
                diesel = value;
            }
        }
        public double Gpl
        {
            get
            {
                return gpl;
            }

            set
            {
                gpl = value;
            }
        }
        public double Metano
        {
            get
            {
                return metano;
            }

            set
            {
                metano = value;
            }
        }
        public ClienteDto Cliente
        {
            get
            {
                return cliente;
            }

            set
            {
                cliente = value;
            }
        }

    }
}
