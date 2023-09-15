using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anonymous.ADT;
using Anonymous.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anonymous.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleCosmosDbService _cosmosDbService;

        public ModuleController(IModuleCosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetItems()
        {
            var datas = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return datas.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetItem(string id)
        {
            var docent = await _cosmosDbService.GetItemAsync(id);
            if (docent == null) return NotFound();
            return docent;
        }

        [HttpPost]
        public async Task<ActionResult<Module>> PostItems(Module[] scenario)
        {
            try
            {
                await _cosmosDbService.AddItemsAsync(scenario);
            }
            catch
            {
                return StatusCode(409);
            }

            return CreatedAtAction(nameof(GetItem), new { id = scenario[0].Id }, scenario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(string id, Module scenario)
        {
            try
            {
                await _cosmosDbService.UpdateItemAsync(id, scenario);
            }
            catch
            {
                return StatusCode(400);
            }

            return StatusCode(200);
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Module>>> DeleteAllItems()
        {
            try
            {
                var datas = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
                var docents = datas.ToList();

                docents.ForEach(async x => { await _cosmosDbService.DeleteItemAsync(x.Id); });
            }
            catch
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteitem(string id)
        {
            try
            {
                await _cosmosDbService.DeleteItemAsync(id);
            }
            catch
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }
    }
}