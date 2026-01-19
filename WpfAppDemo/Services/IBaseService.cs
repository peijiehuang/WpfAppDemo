using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace WpfAppDemo.Services
{
    /// <summary>
    /// 泛型业务服务基接口，定义基础的 CRUD 操作
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IBaseService<T> where T : class, new()
    {
        /// <summary>
        /// 根据 ID 获取单条记录
        /// </summary>
        T GetById(int id);

        /// <summary>
        /// 获取所有记录
        /// </summary>
        List<T> GetList();

        /// <summary>
        /// 分页获取记录
        /// </summary>
        List<T> GetPageList(int pageIndex, int pageSize, ref int totalCount, Expression<Func<T, bool>>? whereExpression = null);

        /// <summary>
        /// 插入记录
        /// </summary>
        bool Insert(T entity);

        /// <summary>
        /// 更新记录
        /// </summary>
        bool Update(T entity);

        /// <summary>
        /// 根据 ID 删除记录
        /// </summary>
        bool Delete(int id);

        /// <summary>
        /// 确保数据库表已创建
        /// </summary>
        void EnsureDatabaseCreated();
    }
}
