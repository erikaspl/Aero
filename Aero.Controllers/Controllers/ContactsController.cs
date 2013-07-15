using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Data.Entity.Validation;
using System.Net.Http;
using System.Net;
using Microsoft.Data.OData;
using Aero.Model;
using Aero.EF;

namespace Aero.Controllers
{
    public class ContactsController : EntitySetController<Contact, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Contact> Get()
        {
            return _context.Contacts;
        }

        [Queryable]
        protected override Contact GetEntityByKey(int key)
        {
            return _context.Contacts.FirstOrDefault(a => a.Id == key);
        }

        public Contact GetContactsFromVendors([FromODataUri] int key)
        {
            return _context.Vendors.Where(ai => ai.Id == key).First().Contact;
        }

        protected override Contact CreateEntity(Contact entity)
        {
            try
            {
                _context.Contacts.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override Contact UpdateEntity(int key, Contact update)
        {
            if (!_context.Contacts.Any(a => a.Id == key))
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Contact key {0} not found", key)
                    }));
            }

            update.Id = key;

            _context.Contacts.Attach(update);
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
            var entity = _context.Contacts.FirstOrDefault(a => a.Id == key);
            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Contact key {0} not found", key)
                    }));
            }
            _context.Contacts.Remove(entity);
            _context.SaveChanges();
        }

        protected override Contact PatchEntity(int key, Delta<Contact> patch)
        {
            var entity = _context.Contacts.FirstOrDefault(a => a.Id == key);

            if (entity == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Artist key {0} not found", key)
                    }));
            }
            patch.Patch(entity);
            _context.SaveChanges();
            return entity;
        }

        protected override int GetKey(Contact entity)
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
