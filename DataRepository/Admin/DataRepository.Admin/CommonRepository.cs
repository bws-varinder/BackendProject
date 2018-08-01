using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using BusinessObject.Admin;
using SqlDapper;
using System.Data;

namespace DataRepository.Common
{
    public class CommonRepository : DapperRepository
    {
        public CommonRepository(IDatabasecontext dbcontext)
              : base(dbcontext)
        { }

        IDbConnection db = CommonRepository.GetConnectionString();

        public static SqlConnection GetConnectionString()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        }

        public static DynamicParameters GetLogParameters()
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@Stat", 0, DbType.Int32, ParameterDirection.Output);
            parameter.Add("@Message", "", DbType.String, ParameterDirection.Output);
            return parameter;

        }



    }
}
