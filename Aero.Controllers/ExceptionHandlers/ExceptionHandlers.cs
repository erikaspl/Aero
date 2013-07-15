using Microsoft.Data.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Aero.Controllers
{
    public class ExceptionHandlers
    {
        public static void EntityValidationException(DbEntityValidationException ex, HttpRequestMessage request)
        {
            throw new HttpResponseException(
                request.CreateODataErrorResponse(
                HttpStatusCode.InternalServerError,
                new ODataError
                {
                    ErrorCode = "EntityValidationException",
                    Message = GetValidationErrorMessage(ex)
                }));
        }

        private static string GetValidationErrorMessage(DbEntityValidationException ex)
        {
            string message = string.Empty;
            foreach (var entityError in ex.EntityValidationErrors)
            {
                foreach (var validationError in entityError.ValidationErrors)
                {
                    message += validationError.ErrorMessage;
                }
            }
            return message;
        }
    }
}