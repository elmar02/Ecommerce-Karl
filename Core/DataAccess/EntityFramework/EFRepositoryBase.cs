using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
    
namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            using var context = new TContext();

            foreach (var entity in entities)
            {
                var addEntity = context.Entry(entity);
                addEntity.State = EntityState.Added;
            }

            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            using var context = new TContext();
            var deleteEntity = context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            using var context = new TContext();

            foreach (var entity in entities)
            {
                var deleteEntity = context.Entry(entity);
                deleteEntity.State = EntityState.Deleted;
            }

            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            var context = new TContext();
            return context.Set<TEntity>().FirstOrDefault(expression);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null)
        {
            var context = new TContext();
            return expression == null ? context.Set<TEntity>() : context.Set<TEntity>().Where(expression);
        }

        public void Update(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            using var context = new TContext();

            foreach (var entity in entities)
            {
                var updateEntity = context.Entry(entity);
                updateEntity.State = EntityState.Modified;
            }

            context.SaveChanges();
        }
    }
}
