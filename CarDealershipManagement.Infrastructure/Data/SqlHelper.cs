using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Infrastructure.Data
{
    public class SqlHelper
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private SqlTransaction _transaction;
        private SqlConnection _connection;

        public SqlHelper(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void SetTransaction(SqlTransaction transaction)
        {
            _transaction = transaction;
            _connection = transaction.Connection;
        }

        public void ClearTransaction()
        {
            _transaction = null;
            _connection = null;
        }

        private SqlCommand CreateCommand(SqlConnection connection, string procedureName, SqlParameter[] parameters = null)
        {
            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public async Task<T> ExecuteScalarAsync<T>(string procedureName, SqlParameter[] parameters = null)
        {
            bool ownsConnection = false;
            SqlConnection connection = _connection;

            try
            {
                if (connection == null)
                {
                    ownsConnection = true;
                    connection = _connectionFactory.CreateConnection();
                    await connection.OpenAsync();
                }

                using var command = CreateCommand(connection, procedureName, parameters);
                if (_transaction != null)
                    command.Transaction = _transaction;

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return default;

                return (T)Convert.ChangeType(result, typeof(T));
            }
            finally
            {
                if (ownsConnection && connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, SqlParameter[] parameters = null)
        {
            bool ownsConnection = false;
            SqlConnection connection = _connection;

            try
            {
                if (connection == null)
                {
                    ownsConnection = true;
                    connection = _connectionFactory.CreateConnection();
                    await connection.OpenAsync();
                }

                using var command = CreateCommand(connection, procedureName, parameters);
                if (_transaction != null)
                    command.Transaction = _transaction;

                return await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (ownsConnection && connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string procedureName, SqlParameter[] parameters = null)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = _connection ?? _connectionFactory.CreateConnection();

                if (_connection == null)
                    await connection.OpenAsync();

                command = CreateCommand(connection, procedureName, parameters);

                if (_transaction != null)
                    command.Transaction = _transaction;

                // CommandBehavior.CloseConnection ensures the connection is closed when the reader is closed
                // but only if we opened the connection (not in a transaction)
                var behavior = _connection == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
                return await command.ExecuteReaderAsync(behavior);
            }
            catch
            {
                command?.Dispose();

                if (_connection == null)
                    connection?.Dispose();

                throw;
            }
        }

        public async Task<DataSet> ExecuteDataSetAsync(string procedureName, SqlParameter[] parameters = null)
        {
            bool ownsConnection = false;
            SqlConnection connection = _connection;

            try
            {
                if (connection == null)
                {
                    ownsConnection = true;
                    connection = _connectionFactory.CreateConnection();
                    await connection.OpenAsync();
                }

                using var command = CreateCommand(connection, procedureName, parameters);
                if (_transaction != null)
                    command.Transaction = _transaction;

                using var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
            finally
            {
                if (ownsConnection && connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
