using Avenga.NotesApp.Dto.FruitDtos;
using Avenga.NotesApp.Services.Interfaces;
using System.Text.Json;

namespace Avenga.NotesApp.Services.Implementations
{
    public class FruitService : IFruitService
    {
        //http is a class that enables us to send http requests and recieve http responses!
        private readonly HttpClient _httpClient;

        public FruitService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FruitDto>> GetAllFruitsAsync()
        {
            //we are calling the api address with our httpclient and getting the provided data
            //with other words, we are reaching out to some external api location
            var response = await _httpClient.GetAsync("https://www.fruityvice.com/api/fruit/all");

            if(response.IsSuccessStatusCode)
            {
                // this will read the content that we get as a string
                var json = await response.Content.ReadAsStringAsync();

                //we deserialize the string and set naming policy for the data to be in camel case
                return JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            return null;
        }

        public async Task<FruitDto> GetFruitInfoAsync(string fruitName)
        {
            var response = await _httpClient.GetAsync("https://www.fruityvice.com/api/fruit/all");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var fruitByName = result.FirstOrDefault(x => x.Name.ToLower() == fruitName.ToLower());
                return fruitByName;
            }

            return null;
        }

        public async Task<List<FruitDto>> GetFruitsByFamilyAsync(string familyName)
        {
            var response = await _httpClient.GetAsync($"https://www.fruityvice.com/api/fruit/family/{familyName}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            return null;
        }

        public async Task<List<FruitDto>> GetFruitsByGenusAsync(string genusName)
        {
            var response = await _httpClient.GetAsync($"https://www.fruityvice.com/api/fruit/genus/{genusName}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            return null;
        }

        public async Task<List<FruitDto>> GetFruitsByOrderAsync(string orderName)
        {
            var response = await _httpClient.GetAsync($"https://www.fruityvice.com/api/fruit/order/{orderName}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return result;
            }

            return null;
        }
    }
}
