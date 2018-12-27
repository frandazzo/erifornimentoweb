using AutoMapper;
using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Db
{
    public static  class DatabaseExtensions
    {
        public static CustomTypeSqlQuery<T> SqlQuery<T>(
            this DatabaseFacade database,
            string sqlQuery, params DbParameter[] values) where T : class
        {
            return new CustomTypeSqlQuery<T>()
            {
                DatabaseFacade = database,
                SQLQuery = sqlQuery,
                QueryParams = values
            };
        }
    }


    public class CustomTypeSqlQuery<T> where T: class
    {
        private IMapper _mapper;

        public DatabaseFacade DatabaseFacade { get; set; }
        public string SQLQuery { get; set; }
        public DbParameter[] QueryParams { get; set; }

        public CustomTypeSqlQuery()
        {
            _mapper = new MapperConfiguration(cfg => {
                cfg.AddDataReaderMapping();
                cfg.CreateMap<IDataRecord, T>();
            }).CreateMapper();
        }

        public async Task<IList<T>> ToListAsync()
        {
            IList<T> results = new List<T>();
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                   


                    command.CommandText = SQLQuery;
                    foreach (var item in QueryParams)
                    {
                        command.Parameters.Add(item);
                    }

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                        results = _mapper.Map<IDataReader, IEnumerable<T>>(reader)
                                         .ToList();
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return results;
        }


    }
}
