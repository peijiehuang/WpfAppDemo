using WpfAppDemo.Models;
using SqlSugar;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace WpfAppDemo.Services
{
    public interface IAuthService
    {
        bool Login(string username, string password);
        void SeedDefaultAdmin();
    }

    public class AuthService : IAuthService
    {
        private readonly SqlSugarClient _db;

        public AuthService(Microsoft.Extensions.Configuration.IConfiguration configuration)
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

        public bool Login(string username, string password)
        {
            return _db.Queryable<User>().Any(u => u.Username == username && u.Password == password);
        }

        public void SeedDefaultAdmin()
        {
            if (!_db.Queryable<User>().Any(u => u.Username == "admin"))
            {
                _db.Insertable(new User 
                { 
                    Username = "admin", 
                    Password = "admin", 
                    Name = "Administrator", 
                    Role = "Admin" 
                }).ExecuteCommand();
            }
        }
    }
}
