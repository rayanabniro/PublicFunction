using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PublicFunction.DataBase
{
    public class EFCore
    {
        /// <summary>
        /// Generic repository interface for performing CRUD operations.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        public interface IGenericRepositoryService<T> where T : class
        {
            /// <summary>
            /// Inserts a new entity into the repository.
            /// </summary>
            /// <param name="entity">The entity to insert.</param>
            void Insert(T entity);

            /// <summary>
            /// Updates an existing entity in the repository.
            /// </summary>
            /// <param name="entity">The entity with updated values.</param>
            void Update(T entity);

            /// <summary>
            /// Deletes an entity from the repository by its identifier.
            /// </summary>
            /// <param name="id">The identifier of the entity to delete.</param>
            void Delete(object id);

            /// <summary>
            /// Retrieves an entity by its identifier.
            /// </summary>
            /// <param name="id">The identifier of the entity.</param>
            /// <returns>The entity if found; otherwise, null.</returns>
            T GetById(object id);

            /// <summary>
            /// Retrieves all entities from the repository.
            /// </summary>
            /// <returns>An enumerable of all entities.</returns>
            IEnumerable<T> GetAll();

            /// <summary>
            /// Finds entities based on a specified predicate.
            /// </summary>
            /// <param name="predicate">The condition to filter entities.</param>
            /// <returns>An enumerable of matching entities.</returns>
            IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        }
        /// <summary>
        /// Generic repository implementation for performing CRUD operations.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        public class GenericRepository<T> : IGenericRepositoryService<T> where T : class
        {
            private readonly DbContext _context;
            private readonly DbSet<T> _dbSet;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
            /// </summary>
            /// <param name="context">The database context.</param>
            public GenericRepository(DbContext context)
            {
                _context = context;
                _dbSet = _context.Set<T>();
            }

            /// <inheritdoc />
            public void Insert(T entity)
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }

            /// <inheritdoc />
            public void Update(T entity)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            /// <inheritdoc />
            public void Delete(object id)
            {
                T entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                }
            }

            /// <inheritdoc />
            public T GetById(object id)
            {
                return _dbSet.Find(id);
            }

            /// <inheritdoc />
            public IEnumerable<T> GetAll()
            {
                return _dbSet.ToList();
            }

            /// <inheritdoc />
            public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
            {
                return _dbSet.Where(predicate).ToList();
            }
        }
    }
}
