using BookWheel.Application.Auth;
using BookWheel.Application.Locations.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests.Location
{
    public class OwnerLocationTests 
        : BaseEndToEndTest
        
    {

        public OwnerLocationTests(EndToEndTestingWebAppFactory factory)
            :base(factory){}


        [Fact]
        public async void OwnerShouldSetLocation()
        {
            var registrationRequest = new RegisterDto();
            registrationRequest.Email = "ot@ot.com";
            registrationRequest.Password = "Vugar2003Vs$";
            registrationRequest.IsCustomer = false;
            registrationRequest.DisplayName = "Test2";

            var ownerId = await CreateOwnerAsync(HttpClient,registrationRequest);

            var setLocationRequest = new SetLocationCommand();
            setLocationRequest.OwnerId = ownerId;
            setLocationRequest.Start = TimeOnly.Parse("09:00:00");
            setLocationRequest.End = TimeOnly.Parse("12:00:00");
            setLocationRequest.Lat = 24.4;
            setLocationRequest.Long = 24.4;
            setLocationRequest.Name = "SampleLocation";

            var statusCode = await SetLocationAsync(HttpClient,setLocationRequest);

            Assert.True(statusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async void FAIL_OwnerShouldSetLocation()
        {
            var registrationRequest = new RegisterDto();
            registrationRequest.Email = "oy@oy.com";
            registrationRequest.Password = "Vugar2003Vs$";
            registrationRequest.IsCustomer = false;
            registrationRequest.DisplayName = "Test2";

            var ownerId = await CreateOwnerAsync(HttpClient, registrationRequest);

            var setLocationRequest = new SetLocationCommand();
            setLocationRequest.OwnerId = ownerId;
            setLocationRequest.Start = TimeOnly.Parse("09:00:00");
            setLocationRequest.End = TimeOnly.Parse("12:00:00");
            setLocationRequest.Lat = 24.4;
            setLocationRequest.Long = 24.4;
            setLocationRequest.Name = "SampleLocation";

            await SetLocationAsync(HttpClient, setLocationRequest);
            var statusCode = await SetLocationAsync(HttpClient, setLocationRequest);

            Assert.True(statusCode != HttpStatusCode.OK);
        }


        public async Task<HttpStatusCode> SetLocationAsync
            (
            HttpClient httpClient,
            SetLocationCommand request
            )
        {
            HttpResponseMessage setLocationMessage = await HttpClient.PostAsJsonAsync("/api/Locations", request);
            return setLocationMessage.StatusCode;
        }



        public async Task<Guid> CreateOwnerAsync
            (
                HttpClient httpClient,
                RegisterDto dto
            )
        {
            var login = new LoginDto();
            login.IsCustomer = false;
            login.Email = dto.Email;
            login.Password = dto.Password;

            HttpResponseMessage registerMessage = await HttpClient.PostAsJsonAsync("/api/auth/register", dto);
            
            HttpResponseMessage loginMessage = 
                await HttpClient
                .PostAsJsonAsync("/api/auth/login", login);

            var response = await loginMessage.Content.ReadFromJsonAsync<AuthResponse>();

            var jwttoken = new JwtSecurityToken(response.Token);

            var Idstring = jwttoken.Claims.Where(c=>c.Type == "nameid").First().Value;

            return Guid.Parse(Idstring);
        }
    }
}
