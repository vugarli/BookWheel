using BookWheel.Application.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests.ApplicationUserRoot
{
    public class CreateUserTests
        : BaseEndToEndTest
    {
        public CreateUserTests(EndToEndTestingWebAppFactory factory)
            : base(factory) { }


        [Fact]
        public async Task ShouldCreateCustomer()
        {
            var request = new RegisterDto();
            request.Email = "oo@oo.com";
            request.Password = "Vugar2003Vs$";
            request.IsCustomer = true;
            request.DisplayName = "Test1";

            HttpResponseMessage message = await HttpClient.PostAsJsonAsync("/api/auth/register",request);
            
            Assert.True(message.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task ShouldCreateOwner()
        {
            var request = new RegisterDto();
            request.Email = "ow@ow.com";
            request.Password = "Vugar2003Vs$";
            request.IsCustomer = false;
            request.DisplayName = "Test2";

            HttpResponseMessage message = await HttpClient.PostAsJsonAsync("/api/auth/register", request);

            Assert.True(message.StatusCode == HttpStatusCode.OK);
        }


    }
}
