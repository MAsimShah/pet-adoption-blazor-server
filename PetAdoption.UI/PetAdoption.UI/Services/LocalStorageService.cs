using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Session;

namespace PetAdoption.UI.Services
{
    public class LocalStorageService(ILocalStorageService _localStorage)
    {
        private const string TokenKey = "authToken";

        public async Task SaveTokenAsync(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }

        public async Task<string> GetTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            return token ?? string.Empty;
        }

        public async Task ClearTokenAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
    }
}