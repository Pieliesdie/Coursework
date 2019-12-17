using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace subdDataProvider
{
    public class DBProvider : IMemory, IDisposable
    {
        private bool disposed = false;
        private NotConsultantv2Context dbcontext;

        public DBProvider(DbContextOptions<NotConsultantv2Context> options) => dbcontext = new NotConsultantv2Context(options);

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
                dbcontext.Dispose();
                disposed = true;
            }
        }

        ~DBProvider() => Dispose(false);

        public async Task<List<Articles>> GetArticlesAsync(int? id) => await dbcontext.Articles.ToListAsync();

        public async Task<List<Authors>> GetAuthorsAsync(int? id) => await dbcontext.Authors.ToListAsync();

        public async Task<List<DocumentTypes>> GetDocumentTypesAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> AddAuthorAsync(Authors author)
        {
            try
            {
                var p = await dbcontext.Authors.AddAsync(author);
                dbcontext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public ValueTask<bool> AddArticleAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteArticleAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateArticleAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteAuthorAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateAuthorAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddDocumentTypeAsync(DocumentTypes documentType)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteDocumentTypesAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateDocumentTypesAsync(DocumentTypes documentTypes)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comments>> GetCommentsAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddCommentAsync(Comments comment)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteCommentAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateCommentAsync(Comments comment)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateAuthorAsync(Authors author)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SecurityLabels>> GetSecurityLabels()
        {
            throw new NotImplementedException();
        }

        public string ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }

        ValueTask<string> IMemory.ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }

        public bool CanConnect => dbcontext.Database.CanConnect();
      
    }
}
