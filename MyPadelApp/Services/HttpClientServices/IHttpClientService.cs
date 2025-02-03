using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.HttpClientServices
{
    public interface IHttpClientService
    {
        Task<GeneralResponse> PostAsync(string url, object data, bool isToken);
    }
}