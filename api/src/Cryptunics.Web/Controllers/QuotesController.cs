namespace Cryptunics.Web.Controllers
{
    using Core;
    using Core.Domain;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class QuotesController : ControllerBase
    {
        private readonly IExchange _exchange;

        public QuotesController(IExchange exchange) => _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));

        [HttpGet("latest/{cryptoCoinId}")]
        [ProducesResponseType(typeof(Response<Quote>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLatestQuote(int cryptoCoinId) => Ok(new Response<Quote>(await _exchange.GetLatestQuoteAsync(cryptoCoinId)));
    }
}
