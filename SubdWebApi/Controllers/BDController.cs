using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using SubdWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BDController : Controller
    {
        NotConsultantv2Context dbContext;

        public BDController(NotConsultantv2Context context, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = context;
            SecurityHelper.dbContext = context;
            SecurityHelper.User = httpContextAccessor.HttpContext.User;
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            //List<Articles> tmp = new List<Articles>();
            //Random rnd = new Random();
            //for (int i = 0; i < 50; i++)
            //{
            //    tmp.Add(new Articles() { ShortName = $"test{i}", CreateDate = DateTime.Now, LastChange = DateTime.Now, SecurityLabel = rnd.Next(1,4) });
            //}
            //dbContext.Articles.AddRange(tmp);
            //dbContext.SaveChanges();
        }

        [Authorize(Roles = "Admin")]
        [Route("ExecuteCode")]
        public async ValueTask<string> ExecuteCode([FromBody]string command)
        {
            //return null;
            StringBuilder sb = new StringBuilder();
            using (var _command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                _command.CommandText = command;
                _command.CommandType = System.Data.CommandType.Text;
                dbContext.Database.OpenConnection();
                try
                {
                    using (var result = _command.ExecuteReader())
                    {
                        if (result.HasRows)
                        {
                            if (sb.Length > 0) sb.Append("___");
                            while (await result.ReadAsync())
                            {
                                for (int i = 0; i < result.FieldCount; i++)
                                {
                                    if (result.GetValue(i) != DBNull.Value)
                                    {
                                        sb.AppendFormat("{0} ", Convert.ToString(result.GetValue(i)));
                                    }
                                }
                                sb.AppendLine();
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    return e.Message;
                }
            }
            return sb.ToString();
        }

        [Route("GetSecurityLabels")]
        public async Task<IEnumerable<SecurityLabels>> GetSecurityLabels()
        {
            return await dbContext.SecurityLabels.ToListAsync();
        }

        [Route("GetInfo")]
        public async Task<Users> GetInfo()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(v => v.Type == "ID").Value);

            return await dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        [Route("GetArticles")]
        public async Task<IEnumerable<Articles>> GetArticles([FromBody]int? id = null) => id == null
                ? await dbContext.Articles.SecurityFilter().ToListAsync()
                : await dbContext.Articles.SecurityFilter().Where(x => x.Id == id).ToListAsync();

        [Authorize(Roles = "Operator")]
        [Route("AddArticle")]
        public async Task<IActionResult> AddArticle([FromBody]Articles article) => StatusCode((int)(await SecurityHelper.AddEntity(article)));

        [Authorize(Roles = "Operator")]
        [Route("DeleteArticle")]
        public async Task<IActionResult> DeleteArticle([FromBody]int id) => StatusCode((int)(await SecurityHelper.DeleteEntity<Articles>(id)));

        [Authorize(Roles = "Operator,SecurityAdmin")]
        [Route("UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromBody]Articles article) => StatusCode((int)(await SecurityHelper.UpdateEntity(article)));


        [Route("GetComments")]
        public async Task<IEnumerable<Comments>> GetComments([FromBody]int? id = null) => id == null
                ? await dbContext.Comments.SecurityFilter().ToListAsync()
                : await dbContext.Comments.SecurityFilter().Where(x => x.Id == id).ToListAsync();

        [Authorize(Roles = "Operator")]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromBody]Comments comment) => StatusCode((int)(await SecurityHelper.AddEntity(comment)));

        [Authorize(Roles = "Operator")]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromBody]int id) => StatusCode((int)(await SecurityHelper.DeleteEntity<Comments>(id)));

        [Authorize(Roles = "Operator,SecurityAdmin")]
        [Route("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody]Comments comment) => StatusCode((int)(await SecurityHelper.UpdateEntity(comment)));




        [Route("GetAuthors")]
        public async Task<IEnumerable<Authors>> GetAuthors([FromBody]int? id = null) => id == null
              ? await dbContext.Authors.SecurityFilter().ToListAsync()
              : await dbContext.Authors.SecurityFilter().Where(x => x.Id == id).ToListAsync();

        [Authorize(Roles = "Operator")]
        [Route("AddAuthor")]
        public async Task<IActionResult> AddAuthor([FromBody]Authors author) => StatusCode((int)(await SecurityHelper.AddEntity(author)));

        [Authorize(Roles = "Operator")]
        [Route("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor([FromBody]int id) => StatusCode((int)(await SecurityHelper.DeleteEntity<Authors>(id)));

        [Authorize(Roles = "Operator,SecurityAdmin")]
        [Route("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor([FromBody]Authors author) => StatusCode((int)(await SecurityHelper.UpdateEntity(author)));




        [Route("GetDocumentTypes")]
        public async Task<IEnumerable<DocumentTypes>> GetDocumentTypes([FromBody]int? id = null) => id == null
              ? await dbContext.DocumentTypes.SecurityFilter().ToListAsync()
              : await dbContext.DocumentTypes.SecurityFilter().Where(x => x.Id == id).ToListAsync();

        [Authorize(Roles = "Operator")]
        [Route("AddDocumentType")]
        public async Task<IActionResult> AddDocumentType([FromBody]DocumentTypes documentType) => StatusCode((int)(await SecurityHelper.AddEntity(documentType)));

        [Authorize(Roles = "Operator")]
        [Route("DeleteDocumentType")]
        public async Task<IActionResult> DeleteDocumentType([FromBody]int id) => StatusCode((int)(await SecurityHelper.DeleteEntity<DocumentTypes>(id)));

        [Authorize(Roles = "Operator,SecurityAdmin")]
        [Route("UpdateDocumentType")]
        public async Task<IActionResult> UpdateDocumentType([FromBody]DocumentTypes documentType) => StatusCode((int)(await SecurityHelper.UpdateEntity(documentType)));

    }
}