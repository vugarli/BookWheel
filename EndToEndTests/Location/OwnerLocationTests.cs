using BookWheel.Application.Auth;
using BookWheel.Application.Locations.Commands;
using EndToEndTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bogus;

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

            var ownerId = await LocationHelpers.CreateOwnerAsync(HttpClient,registrationRequest);
            
            var token = await AuthHelper.LoginAsync(registrationRequest.Email,registrationRequest.Password,HttpClient);

            var setLocationRequest = new SetLocationCommand();

            setLocationRequest.Start = TimeOnly.Parse("09:00:00");
            setLocationRequest.End = TimeOnly.Parse("12:00:00");
            setLocationRequest.Lat = 24.4;
            setLocationRequest.Long = 24.4;
            setLocationRequest.Name = "SampleLocation";

            var statusCode = await LocationHelpers.SetLocationAsync(HttpClient,setLocationRequest);

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

            var ownerId = await LocationHelpers.CreateOwnerAsync(HttpClient, registrationRequest);

            var setLocationRequest = new SetLocationCommand();

            var token = await AuthHelper.LoginAsync(registrationRequest.Email, registrationRequest.Password, HttpClient);

            setLocationRequest.Start = TimeOnly.Parse("09:00:00");
            setLocationRequest.End = TimeOnly.Parse("12:00:00");
            setLocationRequest.Lat = 24.4;
            setLocationRequest.Long = 24.4;
            setLocationRequest.Name = "SampleLocation";

            await LocationHelpers.SetLocationAsync(HttpClient, setLocationRequest);
            var statusCode = await LocationHelpers.SetLocationAsync(HttpClient, setLocationRequest);

            Assert.True(statusCode != HttpStatusCode.OK);
        }


        
    }
}
