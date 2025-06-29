using Microsoft.JSInterop;

namespace PetAdoption.UI.Services
{
    public class CookieStorageService
    {
        private readonly IJSRuntime _jSRuntime;

        public CookieStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task SetCookieAsync(string name, string value, int days)
        {
            await _jSRuntime.InvokeVoidAsync("setCookie", name, value, days);
        }

        public async Task DeleteCookieAsync(string name)
        {
            await _jSRuntime.InvokeVoidAsync("deleteCookie", name);
        }

        public async Task<string> GetCookieAsync(string name)
        {
            return await _jSRuntime.InvokeAsync<string>("getCookie", name);
        }
    }
}