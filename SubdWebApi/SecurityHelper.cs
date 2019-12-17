using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;


namespace SubdWebApi
{
    public static class SecurityHelper
    {
        public static NotConsultantv2Context dbContext { get; set; }

        public static ClaimsPrincipal User { get; set; }

        public static IQueryable<T> SecurityFilter<T>(this IQueryable<T> source) where T:IPermission
        {
            var userid = int.Parse(User.Claims.FirstOrDefault(v => v.Type == "ID").Value);
            var permissions = dbContext.UserReadPermissions(userid).Select(x => x.Id);

            return source.Where(x => permissions.Contains(x.SecurityLabel ?? -1));
        }

        public async static Task<HttpStatusCode> AddEntity<T>(T entity) where T:class,IPermission
        {
            if (entity == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var id = int.Parse(User.Claims.FirstOrDefault(v => v.Type == "ID").Value);
            var permissions = dbContext.UserWritePermissions(id).Select(x => x.Id);
            if (!permissions.Contains(entity.SecurityLabel ?? -1))
            {
                return HttpStatusCode.Forbidden;
            }

            await dbContext.Set<T>().AddAsync(entity);
            var count = await dbContext.SaveChangesAsync();
            if (count <= 0)
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Created;
        }

        public async static Task<HttpStatusCode> DeleteEntity<T>(int id) where T : class, IPermission
        {
            var table = dbContext.Set<T>();
            var permission = int.Parse(User.Claims.FirstOrDefault(v => v.Type == "Permission").Value);
            T entity = table.Find(id);

            if (permission != entity.SecurityLabel)
            {
                return HttpStatusCode.Forbidden;
            }

            dbContext.Set<T>().Remove(entity);
            var count = await dbContext.SaveChangesAsync();
            if (count <= 0)
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }

        public async static Task<HttpStatusCode> UpdateEntity<T>( T entity) where T : class, IPermission
        {
            var table = dbContext.Set<T>();
            if (entity == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var entityInTable = table.Find(entity.Id);

            if (entityInTable == null)
                return HttpStatusCode.NotFound;

            var permission = int.Parse(User.Claims.FirstOrDefault(v => v.Type == "Permission").Value);

            if( !User.IsInRole("SecurityAdmin") && (entityInTable.SecurityLabel!=entity.SecurityLabel || permission != entityInTable.SecurityLabel))
                return HttpStatusCode.Forbidden;

            dbContext.Entry<T>(entityInTable).CurrentValues.SetValues(entity);
            var count = await dbContext.SaveChangesAsync();
            if (count <= 0)
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }
    }
}
