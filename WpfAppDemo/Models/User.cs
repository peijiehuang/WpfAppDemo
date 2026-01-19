using SqlSugar;
using System.ComponentModel;
using System;
using MiniExcelLibs.Attributes;

namespace WpfAppDemo.Models
{
    /// <summary>
    /// 用户实体模型
    /// </summary>
    [SugarTable("Users")]
    public class User : IDataErrorInfo
    {
        /// <summary>
        /// 主键 ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 角色（如 Admin, User）
        /// </summary>
        public string Role { get; set; } = "User";

        /// <summary>
        /// 数据验证错误信息（IDataErrorInfo 成员）
        /// </summary>
        [ExcelIgnore]
        [SugarColumn(IsIgnore = true)]
        public string Error => string.Empty;

        /// <summary>
        /// 索引器，用于属性级数据验证
        /// </summary>
        /// <param name="columnName">属性名</param>
        /// <returns>错误描述</returns>
        [ExcelIgnore]
        [SugarColumn(IsIgnore = true)]
        public string this[string columnName]
        {
            get
            {
                string? result = null;
                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrWhiteSpace(Username))
                            result = "用户名必填";
                        else if (Username.Length < 3)
                            result = "用户名至少需要 3 个字符";
                        break;
                    case nameof(Email):
                        if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
                            result = "电子邮件格式不正确";
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            result = "密码必填";
                        break;
                }
                return result ?? string.Empty;
            }
        }
    }
}