using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using WpfAppDemo.Common;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 代码生成服务接口
    /// </summary>
    public interface ICodeGenService
    {
        /// <summary>
        /// 获取数据库所有表名
        /// </summary>
        List<string> GetTableNames();

        /// <summary>
        /// 获取指定表的列信息
        /// </summary>
        /// <param name="tableName">表名</param>
        List<DbColumnInfo> GetColumnInfos(string tableName);
    }

    /// <summary>
    /// 代码生成服务实现类
    /// </summary>
    public class CodeGenService : ServiceBase, ICodeGenService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置接口</param>
        public CodeGenService(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// 获取表名列表
        /// </summary>
        public List<string> GetTableNames()
        {
            Logger.Information("正在从 sqlite_master 获取实时表结构...");
            // 针对 SQLite，直接查询系统表是最可靠的，完全绕过任何可能的缓存
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' AND name NOT LIKE 'sqlitestat%' ORDER BY name";
            return Db.Ado.SqlQuery<string>(sql);
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        public List<DbColumnInfo> GetColumnInfos(string tableName)
        {
            Logger.Information("获取表 {TableName} 的列信息...", tableName);
            return Db.DbMaintenance.GetColumnInfosByTableName(tableName);
        }
    }
}