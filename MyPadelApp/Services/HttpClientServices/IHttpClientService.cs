using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.HttpClientServices
{
    public interface IHttpClientService
    {
        Task<GeneralResponse> DeleteAsync(string url, object data, bool isToken);
        Task<GeneralResponse> GetAsync(string url, bool isToken);
        Task<GeneralResponse> PostAsync(string url, object data, bool isToken);
        Task<GeneralResponse> PostMultipartAsync(string url, Dictionary<string, string> formData, string filePath, string fileFieldName, bool IsPost = true);
        Task<GeneralResponse> PutAsync(string url, object data, bool isToken);
    }
}