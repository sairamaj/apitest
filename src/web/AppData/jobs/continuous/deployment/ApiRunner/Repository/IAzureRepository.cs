using System.Threading.Tasks;

namespace ApiRunner.Repository
{
    interface IAzureRepository
    {
        Task AddApiInfo(ApiInfoEntity apiEntity);
    }
}