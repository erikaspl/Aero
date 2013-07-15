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
    public class RFQController : EntitySetController<RFQ, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<RFQ> Get()
        {
            return _context.RFQs;
        }

        [Queryable]
        protected override RFQ GetEntityByKey(int key)
        {
            return _context.RFQs.FirstOrDefault(a => a.Id == key);
        }

        protected override RFQ CreateEntity(RFQ entity)
        {
            try
            {
                _context.RFQs.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override RFQ UpdateEntity(int key, RFQ update)
        {
            if (!_context.RFQs.Any(a => a.Id == key))
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

            _context.RFQs.Attach(update);
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
            var entity = _context.RFQs.FirstOrDefault(a => a.Id == key);
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
            _context.RFQs.Remove(entity);
            _context.SaveChanges();
        }

        protected override RFQ PatchEntity(int key, Delta<RFQ> patch)
        {
            var entity = _context.RFQs.FirstOrDefault(a => a.Id == key);

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

        protected override int GetKey(RFQ entity)
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
