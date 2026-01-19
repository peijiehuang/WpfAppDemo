using System.Collections.Generic;
using System.Linq;
using System.IO;
using WpfAppDemo.Models;
using SqlSugar;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WpfAppDemo.Services
{
    public class UserService : IUserService
    {
        private readonly SqlSugarClient _db;

        public UserService(IConfiguration configuration)
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

        public void EnsureDatabaseCreated()
        {
            _db.DbMaintenance.CreateDatabase();
            _db.CodeFirst.InitTables(typeof(User));
        }

        public IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount)
        {
            return _db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Username.Contains(keyword!) || it.Name.Contains(keyword!) || it.Email.Contains(keyword!))
                .ToPageList(pageIndex, pageSize, ref totalCount);
        }

        public void AddUser(User user)
        {
            _db.Insertable(user).ExecuteCommand();
        }

        public void UpdateUser(User user)
        {
            _db.Updateable(user).ExecuteCommand();
        }

        public void DeleteUser(int id)
        {
            _db.Deleteable<User>().In(id).ExecuteCommand();
        }

        public MemoryStream ExportUsers(string? keyword = null)
        {
            var list = _db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Username.Contains(keyword!) || it.Name.Contains(keyword!) || it.Email.Contains(keyword!))
                .ToList();
            var stream = new MemoryStream();
            stream.SaveAs(list);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
