using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Demo
{
    public interface IPersonDataAccess
    {
        PersonModel Get(Guid id);
        IEnumerable<PersonModel> GetAll();
        PersonModel Save(PersonModel p);
        PersonModel Update(Guid id, PersonModel model);
    }

    public class PersonDataAccess : IPersonDataAccess
    {
        private static IDictionary<Guid, PersonModel> _cache = new ConcurrentDictionary<Guid, PersonModel>();

        public PersonModel Save(PersonModel p) => _cache[p.Id] = p;

        public PersonModel Get(Guid id)
        {
            PersonModel model;
            if (_cache.TryGetValue(id, out model)) return model;
            return null;
        }

        public IEnumerable<PersonModel> GetAll() => _cache.Select(x => x.Value);

        public PersonModel Update(Guid id, PersonModel model)
        {
            PersonModel existingModel;
            if (_cache.TryGetValue(id, out existingModel))
            {
                existingModel.Update(model);
                return existingModel;
            }
            return null;
        }
    }

    public class PersonModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Age { get; set; }

        public void Update(PersonModel model)
        {
            Name = model.Name;
            Age = model.Age;
        }
    }
}
