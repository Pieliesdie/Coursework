using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using Model;

namespace subdDataProvider
{
    public class WebAPIProvider : IMemory,IDisposable
    {
        private bool disposed = false;
        public string token { get; set; }
         readonly HttpClient client = new HttpClient();
        private string WebApiHost;

        public void Dispose()
        {
            Dispose(true);
            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                }
                client.Dispose();
                disposed = true;
            }
        }

        ~WebAPIProvider() => Dispose(false);

        public WebAPIProvider(string username, string password, string host)
        {
            try
            {
                WebApiHost = host;
                InitTokenAsync(username, password).Wait();
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    CanConnect = true;
                }
                else
                    CanConnect = false;
            }
            catch (Exception e)
            {
                CanConnect = false;
            }
        }

        private async Task InitTokenAsync(string usr, string password)
        {
            HttpResponseMessage response = await client.GetAsync($"{WebApiHost}/token?username={usr}&password={password}").ConfigureAwait(true);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            dynamic json1 = JsonConvert.DeserializeObject(json);
            token = json1.access_token;
        }

        public bool CanConnect { get; set; }

        public async ValueTask<bool> AddAuthorAsync(Authors author)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(author), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/AddAuthor", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async Task<List<Articles>> GetArticlesAsync(int? id = null)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/GetArticles", stringContent);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Articles>>(json);
            }
            catch (Exception) { return null; }
        }

        public async Task<List<Authors>> GetAuthorsAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/GetAuthors", stringContent);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Authors>>(json);
            }
            catch (Exception) { return null; }
        }

        public async Task<List<DocumentTypes>> GetDocumentTypesAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/GetDocumentTypes", stringContent);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DocumentTypes>>(json);
            }
            catch (Exception) { return null; }
        }

        public async ValueTask<bool> AddArticleAsync(Articles article)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(article), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/AddArticle", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> DeleteArticleAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/DeleteArticle", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> UpdateArticleAsync(Articles article)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(article), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/UpdateArticle", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> DeleteAuthorAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/DeleteAuthor", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> UpdateAuthorAsync(Authors author)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(author), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/UpdateAuthor", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> AddDocumentTypeAsync(DocumentTypes documentType)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(documentType), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/AddDocumentType", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> DeleteDocumentTypesAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/DeleteDocumentType", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> UpdateDocumentTypesAsync(DocumentTypes documentType)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(documentType), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/UpdateDocumentType", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async Task<List<Comments>> GetCommentsAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/GetComments", stringContent);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Comments>>(json);
            }
            catch (Exception) { return null; }
        }

        public async ValueTask<bool> AddCommentAsync(Comments comment)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(comment), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/AddComment", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> DeleteCommentAsync(int? id)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(id), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/DeleteComment", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async ValueTask<bool> UpdateCommentAsync(Comments comment)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(comment), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/UpdateComment", stringContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public async Task<Users> GetInfoAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{WebApiHost}/api/bd/GetInfo").ConfigureAwait(true);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Users>(json);
            }
            catch (Exception) { return null; }
        }

        public async Task<List<SecurityLabels>> GetSecurityLabels()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{WebApiHost}/api/bd/GetSecurityLabels").ConfigureAwait(true);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SecurityLabels>>(json);
            }
            catch (Exception) { return null; }
        }

        public async ValueTask<string> ExecuteCommand(string command)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(command), encoding: Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{WebApiHost}/api/bd/ExecuteCode", stringContent);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception) { return null; }
        }
    }
}
