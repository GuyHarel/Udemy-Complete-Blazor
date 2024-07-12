using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Services
{
    public class BookStoreApiClientFactory
    {
        private readonly IHttpClientFactory factory;
        public BookStoreApiClientFactory(IHttpClientFactory factory)
        {
              this.factory = factory;
        }

        public BookStoreAppApiClient CreateApiClient()
        {
            return new BookStoreAppApiClient("https://localhost:7094", factory.CreateClient());
        }
    }
}
