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
    public class VendorsController : EntitySetController<Vendor, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Vendor> Get()
        {
            return _context.Vendors;
        }

        [Queryable]
        protected override Vendor GetEntityByKey(int key)
        {
            return _context.Vendors.FirstOrDefault(a => a.Id == key);
        }

        protected override Vendor CreateEntity(Vendor entity)
        {
            try
            {
                _context.Vendors.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override Vendor UpdateEntity(int key, Vendor update)
        {
            if (!_context.Vendors.Any(a => a.Id == key))
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

            _context.Vendors.Attach(update);
            _context.Entry(update).State = System.Data.EntityState.Modified;
            foreach (var contact in _context.Contacts.Where(c => c.Id == update.ContactId))
            {
                _context.Entry(contact).State = System.Data.EntityState.Modified;
            }
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
            var entity = _context.Vendors.FirstOrDefault(a => a.Id == key);
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
            _context.Vendors.Remove(entity);
            _context.SaveChanges();
        }

        protected override Vendor PatchEntity(int key, Delta<Vendor> patch)
        {
            var entity = _context.Vendors.FirstOrDefault(a => a.Id == key);

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

        protected override int GetKey(Vendor entity)
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
