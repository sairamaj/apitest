using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace ApiRunner.Repository
{
    class AzureRepository : IAzureRepository
    {
        private const string APIINFO_TABLENAME = "ApiRuns";
        public AzureRepository(string connectionString)
        {
            ConnectionString = connectionString ?? throw new System.ArgumentNullException(nameof(connectionString));
        }
        public string ConnectionString { get; }

        public async Task Add(ApiInfoEntity api)
        {
            await AddRunInfo(api);
        }

        public async Task Add(RunEntity run)
        {
            await AddRunInfo(run);
        }

        private async Task AddRunInfo(TableEntity entity)
        {
            // Retrieve storage account information from connection string.
            var storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
            // Create the InsertOrReplace table operation
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            // Execute the operation.
            var table = await GetApiRunTable(storageAccount);
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

        }
        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            return CloudStorageAccount.Parse(storageConnectionString);
        }

        private static async Task<CloudTable> GetApiRunTable(CloudStorageAccount storageAccount)
        {
            // Create a table client for interacting with the table service
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(APIINFO_TABLENAME);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}