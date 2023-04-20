using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Frameworks
{
    public interface IFrameworkService
    {
        public Task<Framework> GetFramework(string frameworkId);

        Task<IList<Framework>> GetFrameworks();

        Task AddFramework(string name, bool isLocalFundingOnly);

        Task EditFramework(string frameworkId, string name, bool isLocalFundingOnly);

        Task MarkAsExpired(string frameworkId);
    }
}
