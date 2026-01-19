using SqlSugar;
using System;
using MiniExcelLibs.Attributes;

namespace WpfAppDemo.Models
{
    [SugarTable("TEST")]
    public class TEST
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
