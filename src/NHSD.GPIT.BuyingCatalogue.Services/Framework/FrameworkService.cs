using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Frameworks;

namespace NHSD.GPIT.BuyingCatalogue.Services.Framework
{
    public class FrameworkService : IFrameworkService
    {
        private readonly BuyingCatalogueDbContext dbContext;

        public FrameworkService(BuyingCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<EntityFramework.Catalogue.Models.Framework> GetFramework(string frameworkId) =>
            await dbContext.Frameworks.FirstOrDefaultAsync(f => f.Id == frameworkId);

        public async Task<IList<EntityFramework.Catalogue.Models.Framework>> GetFrameworks()
            => await dbContext.Frameworks.ToListAsync();

        public async Task AddFramework(string name, bool isLocalFundingOnly)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var framework =
                new EntityFramework.Catalogue.Models.Framework
                {
                    ShortName = name, LocalFundingOnly = isLocalFundingOnly,
                };

            dbContext.Frameworks.Add(framework);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditFramework(string frameworkId, string name, bool isLocalFundingOnly)
        {
            var framework = await GetFramework(frameworkId);
            if (framework is null)
                return;

            framework.ShortName = name;
            framework.LocalFundingOnly = isLocalFundingOnly;

            await dbContext.SaveChangesAsync();
        }

        public async Task MarkAsExpired(string frameworkId)
        {
            var framework = await GetFramework(frameworkId);
            if (framework is null)
                return;

            framework.IsExpired = true;

            await dbContext.SaveChangesAsync();
        }
    }
}
