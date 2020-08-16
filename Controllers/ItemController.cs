using CosmoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IDocumentClient _documentClient;
        private readonly String _databaseId;
        private readonly String _collectionId;


        ///private readonly IConfiguration _configuration; 
        IConfiguration Configuration { get; }

        public ItemController(IDocumentClient documentClient, IConfiguration configuration)
        {
            _documentClient = documentClient;
            //configuration = _configuration;
            Configuration = configuration;
            _databaseId = configuration["DatabaseId"];
            _collectionId = "Items";

            BuildCollection().Wait();

        }

        private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseId),
                new DocumentCollection { Id = _collectionId });
        }


        [HttpGet]
        public IQueryable<Item> Get()
        {
            return _documentClient.CreateDocumentQuery<Item>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), new FeedOptions { MaxItemCount = 20 });
        }

        [HttpGet("{id}")]
        public IQueryable<Item> Get(string id)
        {
            return _documentClient.CreateDocumentQuery<Item>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => i.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var response = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);

            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Item item)
        {
            await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));

            return Ok();
        }



    }
}
