namespace Cryptunics.Web.Controllers
{
    using System.Net;
    using Core;
    using Core.Domain;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CoinsController : ControllerBase
    {
        private readonly ICoinManager _coinManager;

        public CoinsController(ICoinManager coinManager) => _coinManager = coinManager ?? throw new ArgumentNullException(nameof(coinManager));

        [HttpGet("crypto")]
        [ProducesResponseType(typeof(Response<IEnumerable<CryptoCoin>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCryptoCoins() => Ok(new Response<IEnumerable<CryptoCoin>>(await _coinManager.GetAllCryptoCoinsAsync()));

        [HttpGet("fiat")]
        [ProducesResponseType(typeof(Response<IEnumerable<FiatCoin>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllFiatCoins() => Ok(new Response<IEnumerable<FiatCoin>>(await _coinManager.GetAllFiatCoinsAsync()));
    }
}