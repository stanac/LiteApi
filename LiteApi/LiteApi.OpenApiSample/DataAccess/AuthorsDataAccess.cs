using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteApi.OpenApiSample.Models;

namespace LiteApi.OpenApiSample.DataAccess
{
    public class AuthorsDataAccess : IAuthorsDataAccess
    {
        private static readonly List<Author> _authors;

        static AuthorsDataAccess()
        {
            _authors = new List<Author>();
            _authors.AddRange(SampleData.Authors);
        }

        public Author Add(Author model)
        {
            _authors.Add(model);
            model.Id = Guid.NewGuid();
            return model;
        }

        public bool Delete(Guid id)
        {
            var toRemove = _authors.FirstOrDefault(x => x.Id == id);
            if (toRemove != null)
            {
                _authors.Remove(toRemove);
                return true;
            }
            return false;
        }

        public Author Get(Guid id)
        {
            return _authors.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Author> GetAll()
        {
            return _authors;
        }

        public Author Update(Guid id, Author model)
        {
            model.Id = id;
            var toUpdate = _authors.FirstOrDefault(x => x.Id == id);
            if (toUpdate != null)
            {
                toUpdate.BirthYear = model.BirthYear;
                toUpdate.Name = model.Name;
                return toUpdate;
            }
            return null;
        }
    }
}
