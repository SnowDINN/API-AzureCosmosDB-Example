namespace NorthEducationAPI
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NorthEducationAPI.Models;

    public interface IModule_02CosmosDbService
    {
        Task<IEnumerable<Module>> GetScenarioesAsync(string query);
        Task<Module> GetScenarioAsync(string id);
        Task AddScenarioAsync(Module scenario);
        Task AddScenarioesAsync(Module[] scenario);
        Task UpdateScenarioAsync(string id, Module user);
        Task DeleteScenarioAsync(string id);
    }
}