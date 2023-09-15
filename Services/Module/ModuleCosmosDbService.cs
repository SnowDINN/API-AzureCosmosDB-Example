using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Anonymous.ADT;
using Anonymous.Models;
using Microsoft.Azure.Cosmos;

namespace Anonymous.IMPL
{
    public class ModuleCosmosDbService : IModuleCosmosDbService
    {
        private readonly Container _container;

        public ModuleCosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Module item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task AddItemsAsync(Module[] items)
        {
            foreach (var item in items) await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<Module>(id, new PartitionKey(id));
        }

        public async Task<Module> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Module>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Module>> GetItemsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Module>(new QueryDefinition(queryString));
            var results = new List<Module>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Module scenario)
        {
            await _container.UpsertItemAsync(scenario, new PartitionKey(id));
        }
    }
}