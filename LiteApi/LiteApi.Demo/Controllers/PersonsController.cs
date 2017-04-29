using LiteApi.Attributes;
using System;
using System.Collections.Generic;

namespace LiteApi.Demo.Controllers
{
    [Restful]
    public class PersonsController: LiteController
    {
        private readonly IPersonDataAccess _dataAccess;

        public PersonsController(IPersonDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpGet] // will respond to /api/persons?id={someGuid}
        public PersonModel ById(Guid id) => _dataAccess.Get(id);

        [HttpGet, ActionRoute("/{id}")] // will respond to /api/persons/{someGuid}
        public PersonModel ByIdFromRoute([FromRoute]Guid id) => _dataAccess.Get(id);

        [HttpGet] // will respond to /api/persons
        public IEnumerable<PersonModel> All() => _dataAccess.GetAll();

        [HttpPost] // will respond to /api/persons
        public PersonModel Save(PersonModel model) => _dataAccess.Save(model);

        [HttpPost, ActionRoute("/{id}")] // will respond to /api/persons/{someGuid}
        public PersonModel Update(Guid id, PersonModel model) => _dataAccess.Update(id, model);
    }
}
