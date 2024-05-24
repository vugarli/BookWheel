using BookWheel.Application.Locations.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWheel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public SearchController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public IMediator Mediator { get; }

        [HttpGet("{SearchTerm}")]
        public async Task<IActionResult> SearchAsync([FromRoute] SearchLocationByNameQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

    }
}
