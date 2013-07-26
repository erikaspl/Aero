using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Aero.EF;
using Aero.Model;
using Breeze.WebApi;

namespace Aero.Angular.Models
{
    public class AeroRepository : EFContextProvider<AeroContainer>
    {
        public AeroRepository(IPrincipal user)
        {
            UserId = user.Identity.Name;
        }

        public string UserId { get; private set; }

        public DbQuery<Part> Parts
        {
            get
            {
                return (DbQuery<Part>)Context.Parts;
            }
        }

        public DbQuery<RFQ> RFQs
        {
            get
            {
                return (DbQuery<RFQ>)Context.RFQs
                    .Where(t => t.Customer.UserName == UserId);
            }
        }

        public DbQuery<Priority> Priorities
        {
            get
            {
                return (DbQuery<Priority>)Context.Priorities;
            }

        }
    }
}