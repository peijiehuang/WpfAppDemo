using WpfAppDemo.Models;
using WpfAppDemo.Common;
using Microsoft.Extensions.Configuration;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 认证服务实现类
    /// </summary>
    public class AuthService : ServiceBase, IAuthService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置接口</param>
        public AuthService(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// 执行登录逻辑
        /// </summary>
        public bool Login(string username, string password)
        {
            Logger.Information("尝试登录用户: {Username}", username);
            return Db.Queryable<User>().Any(u => u.Username == username && u.Password == password);
        }

        /// <summary>
        /// 初始化默认管理员
        /// </summary>
        public void SeedDefaultAdmin()
        {
            if (!Db.Queryable<User>().Any(u => u.Username == "admin"))
            {
                Logger.Information("初始化默认管理员账号...");
                Db.Insertable(new User 
                { 
                    Username = "admin", 
                    Password = "admin", 
                    Name = "管理员", 
                    Role = "Admin" 
                }).ExecuteCommand();
            }
        }
    }
}