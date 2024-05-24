using BookWheel.Application.Auth;
using BookWheel.Application.Locations.Commands;
using BookWheel.Application.LocationServices.Commands;
using BookWheel.Application.LocationServices.Dtos;
using BookWheel.Infrastructure;
using Dapper;
using EndToEndTests.Helpers;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests.Location
{
    public class LocationServicesTests 
        : BaseEndToEndTest
    {
        public LocationServicesTests(EndToEndTestingWebAppFactory factory)
            : base(factory) 
        {
            Connection = factory.Connection;
        }

        public SqlConnection Connection { get; set; }

        public async Task<Guid> Init()
        {
            var registerRequest = new RegisterDto()
            {
                Email = Email,
                Password = Password,
                IsCustomer = false,
                DisplayName = "a"
            };

            var id = await LocationHelpers.CreateOwnerAsync(HttpClient,registerRequest);
            await AuthHelper.LoginAsync(Email, Password, HttpClient);
            var setLocationRequest = new SetLocationCommand();
             
            setLocationRequest.Start = TimeOnly.Parse("09:00:00");
            setLocationRequest.End = TimeOnly.Parse("12:00:00");
            setLocationRequest.Lat = 24.4;
            setLocationRequest.Long = 24.4;
            setLocationRequest.Name = "SampleLocation";

            await LocationHelpers.SetLocationAsync(HttpClient, setLocationRequest);
            return id;
        }

        public const string Email = "ss@s.com";
        public const string Password = "Vugar2003Vs$";
        public SharedDatabaseFixture Fixture { get; }

        [Fact]
        public async Task SuccessfullyAddsServiceToLocation()
        {
            var Id = await Init();
            
            var locationId = await Connection.QueryFirstOrDefaultAsync<Guid>
                ($"SELECT Id FROM Location WHERE OwnerId = '{Id}'");

            var addServiceRequest = new
            {
                Name = "D",
                Description = "D",
                MinuteDuration = 30,
                Price = 34
            };

            var statusCode = await HttpClient
                .PostAsJsonAsync($"/api/locations/{locationId}/services",addServiceRequest);

            Assert.True(statusCode.IsSuccessStatusCode);

            var servicesStr = await HttpClient
                .GetStringAsync($"/api/locations/{locationId}/services");

            var services = JsonConvert.DeserializeObject<IEnumerable<ServiceDto>>(servicesStr);

            Assert.NotEmpty(services);
            Assert.True(services.First().Name == "D");
        }




    }
}
