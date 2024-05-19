using BookWheel.Application.Auth;
using Newtonsoft.Json;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EndToEndTests.Helpers
{
    

    public static class AuthHelper
    {



        public static async Task<string> LoginAsync(string email,string password,HttpClient httpClient)
        {
            var login = new LoginDto() {Email = email,Password=password,IsCustomer=false };
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/auth/login",content);
            
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());

            httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse.Token);


            return authResponse.Token;
        }



    }
}
