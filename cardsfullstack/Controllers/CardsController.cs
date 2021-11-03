using cardsfullstack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardsfullstack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<IEnumerable<Card>> runTest()
        {
            return await DAL.InitializeDeck();
        }

        [HttpGet("deck")]
        public async Task<IEnumerable<Card>> GetDeck()
        {
            return await DAL.InitializeDeck();
        }

        [HttpGet("cards/{id}")]
        public async Task<IEnumerable<Card>> GetCards(string id)
        {
            return await DAL.DrawTwoCards(id);
        } 

    }   
}
