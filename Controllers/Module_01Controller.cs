using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthEducationAPI.Models;

namespace NorthEducationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Module_01Controller : ControllerBase
    {
        private readonly IModule_01CosmosDbService _cosmosDbService;

        public Module_01Controller(IModule_01CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetTodoItems()
        {
            var datas = await _cosmosDbService.GetScenarioesAsync("SELECT * FROM c");
            return datas.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Module>> PostScenario(Module[] scenario)
        {
            try
            {
                await _cosmosDbService.AddScenarioesAsync(scenario);
            }
            catch
            {
                return StatusCode(409);
            }
            return CreatedAtAction(nameof(GetScenario), new { id = scenario[0].Id }, scenario);
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Module>>> DeleteAllScenario()
        {
            try
            {
                var datas = await _cosmosDbService.GetScenarioesAsync("SELECT * FROM c");
                var docents = datas.ToList();

                docents.ForEach(async x =>
                {
                    await _cosmosDbService.DeleteScenarioAsync(x.Id);
                });
            }
            catch
            {
                return StatusCode(404);
            }
            return StatusCode(200);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetScenario(string id)
        {
            var docent = await _cosmosDbService.GetScenarioAsync(id);
            if (docent == null)
            {
                return NotFound();
            }
            return docent;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(string id, Module scenario)
        {
            try
            {
                await _cosmosDbService.UpdateScenarioAsync(id, scenario);
            }
            catch
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScenario(string id)
        {
            try
            {
                await _cosmosDbService.DeleteScenarioAsync(id);
            }
            catch
            {
                return StatusCode(404);
            }
            return StatusCode(200);
        }
    }
}
