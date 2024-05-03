using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests
{
    public class BaseEndToEndTest : IClassFixture<EndToEndTestingWebAppFactory>
    {
        public HttpClient HttpClient { get; set; }

        public BaseEndToEndTest(EndToEndTestingWebAppFactory endToEndTestingWebAppFactory)
        {
            HttpClient = endToEndTestingWebAppFactory.CreateClient();
        }
    }
}
