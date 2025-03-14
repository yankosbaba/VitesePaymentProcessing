namespace Vitese_Payment_Processing.API.BaseClient
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint) where T : class;
        Task<T> TestharnessPostAsync<T>(string endpoint, object requestObject) where T : class;
    }
}
