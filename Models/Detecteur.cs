using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DetectionLangue.Models
{
    public class Detecteur : IDisposable
    {
        private string _urlBaseApi;
        private HttpClient _httpClient;
        public string DetectText { get; set; }
        public Detecteur(string urlBaseApi) 
        {
            if (urlBaseApi.EndsWith('/'))
                urlBaseApi = urlBaseApi.Substring(0, urlBaseApi.Length - 1);
            _urlBaseApi = urlBaseApi;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }
        public async Task<string> RequeteGetAsync(string endpoint)
        {
            HttpResponseMessage hrm = await _httpClient.GetAsync(_urlBaseApi + endpoint);
            return await hrm.Content.ReadAsStringAsync();
        }

        public void SetHttpRequestHeader(string header, string val)
        {
            _httpClient.DefaultRequestHeaders.Add(header, val);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
