﻿using System;
using System.Linq;

namespace WebApplication3.Db
{
    public class IntegrationsQueries
    {
        public static string RicercaClienteTarga(string clienteId, string targa)
        {
            return @"SELECT [puddy_id_tag] as Id
                          ,[cky_cnt] as ClienteId
                          ,[targa] as Targa
                          ,[utente] as Utente
                    FROM a040_puddytag where cky_cnt = '" + clienteId + "' and  targa = '"+ targa +"'";
        }

        public static string UpdateClienteTarga(string clienteId, string targa)
        {
            string id = Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 15);
            return String.Format( @"INSERT INTO a040_puddytag
                           ([puddy_id_tag]
                           ,[stato]
                           ,[cky_cnt]
                           ,[targa]
                           ,[data_caricamento]
                           ,[utente]
                           ,[data_ult_modifica])
                     VALUES
                           ('{0}'
                           ,1
                           ,'{1}'
                           ,'{2}'
                           ,getDate()
                           ,'eRiforn'
                           ,getDate())", id,clienteId, targa);
        }


        public static string RicercaClientiTargheView()
        {
            string query = @"SELECT Top 50
                                rudt.[cky_cnt] CodiceCliente,
                                rudt.CDS_CNT_RAGSOC RagioneSociale,
                                rudt.IST_PIVA PartitaIva,
                                rudt.CSG_CFIS CodiceFiscale,
                                rudt.CKY_PAESE Nazione,
                                rudt.CDS_PROV Provincia,
                                rudt.CDS_LOC Comune,
                                rudt.CDS_INDIR Indirizzo,
                                rudt.CDS_CAP Cap,
                                rudt.ist_naz Cee,
                                CDS_CODSDI CodiceSdi,
                                cds_pec Pec,
                                [targa] Targa
                                FROM rudt join pico on pico.CKY_CNT=rudt.CKY_CNT and pico.IST_CNT='C'
                                left join [a040_puddytag] puddy_targhe on rudt.CKY_CNT=puddy_targhe.cky_cnt
                                where left(IST_PIVA,1)='N' and len(ist_piva)>1 
                                and (rudt.CDS_CNT_RAGSOC like '%' + @token + '%' or [targa] like '%' + @token + '%' or rudt.IST_PIVA like '%' + @token + '%')
                                order by Targa desc";


            return query;
        }




        public static string RicercaClienteById()
        {
            string query = @"SELECT  
                            rudt.[cky_cnt] CodiceCliente,
                            rudt.CDS_CNT_RAGSOC RagioneSociale,
                            rudt.IST_PIVA PartitaIva,
                            rudt.CSG_CFIS CodiceFiscale,
                            rudt.CKY_PAESE Nazione,
rudt.CDS_PROV Provincia,
                            rudt.CDS_LOC Comune,
                            rudt.CDS_INDIR Indirizzo,
                            rudt.CDS_CAP Cap,
                            rudt.ist_naz Cee,
                            CDS_CODSDI CodiceSdi,
                            cds_pec Pec
                            FROM rudt join pico on pico.CKY_CNT=rudt.CKY_CNT and pico.IST_CNT='C'
                            where rudt.[cky_cnt] = @id";

            return query;
        }
        public static string RicercaClienteByPartitaIva()
        {
            string query = @"SELECT  
                            rudt.[cky_cnt] CodiceCliente,
                            rudt.CDS_CNT_RAGSOC RagioneSociale,
                            rudt.IST_PIVA PartitaIva,
                            rudt.CSG_CFIS CodiceFiscale,
                            rudt.CKY_PAESE Nazione,
rudt.CDS_PROV Provincia,
                            rudt.CDS_LOC Comune,
                            rudt.CDS_INDIR Indirizzo,
                            rudt.CDS_CAP Cap,
                            rudt.ist_naz Cee,
                            CDS_CODSDI CodiceSdi,
                            cds_pec Pec
                            FROM rudt join pico on pico.CKY_CNT=rudt.CKY_CNT and pico.IST_CNT='C'
                            where rudt.IST_PIVA like '%' + @partita + '%'";

            return query;
        }


        public static string RicercaArticoloById()
        {
            string query = @"select 
                                a.cky_art CodiceArticolo,
                                a.CDS_ART + CDS_AGGIUN_ART Descrizione,
                                a.NGB_IVA Iva,
                                l.NPZ_LIS Costo 
                                from arti a inner join ARTI_LISTINI l on a.CKY_ART = l.CKY_ART
                                where a.cky_art= @id and l.id_cr=2";

            return query;
        }
    }
}
