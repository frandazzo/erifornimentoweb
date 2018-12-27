
using PassepartoutMicroservice.Domain;
using System;

namespace PassepartoutMicroservice.Passepartout
{
    public class PassepartoutFacade
    {
        private static PassepartoutFacade instance;
        public static PassepartoutFacade Instance
        {
            get
            {
                if (instance == null)
                    instance = new PassepartoutFacade();
                return instance;
            }
        }

        private MSprixDn.MxSpxDotNet mexalCnn;
        private readonly PassepartoutConfiguration configuration;

        private PassepartoutFacade()
        {
            this.configuration = new  PassepartoutConfiguration();

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



            //azzera le variabili all'interno della dll solo per i clienti
            //il parametro 1 significa solo per i clienti (il 4 per magazzino etc)
            mexalCnn.AZZVARSYS(1);
            //CODICE PER LA numerazione automatica
            mexalCnn.PCCOD_S = configuration.MastrinoCliente + ".AUTO";
            mexalCnn.PCDES_S = cliente.RagioneSociale;
            mexalCnn.PCNPI_S = cliente.PartitaIva;
            mexalCnn.PCCFI_S = cliente.CodiceFiscale;
            mexalCnn.PCNAZ_S = cliente.Cee;
            mexalCnn.PCPAE_S = cliente.Nazione;
            mexalCnn.PCLOC_S = cliente.Comune;
            mexalCnn.PCCAP_S = cliente.Cap.ToString();
            mexalCnn.PCPRO_S = cliente.Provincia;
            mexalCnn.PCIND_S = cliente.Indirizzo;
            mexalCnn.PCPEC_S = cliente.Pec;
            mexalCnn.PCCODSDI_S = cliente.CodiceSdi;


            //aggiungo un paio di variabili necessarie
            //la valuta di gestione del cliente
            mexalCnn.PCVAL = configuration.ValutaCliente;
            //il listino associato al cliente
            mexalCnn.PCLIS = configuration.ListinoCliente;
            //codice pagamneto
            mexalCnn.PCPAG = 1;
            mexalCnn.PCSERIELE = 2;
            mexalCnn.PCFATTELE_S = "S";

            mexalCnn.PUTPC();

            if (!String.IsNullOrEmpty(mexalCnn.ERRPC_S))
            {
                throw new Exception(string.Format("Errore nella creazione del cliente: {0}", mexalCnn.ERRPC_S));
            }


            cliente.CodiceCliente = mexalCnn.PCCOD_S;
            return cliente;
        }
        public Fattura CreateFattura(Fattura f)
        {
            VerificaConnessioneAttiva();
           

            DateTime date = DateTime.Now;
            // azzeramento variabili
            mexalCnn.AZZVARSYS(4);
            //sigla documento
            mexalCnn.MMSIG_S = Properties.Settings.Default.SiglaDocumento;
            //sezionale
            mexalCnn.MMSER = Properties.Settings.Default.SerieDocumento;
            //numero documento
            mexalCnn.MMNUM = 0;

            //note
            mexalCnn.MMNOT_S[1] = f.Cliente.Targa.ToUpper();
            //data documento
            mexalCnn.MMDAT_S = date.ToString("yyyyMMdd");
            //magazzino
            mexalCnn.MMMAG = Properties.Settings.Default.MagazzinoDocumento;
            //codice cliente
            mexalCnn.MMCLI_S = f.Cliente.CodiceCliente;
            //causale magazzino
            mexalCnn.MMCMO[1] = Properties.Settings.Default.CausaleDocumento;

            if (f.TipoPagamento == 0)
                mexalCnn.MMPAG = Properties.Settings.Default.PagamentoContanti;
            else
                mexalCnn.MMPAG = Properties.Settings.Default.PagamentoCarta;

            int i = 0;

            foreach (FatturaItem item in f.items)
            {
                i++;
                //tipo riga R=articolo, D=descrizione
                mexalCnn.MMTPR_S[i] = "R";
                //codice articolo
                mexalCnn.MMART_S[i] = item.Articolo.CodiceArticolo;
                //descrizione articolo
                mexalCnn.MMDES_S[i] = "";
                //quantita
                mexalCnn.MMQTA[i] = item.Quantita;
                //aliquota iva
                mexalCnn.MMALI_S[i] = item.Articolo.Iva.ToString();

                //accisa
                mexalCnn.MMVDR_S[i, 4] = mexalCnn.DIZ("ar1dd", 0, item.Articolo.CodiceArticolo).ToString();

                //prezzo
                if (!Properties.Settings.Default.SiglaDocumento.Equals("CO"))
                    mexalCnn.MMPRZ[i] = item.PrezzoUnitarioImponibile;
                else
                {
                    double prezzoArticolo = item.Articolo.Costo;
                    double totItem = item.Totale;
                    double quantita = item.Quantita;
                    double valoreItem = Math.Round(Convert.ToDouble(quantita * prezzoArticolo), 2);

                    if (totItem != valoreItem)
                    {
                        double calculated = Math.Round(Convert.ToDouble(totItem / quantita), 5);
                        mexalCnn.MMPRZ[i] = calculated;
                    }
                    else
                    {
                        mexalCnn.MMPRZ[i] = item.Articolo.Costo;
                    }
                }

                // sconto
                mexalCnn.MMSCO_S[i] = "";
                // unità di misura
                mexalCnn.MMTIP_S[i] = "1";
            }

            mexalCnn.TOTMM();
            double totmexal = mexalCnn.MMT_TIMPON + mexalCnn.MMT_TIMPOS;
            if (!totmexal.Equals(f.GetTotal()))
            {
                mexalCnn.MMACC = totmexal - f.GetTotal();
            }

            mexalCnn.PUTMM(0);



            //contiene un eventuale errore, vuoto se non ce ne sono
            
            if (!String.IsNullOrEmpty(mexalCnn.ERRMM_S))
            {
                throw new Exception(string.Format("Errore nella creazione del documento: {0}", mexalCnn.ERRMM_S));
            }

            f.Codice = mexalCnn.MMNUM.ToString();


           return f;
        }


    }
}
