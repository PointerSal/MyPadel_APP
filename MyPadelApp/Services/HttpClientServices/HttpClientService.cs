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

        public string baseUrl = "http://ufficio.pointer.re.it:7070/api/";

        #endregion

        public async Task<GeneralResponse> PostAsync(string url, object data, bool isToken, bool IsPost = true)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
                clientHandler.AllowAutoRedirect = true;
                clientHandler.UseCookies = true;
                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = TimeSpan.FromMinutes(2)
                };

                string NewUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken && await SecureStorage.Default.GetAsync("Token") != null && !string.IsNullOrEmpty(await SecureStorage.Default.GetAsync("Token")))
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await SecureStorage.Default.GetAsync("Token"));

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                StringContent stringContent = new StringContent("");
                var header_data = JsonSerializer.Serialize(data);
                if (header_data != null)
                    stringContent = new StringContent(header_data, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse;
                if(IsPost)
                    httpResponse = header_data != null ? await client.PostAsync(baseUrl + url, stringContent) : await client.PostAsync(baseUrl + url, null);
                else
                    httpResponse = header_data != null ? await client.PatchAsync(baseUrl + url, stringContent) : await client.PatchAsync(baseUrl + url, null);
                var responseCon = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseCon);
                return response != null ? response : new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }
        //public async Task<GeneralResponse> PostMultipartAsync(string url, Dictionary<string, string> formData, string filePath, string fileFieldName, bool IsPost = true)
        //{
        //    try
        //    {
        //        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        //        //using HttpClientHandler clientHandler = new HttpClientHandler
        //        //{
        //        //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        //        //};

        //        using HttpClient client = new HttpClient();

        //        string fullUrl = baseUrl + url;
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Clear();

        //        string token = await SecureStorage.Default.GetAsync("Token");
        //        if (!string.IsNullOrEmpty(token))
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        using MultipartFormDataContent form = new MultipartFormDataContent();

        //        // Add form data
        //        foreach (var entry in formData)
        //        {
        //            form.Add(new StringContent(entry.Value), entry.Key);
        //        }

        //        // Handle file upload properly
        //        StreamContent fileContent = null;
        //        FileStream fileStream = null;

        //        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        //        {
        //            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //            fileContent = new StreamContent(fileStream);
        //            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        //            form.Add(fileContent, fileFieldName, Path.GetFileName(filePath));
        //        }

        //        Console.WriteLine($"URL: {baseUrl}{url}");
        //        Console.WriteLine($"Token: {token}");
        //        foreach (var entry in formData)
        //        {
        //            Console.WriteLine($"Key: {entry.Key}, Value: {entry.Value}");
        //        }
        //        Console.WriteLine($"File Path: {filePath}");

        //        HttpResponseMessage response;
        //        if (IsPost)
        //            response = await client.PostAsync(fullUrl, form);
        //        else
        //        {
        //            var request = new HttpRequestMessage(new HttpMethod("PATCH"), fullUrl)
        //            {
        //                Content = form
        //            };

        //            response = await client.SendAsync(request);
        //        }

        //        string responseString = await response.Content.ReadAsStringAsync();

        //        // Ensure the stream is closed after the request is completed
        //        fileStream?.Dispose();
        //        fileContent?.Dispose();

        //        return JsonSerializer.Deserialize<GeneralResponse>(responseString) ?? new GeneralResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in PostMultipartAsync: {ex}");
        //    }

        //    return new GeneralResponse();
        //}

        public async Task<GeneralResponse> PutAsync(string url, object data, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = await SecureStorage.Default.GetAsync("Token");
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                StringContent stringContent = new StringContent("");
                if (data != null)
                {
                    var jsonData = JsonSerializer.Serialize(data);
                    stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage httpResponse = await client.PutAsync(fullUrl, stringContent);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);
                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }

        public async Task<GeneralResponse> DeleteAsync(string url, object data, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = await SecureStorage.Default.GetAsync("Token");
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                // Convert payload to JSON string
                StringContent stringContent = new StringContent(string.Empty);
                if (data != null)
                {
                    string jsonData = JsonSerializer.Serialize(data);
                    stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(fullUrl),
                    Content = stringContent
                };

                HttpResponseMessage httpResponse = await client.SendAsync(request);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);
                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }
        public async Task<GeneralResponse> GetAsync(string url, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = await SecureStorage.Default.GetAsync("Token");
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponse = await client.GetAsync(fullUrl);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);

                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex}");
            }

            return new GeneralResponse();
        }

    }
}