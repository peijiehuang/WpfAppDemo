using System.IO;
using WpfAppDemo.Models;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 用户管理服务接口，继承自泛型基接口
    /// </summary>
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 带有关键字过滤的分页查询（特殊业务）
        /// </summary>
        System.Collections.Generic.IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount);

        /// <summary>
        /// 导出用户数据
        /// </summary>
        MemoryStream ExportUsers(string? keyword = null);
    }
}