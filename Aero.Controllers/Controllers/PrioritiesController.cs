using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.OData;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Data.Entity.Validation;
using System.Net;
using Microsoft.Data.OData;
using System.Net.Http;
using Aero.Model;
using Aero.EF;

namespace Aero.Controllers
{
    public class PrioritiesController : EntitySetController<Priority, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Priority> Get()
        {
            return _context.Priorities;
        }

        [Queryable]
        protected override Priority GetEntityByKey(int key)
        {
            return _context.Priorities.FirstOrDefault(a => a.Id == key);
        }

        protected override Priority CreateEntity(Priority entity)
        {
            try
            {
                _context.Priorities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override Priority UpdateEntity(int key, Priority update)
        {
            if (!_context.Priorities.Any(a => a.Id == key))
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Vendor key {0} not found", key)
                    }));
            }

            update.Id = key;

            _context.Priorities.Attach(update);
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
            var entity = _context.Priorities.FirstOrDefault(a => a.Id == key);
            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Vendor key {0} not found", key)
                    }));
            }
            _context.Priorities.Remove(entity);
            _context.SaveChanges();
        }

        protected override Priority PatchEntity(int key, Delta<Priority> patch)
        {
            var entity = _context.Priorities.FirstOrDefault(a => a.Id == key);

            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Vendor key {0} not found", key)
                    }));
            }
            patch.Patch(entity);
            _context.SaveChanges();
            return entity;
        }

        protected override int GetKey(Priority entity)
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
