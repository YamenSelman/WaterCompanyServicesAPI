using Newtonsoft.Json;
using WaterCompanyModelLibrary;
using WaterCompanyServices.Areas.Consumer.Models;

namespace WaterCompanyServices.Areas.Home.DataAccess
{
    public static class HomeDA
    {
        static HttpClient client = new HttpClient();
        //static string BaseUrl = "http://watercompanyservicesapi.somee.com/";
        static string BaseUrl = $"http://localhost:7186/";
        
        public static async Task<User> Login(string userName,string password)
        {
            return JsonConvert.DeserializeObject<User>(await client.GetStringAsync($"{BaseUrl}Consumer"));
        }
        
        public static async Task<IEnumerable<WaterCompanyModelLibrary.Consumer>> GetConsumerAccounts()
        {
            var result = JsonConvert.DeserializeObject<IEnumerable<WaterCompanyModelLibrary.Consumer>>(await client.GetStringAsync($"https://localhost:7186/consumer"));
            return result;
        }
    }
}
