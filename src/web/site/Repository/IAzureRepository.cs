using System.Collections.Generic;
using System.Threading.Tasks;
using site.Models;

namespace site.Repository
{
    public interface IAzureRepository
    {
        IAsyncEnumerable<RunEntity> GetRuns(string environment);
        IAsyncEnumerable<ApiInfoEntity> GetApis(string runId);

        Task<ApiInfoDetailEntity> GetApiDetails(string environment, string apiId);
    }
}