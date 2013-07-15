using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity.Validation;
using System.Net;
using Microsoft.Data.OData;
using Aero.Model;
using Aero.EF;

namespace Aero.Controllers
{
    public class PartsController : EntitySetController<Part, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All, PageSize=200)]
        public override IQueryable<Part> Get()
        {
            return _context.Parts;
        }

        [Queryable]
        protected override Part GetEntityByKey(int key)
        {
            return _context.Parts.FirstOrDefault(a => a.Id == key);
        }

        protected override Part CreateEntity(Part entity)
        {
            try
            {
                _context.Parts.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override Part UpdateEntity(int key, Part update)
        {
            if (!_context.Parts.Any(a => a.Id == key))
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Part key {0} not found", key)
                    }));
            }

            update.Id = key;

            _context.Parts.Attach(update);
            _context.Entry(update).State = System.Data.EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return update;
        }

        public override void Delete([FromODataUri]int key)
        {
            var entity = _context.Parts.FirstOrDefault(a => a.Id == key);
            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Part key {0} not found", key)
                    }));
            }
            _context.Parts.Remove(entity);
            _context.SaveChanges();
        }

        protected override Part PatchEntity(int key, Delta<Part> patch)
        {
            var entity = _context.Parts.FirstOrDefault(a => a.Id == key);

            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Part key {0} not found", key)
                    }));
            }
            patch.Patch(entity);
            _context.SaveChanges();
            return entity;
        }

        protected override int GetKey(Part entity)
        {
            return entity.Id;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context.Dispose();
        }
    }
}
