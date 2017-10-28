using LiteApi.OpenApiSample.Models;
using System;
using System.Collections.Generic;

namespace LiteApi.OpenApiSample.DataAccess
{
    public interface IAuthorsDataAccess
    {
        IEnumerable<Author> GetAll();
        Author Get(Guid id);
        Author Add(Author model);
        bool Delete(Guid id);
        Author Update(Guid id, Author model);
    }
}
