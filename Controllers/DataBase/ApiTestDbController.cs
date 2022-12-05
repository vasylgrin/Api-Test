using Api_Test.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Api_Test.Controllers.DataBase
{
    public class ApiTestDbController<T> : DbContext where T : class
    {
        ApiTestContext testContext;
        DbSet<T> dbSet;

        public ApiTestDbController()
        {
            testContext = new ApiTestContext();
            dbSet = testContext.Set<T>();
        }

        public void Save(List<T> entity)
        {
            dbSet.AddRange(entity);
            testContext.SaveChanges();
        }

        public List<T> Load()
        {
            return dbSet.AsNoTracking().ToList();
        }

        public List<Person> LoadPersonInclude()
        {
            using (ApiTestContext apiTestContext = new())
            {
                return apiTestContext.Persons.Include(p => p.Origin).ToList();
            }
        }
    }
}
