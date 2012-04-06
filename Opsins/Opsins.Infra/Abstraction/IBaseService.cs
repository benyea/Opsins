using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Opsins.Infra
{
    public interface IBaseService<TEntity>  where TEntity : BaseEntity
    {
        bool Exist(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
    }
}
