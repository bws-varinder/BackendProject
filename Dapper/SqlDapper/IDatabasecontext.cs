using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDapper
{
    public interface IDatabasecontext : IDisposable
    {
        void OpenConnection();

        void CloseConnection();

        int Execute(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);

        IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);
        IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);
        Dapper.SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);
    }
}
