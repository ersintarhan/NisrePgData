using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using NisrePgData;
using Npgsql;
using NpgsqlTypes;

namespace CasinoPro.Integration.Database
{
    public class DbRepo
    {
        private readonly string _dbConnectionString;
        private readonly ILogger<DbRepo> _logger;

        public DbRepo(NpgsqlConnectionStringBuilder csb, ILogger<DbRepo> logger)
        {
            _logger = logger;
            _dbConnectionString = csb.ToString();
        }

        public Task EvolutionBookTransaction(long transactionId)
        {
            var p = new PgParam();
            p.Add("p_transaction_id",transactionId,NpgsqlDbType.Bigint);
            return this.Execute("evolution.book_transaction", p);
        }


        
        #region Private Methods

        public async Task Execute(string procedure, PgParam parameter)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                await db.ExecuteAsync(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger, parameter);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Execute Error");
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public async Task Execute(string procedure)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                await db.ExecuteAsync(procedure, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Execute Error");
                throw new Exception("System Error");
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string procedure)
        {
            try
            {
                await using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QueryAsync<T>(procedure, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Query Error");
                throw new Exception("System Error");
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string procedure, PgParam parameter)
        {
            try
            {
                await using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QueryAsync<T>(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger, parameter);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Query Error");
                throw new Exception("System Error");
            }
        }

        public async Task<T> QuerySingle<T>(string procedure, PgParam parameter)
        {
            try
            {
                await using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QuerySingleOrDefaultAsync<T>(procedure, parameter,
                    commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger, parameter);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Query Single Error");
                throw new Exception("System Error");
            }
        }

        public async Task<T> QuerySingle<T>(string procedure)
        {
            try
            {
                await using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QuerySingleOrDefaultAsync<T>(procedure, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = ex.ParseException(_logger);
                throw exception;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Query Single Error");
                throw new Exception("System Error");
            }
        }

        #endregion
    }
}