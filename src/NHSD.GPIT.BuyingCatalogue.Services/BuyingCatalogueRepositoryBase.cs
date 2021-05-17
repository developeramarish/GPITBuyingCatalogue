﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;

namespace NHSD.GPIT.BuyingCatalogue.Services
{
    [ExcludeFromCodeCoverage]
    public abstract class BuyingCatalogueRepositoryBase<T> : IBuyingCatalogueRepository<T>
        where T : class
    {
        private readonly BuyingCatalogueDbContext dbContext;

        protected BuyingCatalogueRepositoryBase(BuyingCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public abstract void Add(T item);

        public abstract void AddAll(IList<T> items);

        public abstract Task<T[]> GetAllAsync(Expression<Func<T, bool>> predicate);

        public abstract Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        public abstract void Remove(T item);

        public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
    }
}
