namespace NorthEducationAPI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using NorthEducationAPI.Models;

    public class Module_05CosmosDbService : IModule_05CosmosDbService
    {
        private Container _container;

        public Module_05CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddScenarioAsync(Module scenario)
        {
            await this._container.CreateItemAsync<Module>(scenario, new PartitionKey(scenario.Id));
        }

        public async Task AddScenarioesAsync(Module[] scenario)
        {
            for(int i = 0; i < scenario.Length; i++)
            {
                await this._container.CreateItemAsync<Module>(scenario[i], new PartitionKey(scenario[i].Id));
            }
        }

        public async Task DeleteScenarioAsync(string id)
        {
            await this._container.DeleteItemAsync<Module>(id, new PartitionKey(id));
        }

        public async Task<Module> GetScenarioAsync(string id)
        {
            try
            {
                ItemResponse<Module> response = await this._container.ReadItemAsync<Module>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Module>> GetScenarioesAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Module>(new QueryDefinition(queryString));
            List<Module> results = new List<Module>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateScenarioAsync(string id, Module scenario)
        {
            await this._container.UpsertItemAsync<Module>(scenario, new PartitionKey(id));
        }
    }
}