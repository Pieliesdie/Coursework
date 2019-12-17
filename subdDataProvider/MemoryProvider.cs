using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace subdDataProvider
{
    public class MemoryProvider : IMemory
    {
        public bool CanConnect => throw new NotImplementedException();

        public ValueTask<bool> AddArticleAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddAuthorAsync(Authors author)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddCommentAsync(Comments comment)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddDocumentTypeAsync(DocumentTypes documentType)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteArticleAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteAuthorAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteCommentAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> DeleteDocumentTypesAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }

        public Task<List<Articles>> GetArticlesAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Authors>> GetAuthorsAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comments>> GetCommentsAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentTypes>> GetDocumentTypesAsync(int? id)
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

        public ValueTask<bool> UpdateArticleAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateAuthorAsync(Articles article)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateAuthorAsync(Authors author)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateCommentAsync(Comments comment)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> UpdateDocumentTypesAsync(DocumentTypes documentTypes)
        {
            throw new NotImplementedException();
        }

        ValueTask<string> IMemory.ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
