using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WpfAppDemo.Models;
using SqlSugar;

namespace WpfAppDemo.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        void EnsureDatabaseCreated();
    }

    public class UserService : IUserService
    {
        private readonly SqlSugarClient _db;

        public UserService(Microsoft.Extensions.Configuration.IConfiguration configuration)
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

        public void EnsureDatabaseCreated()
        {
            _db.DbMaintenance.CreateDatabase();
            _db.CodeFirst.InitTables(typeof(User));
        }

        public IEnumerable<User> GetUsers()
        {
            return _db.Queryable<User>().ToList();
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
    }
}
