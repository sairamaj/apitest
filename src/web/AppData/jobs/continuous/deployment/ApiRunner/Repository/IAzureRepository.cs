using System.Threading.Tasks;

namespace ApiRunner.Repository
{
    interface IAzureRepository
    {
        Task Add(ApiInfoEntity api);
        Task Add(RunEntity run);
    }
}