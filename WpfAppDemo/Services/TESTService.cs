using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WpfAppDemo.Models;
using SqlSugar;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WpfAppDemo.Services
{
    public class TESTService : ITESTService
    {
        private readonly SqlSugarClient _db;

        public TESTService(IConfiguration configuration)
        {
            var connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection") 
                                 ?? "DataSource=app.db";

            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public IEnumerable<TEST> GetTESTs(string? keyword = null)
        {
            return _db.Queryable<TEST>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Name.Contains(keyword!))
                .ToList();
        }

        public void AddTEST(TEST entity)
        {
            _db.Insertable(entity).ExecuteCommand();
        }

        public void UpdateTEST(TEST entity)
        {
            _db.Updateable(entity).ExecuteCommand();
        }

        public void DeleteTEST(int id)
        {
            _db.Deleteable<TEST>().In(id).ExecuteCommand();
        }

        public MemoryStream ExportTESTs(string? keyword = null)
        {
            var list = GetTESTs(keyword);
            var stream = new MemoryStream();
            stream.SaveAs(list);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
