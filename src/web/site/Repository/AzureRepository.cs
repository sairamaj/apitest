using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using site.Models;

namespace site.Repository
{
    class AzureRepository : IAzureRepository
    {
        private const string APIINFO_TABLENAME = "ApiRuns";
        private string _connectionString;
        public AzureRepository(IConfiguration configuration)
        {
            this._connectionString = configuration.GetSection("ConnectionStrings")["AzureWebJobsStorage"];
        }

        public async IAsyncEnumerable<RunEntity> GetRuns(string environment)
        {
            var table = await GetApiRunTable();
            TableContinuationToken token = null;
            var entities = new List<RunEntity>();
            do
            {
                
                var queryResult = await table.ExecuteQuerySegmentedAsync(
                    new TableQuery<RunEntity>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey",QueryComparisons.Equal, environment)), token);
                foreach (var result in queryResult.Results)
                {
                    yield return result;
                }

                token = queryResult.ContinuationToken;
            } while (token != null);
        }

        public async IAsyncEnumerable<ApiInfoEntity> GetApis(string runId)
        {
            var table = await GetApiRunTable();
            TableContinuationToken token = null;
            var entities = new List<RunEntity>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(
                    new TableQuery<ApiInfoEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal, runId)), token);
                foreach (var result in queryResult.Results)
                {
                    yield return result;
                }

                token = queryResult.ContinuationToken;
            } while (token != null);
        }


        public async IAsyncEnumerable<ApiInfoEntity> GetApiDetails(string apiId)
        {
            var table = await GetApiRunTable();
            TableContinuationToken token = null;
            var entities = new List<RunEntity>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(
                    new TableQuery<ApiInfoEntity>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey",QueryComparisons.Equal, apiId)), token);
                foreach (var result in queryResult.Results)
                {
                    yield return result;
                }

                token = queryResult.ContinuationToken;
            } while (token != null);
        }

        private CloudStorageAccount StorageAccount => CloudStorageAccount.Parse(this._connectionString);
        private async Task<CloudTable> GetApiRunTable()
        {
            // Create a table client for interacting with the table service
            var tableClient = this.StorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(APIINFO_TABLENAME);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}