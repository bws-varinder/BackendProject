using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Dapper;
using System.Configuration;
using System.Collections;

namespace SqlDapper
{
    public class BaseDapper : IDatabasecontext
    {
        private readonly IDbConnection dbConnection;//<add name="con" connectionString="Data Source=CHANDAN-PC; Initial Catalog=SubsourceSuperAdmin;Integrated Security=True"/>
        private static string connectionString = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
        //private static string connectionString = @"Server=DELL\VARINDER2012;Database=SubsourceNew;Integrated Security=True;";
        private static string sqlProvider = "System.Data.SqlClient";

        private BaseDapper(IDbConnection dbConnection)
        {

            this.dbConnection = dbConnection;
        }

        public static IDatabasecontext CreateInstance()
        {
            try
            {
                var providerFactory = DbProviderFactories.GetFactory(sqlProvider);
                var connection = providerFactory.CreateConnection();
                if (connection == null)
                {
                    Exception ex = new InvalidOperationException(string.Format("Provider failed to create connection {0}", sqlProvider));
                    throw ex;
                }
                connection.ConnectionString = connectionString;
                return new BaseDapper(connection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            CloseConnection();
            dbConnection.Dispose();
        }

        public void OpenConnection()
        {
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return ReplaceFunction<int>(() => dbConnection.Execute(sql, param, transaction, null, commandType));
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return ReplaceFunction<IEnumerable<T>>(() => dbConnection.Query<T>(sql, param, transaction, true, null, commandType));
        }
        public IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return dbConnection.Query(sql, param, transaction, true, null, commandType);
        }

        //public dynamic QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        //{
        //    return ReplaceFunction<dynamic>(() => dbConnection.QueryMultiple(sql, param, transaction, null, commandType));
        //}
        //public dynamic QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        //{
        //    return ReplaceFunction<dynamic>(() => dbConnection.QueryMultipleAsync(sql, param, transaction, null, commandType));
        //}

        public Dapper.SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(this.dbConnection, sql, param, transaction, 0, commandType);
        }

        private TResult ReplaceFunction<TResult>(Func<TResult> func)
        {
            try
            {
                OpenConnection();
                return func();
            }
            finally
            {
                CloseConnection();
            }

        }

    }
}
