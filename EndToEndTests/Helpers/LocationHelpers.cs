using BookWheel.Application.Auth;
using BookWheel.Application.Locations.Commands;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests.Helpers
{
    public static class LocationHelpers
    {

        public static async Task<HttpStatusCode> SetLocationAsync
            (
            HttpClient httpClient,
            SetLocationCommand request
            )
        {
            HttpResponseMessage setLocationMessage = await httpClient.PostAsJsonAsync("/api/Locations", request);
            return setLocationMessage.StatusCode;
        }



        public static async Task<Guid> CreateOwnerAsync
            (
                HttpClient httpClient,
                RegisterDto dto
            )
        {
            var login = new LoginDto();
            login.IsCustomer = false;
            login.Email = dto.Email;
            login.Password = dto.Password;

            HttpResponseMessage registerMessage = await httpClient.PostAsJsonAsync("/api/auth/register", dto);

            HttpResponseMessage loginMessage =
                await httpClient
                .PostAsJsonAsync("/api/auth/login", login);

            var response = await loginMessage.Content.ReadFromJsonAsync<AuthResponse>();

            var jwttoken = new JwtSecurityToken(response.Token);

            var Idstring = jwttoken.Claims.Where(c => c.Type == "nameid").First().Value;

            return Guid.Parse(Idstring);
        }
    }
}
