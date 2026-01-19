namespace WpfAppDemo.Services
{
    /// <summary>
    /// 认证服务接口，处理用户登录及初始化权限
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录是否成功</returns>
        bool Login(string username, string password);

        /// <summary>
        /// 种子数据：如果不存在则创建默认管理员账号
        /// </summary>
        void SeedDefaultAdmin();
    }
}
