using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WpfAppDemo.Models;
using SqlSugar;

namespace WpfAppDemo.Services
{
    public class TESTService : ITESTService
    {
        private readonly SqlSugarClient _db;

        public TESTService(IConfiguration configuration)
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

        public IEnumerable<TEST> GetTESTs(string keyword = null)
        {
            return _db.Queryable<TEST>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Name.Contains(keyword))
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
    }
}
