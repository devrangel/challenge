using Api.Entities;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class GitDataController : ControllerBase
    {
        private readonly IGithubService _service;

        public GitDataController(IGithubService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<JsonGithubData>>> Get()
        {
            var response = await _service.GetLastFiveCSharpRepos();
            
            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
