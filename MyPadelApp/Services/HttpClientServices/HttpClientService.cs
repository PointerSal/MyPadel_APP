using MyPadelApp.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.Services.HttpClientServices
{
    public class HttpClientService : IHttpClientService
    {
        #region Services

        public string baseUrl = "https://e5d1-151-69-4-233.ngrok-free.app/api/";

        #endregion

        public async Task<GeneralResponse> PostAsync(string url, object data, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
                clientHandler.AllowAutoRedirect = true;
                clientHandler.UseCookies = true;
                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string NewUrl = baseUrl + url;

                if (isToken && await SecureStorage.Default.GetAsync("Token") != null && !string.IsNullOrEmpty(await SecureStorage.Default.GetAsync("Token")))
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await SecureStorage.Default.GetAsync("Token"));

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                StringContent stringContent = new StringContent("");
                var header_data = JsonSerializer.Serialize(data);
                if (header_data != null)
                    stringContent = new StringContent(header_data, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse;
                httpResponse = header_data != null ? await client.PostAsync(baseUrl + url, stringContent) : await client.PostAsync(baseUrl + url, null);
                var responseCon = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseCon);
                return response != null ? response : new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }
    }
}