using System.Collections.Generic;
using System.IO;
using WpfAppDemo.Models;

namespace WpfAppDemo.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers(int pageIndex, int pageSize, string? keyword, ref int totalCount);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        void EnsureDatabaseCreated();
        MemoryStream ExportUsers(string? keyword = null);
    }
}