using System.Linq.Expressions;

namespace CSAS.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected readonly AppDbContext Context;

		public Repository(AppDbContext context) => Context = context;

		public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

		public void AddRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().AddRange(entities);

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => Context.Set<TEntity>().Where(predicate).ToList();

		public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

		public TEntity Get(string id) => string.IsNullOrEmpty(id)
				? throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id))
				: Context.Set<TEntity>().Find(id);

		public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

		public void RemoveRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().RemoveRange(entities);

		public void Update(TEntity entity) => Context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
	}
}
