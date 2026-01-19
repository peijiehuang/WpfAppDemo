using System.Collections.Generic;
using System.IO;
using WpfAppDemo.Models;

namespace WpfAppDemo.Services
{
    public interface ITESTService
    {
        IEnumerable<TEST> GetTESTs(string? keyword = null);
        void AddTEST(TEST entity);
        void UpdateTEST(TEST entity);
        void DeleteTEST(int id);
        MemoryStream ExportTESTs(string? keyword = null);
    }
}