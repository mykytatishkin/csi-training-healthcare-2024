namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IAuthorizedHttpClient
    {
        string GetBaseAddress(string urlConfigurationString);
        void SetBaseAddress(string urlConfigurationString);
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> PutAsyc(string url, HttpContent content);
        Task<HttpResponseMessage> PatchAsync(string url, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string url);
    }
}