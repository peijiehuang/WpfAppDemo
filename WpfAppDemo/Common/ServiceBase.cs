using Serilog;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using WpfAppDemo.Models;

namespace WpfAppDemo.Common
{
    /// <summary>
    /// 业务服务基类，提供日志记录和数据库访问的基础支持
    /// </summary>
    public abstract class ServiceBase
    {
        protected readonly ILogger Logger;
        protected readonly SqlSugarClient Db;

        /// <summary>
        /// 构造函数，初始化日志和数据库连接
        /// </summary>
        /// <param name="configuration">配置接口</param>
        protected ServiceBase(IConfiguration configuration)
        {
            Logger = Log.ForContext(this.GetType());

            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? "DataSource=app.db";

            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        /// <summary>
        /// 确保数据库和表已创建（通常在程序启动时调用）
        /// </summary>
        public virtual void EnsureDatabaseCreated()
        {
            Db.DbMaintenance.CreateDatabase();
            // 子类可以重写此方法以初始化特定表
        }
    }
}
