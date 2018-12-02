using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace cg.Api.Infrastructure
{
    public interface IIncludable
    {
       

    }

    public interface IIncludable<TEntity>:IIncludable
    {
        IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath);
        IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPropertyPath);
        IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, ICollection<TProperty>>> navigationPropertyPath);

    }

    public interface IIncludable<TEntity,TProperty> :IIncludable<TEntity>
    {
        IIncludable<TEntity, TProperty> ThenInclude<TPreviousProperty>(Expression<Func<TProperty, TPreviousProperty>> navigationPropertyPath);
    }
}
