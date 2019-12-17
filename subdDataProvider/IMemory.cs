using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace subdDataProvider
{
    public interface IMemory:IDisposable
    {
        ValueTask<string> ExecuteCommand(string command);

        Task<List<SecurityLabels>> GetSecurityLabels();

        Task<Users> GetInfoAsync();

        Task<List<Articles>> GetArticlesAsync(int? id);

        ValueTask<bool> AddArticleAsync(Articles article);

        ValueTask<bool> DeleteArticleAsync(int? id);

        ValueTask<bool> UpdateArticleAsync(Articles article);



        Task<List<Authors>> GetAuthorsAsync(int? id);

        ValueTask<bool> AddAuthorAsync(Authors author);

        ValueTask<bool> DeleteAuthorAsync(int? id);

        ValueTask<bool> UpdateAuthorAsync(Authors author);



        Task<List<DocumentTypes>> GetDocumentTypesAsync(int? id);

        ValueTask<bool> AddDocumentTypeAsync(DocumentTypes documentType);

        ValueTask<bool> DeleteDocumentTypesAsync(int? id);

        ValueTask<bool> UpdateDocumentTypesAsync(DocumentTypes documentType);


        Task<List<Comments>> GetCommentsAsync(int? id);

        ValueTask<bool> AddCommentAsync(Comments comment);

        ValueTask<bool> DeleteCommentAsync(int? id);

        ValueTask<bool> UpdateCommentAsync(Comments comment);

        bool CanConnect { get; }
    }
}
