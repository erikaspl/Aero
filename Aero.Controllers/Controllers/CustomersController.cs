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
    public class CustomersController : EntitySetController<Customer, int>
    {
        private AeroContainer _context = new AeroContainer();

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Customer> Get()
        {
            return _context.Customers;
        }

        [Queryable]
        protected override Customer GetEntityByKey(int key)
        {
            return _context.Customers.FirstOrDefault(a => a.Id == key);
        }

        protected override Customer CreateEntity(Customer entity)
        {
            try
            {
                _context.Customers.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionHandlers.EntityValidationException(ex, Request);
            }
            return entity;
        }

        protected override Customer UpdateEntity(int key, Customer update)
        {
            if (!_context.Customers.Any(a => a.Id == key))
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = string.Format("Customer key {0} not found", key)
                    }));
            }

            update.Id = key;

            _context.Customers.Attach(update);
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
            var entity = _context.Customers.FirstOrDefault(a => a.Id == key);
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
            _context.Customers.Remove(entity);
            _context.SaveChanges();
        }

        protected override Customer PatchEntity(int key, Delta<Customer> patch)
        {
            var entity = _context.Customers.FirstOrDefault(a => a.Id == key);

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

        protected override int GetKey(Customer entity)
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
