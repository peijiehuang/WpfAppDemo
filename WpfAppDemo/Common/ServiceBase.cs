using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Serilog;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using WpfAppDemo.Services;

namespace WpfAppDemo.Common
{
    /// <summary>
    /// 业务服务基础抽象类，提供日志和数据库访问
    /// </summary>
    public abstract class ServiceBase
    {
        protected readonly ILogger Logger;
        protected readonly SqlSugarClient Db;

        protected ServiceBase(IConfiguration configuration)
        {
            Logger = Log.ForContext(this.GetType());

            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? "DataSource=app.db";

            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public virtual void EnsureDatabaseCreated()
        {
            Db.DbMaintenance.CreateDatabase();
        }
    }

    /// <summary>
    /// 泛型业务服务基类，实现通用的 CRUD 逻辑
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class ServiceBase<T> : ServiceBase, IBaseService<T> where T : class, new()
    {
        protected ServiceBase(IConfiguration configuration) : base(configuration)
        {
        }

        public override void EnsureDatabaseCreated()
        {
            base.EnsureDatabaseCreated();
            Db.CodeFirst.InitTables(typeof(T));
        }

        public virtual T GetById(int id) => Db.Queryable<T>().InSingle(id);

        public virtual List<T> GetList() => Db.Queryable<T>().ToList();

        public virtual List<T> GetPageList(int pageIndex, int pageSize, ref int totalCount, Expression<Func<T, bool>>? whereExpression = null)
        {
            return Db.Queryable<T>()
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageList(pageIndex, pageSize, ref totalCount);
        }

        public virtual bool Insert(T entity)
        {
            Logger.Information("插入实体: {Entity}", typeof(T).Name);
            return Db.Insertable(entity).ExecuteCommand() > 0;
        }

        public virtual bool Update(T entity)
        {
            Logger.Information("更新实体: {Entity}", typeof(T).Name);
            return Db.Updateable(entity).ExecuteCommand() > 0;
        }

        public virtual bool Delete(int id)
        {
            Logger.Information("删除实体: {Entity}, ID: {Id}", typeof(T).Name, id);
            return Db.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }
    }
}