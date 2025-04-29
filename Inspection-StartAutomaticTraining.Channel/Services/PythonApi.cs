using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using InspectionStartAutomaticTraining.Messages.Dtos;

namespace InspectionStartAutomaticTraining.Channel
{
    public class PythonAPI
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string Url;
        private readonly string _username;
        private readonly string _password;

        public PythonAPI(string url, string username, string password)
        {
            Url = url;
            _username = username;
            _password = password;
        }

        public async Task<string?> SendToImageTrainingAPI(string url, AutomaticTrainingRequest message)
        {
            try
            {
                var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                string requestUrl = $"{Url + url}?model_url={Uri.EscapeDataString(message.ModelUrl)}&numberofimgs={message.NumberofImgs}&county={Uri.EscapeDataString(message.county)}";

                HttpResponseMessage response = await _client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
