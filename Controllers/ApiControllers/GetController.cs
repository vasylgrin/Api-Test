using Api_Test.Controllers.DataBase;
using Api_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api_Test.Controllers.ApiControllers
{
    public class GetController
    {
        List<Person> Persons = new();

        public IActionResult GetPerson(string name)
        {
            if (!GetPersonByNameFromDB(name, out Persons))
            {
                if (!GetPersonByNameFromApiSite(name, out Persons))
                    return new NotFoundResult();
                else
                    new ApiTestDbController<Person>().Save(Persons);
            }
            return new ObjectResult(Persons);
        }

        private bool GetPersonByNameFromApiSite(string name, out List<Person> persons)
        {
            persons = new List<Person>();

            var person = JsonController.GetRequest($"https://rickandmortyapi.com/api/character/?name={name}")["results"] ?? new JObject();

            if (person.Any())
            {
                for (int count = 1; count < person.Count() - 1; count++)
                {
                    var per = new Person()
                    {
                        Id = Convert.ToInt32(person[count]["Id"]),
                        Name = person[count]["name"].ToString(),
                        Status = person[count]["status"].ToString(),
                        Species = person[count]["species"].ToString(),
                        Type = person[count]["type"].ToString(),
                    };

                    var temp = person[count]["location"];

                    if (!string.IsNullOrWhiteSpace(temp["url"].ToString()))
                    {
                        var location = JsonController.GetRequest(temp["url"].ToString());

                        per.Origin.Name = location["name"].ToString();
                        per.Origin.Type = location["type"].ToString();
                        per.Origin.Dimension = location["dimension"].ToString();
                        persons.Add(per);
                    }
                    else
                    {
                        per.Origin.Name = "unknown";
                        per.Origin.Type = "unknown";
                        per.Origin.Dimension = "unknown";
                        persons.Add(per);
                    }
                }

                return true;
            }
            else
                return false;
        }

        private bool GetPersonByNameFromDB(string name, out List<Person> persons)
        {

            persons = new ApiTestDbController<Person>().Load().Where(person => person.Name == name).ToList();
            if (persons.Any())
                return true;
            else
                return false;
        }
    }
}
