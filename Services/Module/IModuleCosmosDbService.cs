using System.Collections.Generic;
using System.Threading.Tasks;
using Anonymous.Models;

namespace Anonymous.ADT
{
    public interface IModuleCosmosDbService
    {
        Task<IEnumerable<Module>> GetItemsAsync(string query);
        Task<Module> GetItemAsync(string id);
        Task AddItemAsync(Module item);
        Task AddItemsAsync(Module[] items);
        Task UpdateItemAsync(string id, Module user);
        Task DeleteItemAsync(string id);
    }
}