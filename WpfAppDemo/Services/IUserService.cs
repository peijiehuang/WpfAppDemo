using System.Collections.Generic;
using System.IO;
using WpfAppDemo.Models;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 用户管理服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="totalCount">总记录数输出</param>
        /// <returns>用户集合</returns>
        IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount);

        /// <summary>
        /// 添加新用户
        /// </summary>
        void AddUser(User user);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        void UpdateUser(User user);

        /// <summary>
        /// 删除指定 ID 的用户
        /// </summary>
        void DeleteUser(int id);

        /// <summary>
        /// 确保数据库和用户表已创建
        /// </summary>
        void EnsureDatabaseCreated();

        /// <summary>
        /// 导出用户数据到 Excel 流
        /// </summary>
        /// <param name="keyword">过滤关键词</param>
        /// <returns>内存流</returns>
        MemoryStream ExportUsers(string? keyword = null);
    }
}
