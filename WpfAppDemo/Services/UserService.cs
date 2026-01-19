using System.Collections.Generic;
using System.IO;
using WpfAppDemo.Models;
using WpfAppDemo.Common;
using SqlSugar;
using MiniExcelLibs;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 用户管理服务实现类
    /// </summary>
    public class UserService : ServiceBase, IUserService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置接口</param>
        public UserService(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// 初始化表结构
        /// </summary>
        public override void EnsureDatabaseCreated()
        {
            base.EnsureDatabaseCreated();
            Logger.Information("初始化用户表结构...");
            Db.CodeFirst.InitTables(typeof(User));
        }

        /// <summary>
        /// 分页获取用户
        /// </summary>
        public IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount)
        {
            return Db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Username.Contains(keyword!) || it.Name.Contains(keyword!) || it.Email.Contains(keyword!))
                .ToPageList(pageIndex, pageSize, ref totalCount);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public void AddUser(User user)
        {
            Logger.Information("添加用户: {Username}", user.Username);
            Db.Insertable(user).ExecuteCommand();
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        public void UpdateUser(User user)
        {
            Logger.Information("更新用户 ID: {Id}", user.Id);
            Db.Updateable(user).ExecuteCommand();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public void DeleteUser(int id)
        {
            Logger.Information("删除用户 ID: {Id}", id);
            Db.Deleteable<User>().In(id).ExecuteCommand();
        }

        /// <summary>
        /// 导出数据到 Excel
        /// </summary>
        public MemoryStream ExportUsers(string? keyword = null)
        {
            Logger.Information("导出用户数据...");
            var list = Db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Username.Contains(keyword!) || it.Name.Contains(keyword!) || it.Email.Contains(keyword!))
                .ToList();
            var stream = new MemoryStream();
            stream.SaveAs(list);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}