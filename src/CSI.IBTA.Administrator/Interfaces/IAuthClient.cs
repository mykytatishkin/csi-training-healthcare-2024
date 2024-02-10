namespace CSI.IBTA.Administrator.Interfaces
{
    public interface IAuthClient
    {
        Task<HttpResponseMessage> PostAsync<T>(T dto, string apiEndpoint) where T : class;
    }
}
