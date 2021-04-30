using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;

namespace NHSD.GPIT.BuyingCatalogue.Services
{
    public abstract class UsersDbRepositoryBase<T> : IUsersDbRepository<T> where T : class
    {
        private readonly UsersDbContext _dbContext;

        protected UsersDbRepositoryBase(UsersDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public abstract void Add(T item);
        public abstract void AddAll(IList<T> items);
        public abstract Task<T[]> GetAllAsync(Expression<Func<T, bool>> predicate);
        public abstract Task<T> SingleAsync(Expression<Func<T, bool>> predicate);
        public abstract void Remove(T item);
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}