using System.Collections.Generic;
using System.IO;
using WpfAppDemo.Models;
using WpfAppDemo.Common;
using SqlSugar;
using Microsoft.Extensions.Configuration;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 用户管理服务实现类，继承自泛型基类 ServiceBase<User>
    /// </summary>
    public class UserService : ServiceBase<User>, IUserService
    {
        public UserService(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// 特殊的分页查询逻辑（包含多字段关键词过滤）
        /// </summary>
        public IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount)
        {
            return Db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(keyword), it => it.Username.Contains(keyword!) || it.Name.Contains(keyword!) || it.Email.Contains(keyword!))
                .ToPageList(pageIndex, pageSize, ref totalCount);
        }

        /// <summary>
        /// 特殊的导出逻辑
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
