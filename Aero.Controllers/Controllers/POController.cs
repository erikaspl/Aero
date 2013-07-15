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
    public class POController : EntitySetController<PO, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<PO> Get()
        {
            return _context.POes;
        }

        [Queryable]
        protected override PO GetEntityByKey(int key)
        {
            return _context.POes.FirstOrDefault(a => a.Id == key);
        }

        protected override PO CreateEntity(PO entity)
        {
            try
            {
                _context.POes.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override PO UpdateEntity(int key, PO update)
        {
            if (!_context.POes.Any(a => a.Id == key))
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

            _context.POes.Attach(update);
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
            var entity = _context.POes.FirstOrDefault(a => a.Id == key);
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
            _context.POes.Remove(entity);
            _context.SaveChanges();
        }

        protected override PO PatchEntity(int key, Delta<PO> patch)
        {
            var entity = _context.POes.FirstOrDefault(a => a.Id == key);

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

        protected override int GetKey(PO entity)
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
