using Microsoft.EntityFrameworkCore;
using Model;
using subdDataProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace subdClientApp
{
    public enum DataSource
    {
        DB,
        InMemory,
        WebAPI
    }

    public class DataReader:IDisposable
    {
        private bool disposed = false;
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
                Provider?.Dispose();
                disposed = true;
            }
        }

        ~DataReader() => Dispose(false);

        IMemory Provider;

        public DataReader(DataSource dataSource,string name,string password)
        {
            switch (dataSource)
            {
                case DataSource.DB:
                    {
                        var options = new DbContextOptionsBuilder<NotConsultantv2Context>();
                        options.UseSqlServer($"Server = DESKTOP-MK32EF2; Database = NotConsultant; user id = {name}; password = {password};");
                        Provider = new DBProvider(options.Options);
                        break;
                    }
                case DataSource.InMemory:
                    {
                        Provider = new MemoryProvider();
                        break;
                    }
                case DataSource.WebAPI:
                    {
                        Provider = new WebAPIProvider(name,password,Properties.Settings.Default.Server/* App.WebApiHost*/);
                        break;
                    }
            }
        }

        public async ValueTask<string> ExecCode(string command)
        {
            return await Provider.ExecuteCommand(command);
        }

        public async Task<List<SecurityLabels>> GetSecurityLabelsAsync()
        {
            return await Provider.GetSecurityLabels();
        }
        public async Task<Users> GetInfoAsync()
        {
            return await Provider.GetInfoAsync();
        }

        public async Task<List<Authors>> GetAuthorsAsync(int? id=null)
        {
            return await Provider.GetAuthorsAsync(id);
        }

        public async ValueTask<bool> AddAuthorAsync(Authors author)
        {
            return await Provider.AddAuthorAsync(author);
        }

        public async ValueTask<bool> UpdateAuthorAsync(Authors author)
        {
            return await Provider.UpdateAuthorAsync(author);
        }

        public async ValueTask<bool> DeleteAuthorAsync(int id)
        {
            return await Provider.DeleteAuthorAsync(id);
        }

        public async ValueTask<bool> AddArticleAsync(Articles article)
        {
            return await Provider.AddArticleAsync(article);
        }

        public async ValueTask<bool> UpdateArticleAsync(Articles article)
        {
            return await Provider.UpdateArticleAsync(article);
        }

        public async ValueTask<bool> DeleteArticleAsync(int id)
        {
            return await Provider.DeleteArticleAsync(id);
        }

        public async ValueTask<bool> AddDocumentTypeAsync(DocumentTypes document)
        {
            return await Provider.AddDocumentTypeAsync(document);
        }

        public async ValueTask<bool> UpdateDocumentTypeAsync(DocumentTypes document)
        {
            return await Provider.UpdateDocumentTypesAsync(document);
        }

        public async ValueTask<bool> DeleteDocumentTypeAsync(int id)
        {
            return await Provider.DeleteDocumentTypesAsync(id);
        }

        public async Task<List<Articles>> GetArticlesAsync(int? id = null)
        {
            return await Provider.GetArticlesAsync(id);
        }

        public async Task<List<DocumentTypes>> GetDocumentTypesAsync(int? id = null)
        {
            return await Provider.GetDocumentTypesAsync(id);
        }


        public bool CanConnect => Provider.CanConnect;
    }
}
