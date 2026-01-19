using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace WpfAppDemo.Services
{
    public interface ICodeGenService
    {
        List<string> GetTableNames();
        List<DbColumnInfo> GetColumnInfos(string tableName);
    }

    public class CodeGenService : ICodeGenService
    {
        private readonly SqlSugarClient _db;

        public CodeGenService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? "DataSource=app.db";

            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public List<string> GetTableNames()
        {
            return _db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
        }

        public List<DbColumnInfo> GetColumnInfos(string tableName)
        {
            return _db.DbMaintenance.GetColumnInfosByTableName(tableName);
        }
    }
}
