using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace NisrePgData
{
    public class DbRepo
    {
        private readonly string _dbConnectionString;
        public DbRepo(NpgsqlConnectionStringBuilder csb)
        {
            _dbConnectionString = csb.ToString();
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
                var exception = new NisreDbException(ex,parameter);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
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
                var exception = new NisreDbException(ex);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string procedure)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QueryAsync<T>(procedure, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = new NisreDbException(ex);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string procedure, PgParam parameter)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QueryAsync<T>(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = new NisreDbException(ex,parameter);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
            }
        }

        public async Task<T> QuerySingle<T>(string procedure, PgParam parameter)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QuerySingleOrDefaultAsync<T>(procedure, parameter,
                    commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = new NisreDbException(ex);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
            }
        }

        public async Task<T> QuerySingle<T>(string procedure)
        {
            try
            {
                using var db = new NpgsqlConnection(_dbConnectionString);
                await db.OpenAsync();
                return await db.QuerySingleOrDefaultAsync<T>(procedure, commandType: CommandType.StoredProcedure);
            }
            catch (NpgsqlException ex)
            {
                var exception = new NisreDbException(ex);
                throw exception;
            }
            catch (Exception e)
            {
                throw new NisreDbException(e);
            }
        }

        #endregion
    }
}