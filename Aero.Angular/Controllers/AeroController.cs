namespace Aero.Angular.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Aero.Model;
    using Breeze.WebApi;
    using Filters;
    using Models;
    using Newtonsoft.Json.Linq;

    [Authorize]
    [BreezeController]
    public class AeroController : ApiController
    {
        private readonly AeroRepository _repository;

        public AeroController()
        {
            _repository = new AeroRepository(User);
        }

        // GET ~/api/Aero/Metadata 
        [HttpGet]
        public string Metadata()
        {
            return _repository.Metadata();
        }

        // POST ~/api/Aero/SaveChanges
        [HttpPost]
        [ValidateHttpAntiForgeryToken]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _repository.SaveChanges(saveBundle);
        }

        // GET ~/api/Aero/RFQs
        [HttpGet]
        public IQueryable<RFQ> RFQs()
        {
            return _repository.RFQs;
            // We do the following on the client
            //.Include("Todos")
            //.OrderByDescending(t => t.TodoListId);
        }

        [HttpGet]
        public IQueryable<Part> Parts()
        {
            return _repository.Parts;
        }

        [HttpGet]
        public IQueryable<Priority> Priorities()
        {
            return _repository.Priorities;
        }
    }
}