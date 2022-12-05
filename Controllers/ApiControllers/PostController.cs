using Api_Test.Controllers.DataBase;
using Api_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api_Test.Controllers.ApiControllers
{
    public class PostController
    {
        List<PostRequest> Requests = new();
        PostRequest postRequest;

        /// <summary>
        /// Checks if the character was in the specified episode.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>true - if there was, false - if there was not, NotFound(404) - if the character or episode was not found.</returns>
        public IActionResult CheckIfExistPersonInTheEpisode(PostRequest request)
        {
            postRequest = GetPostRequestFromDB(request);
            if(postRequest == null)
            {
                var persons = GetPerson(request.personName);
                var episodes = GetEpisode(request.episodeName);

                if (persons.Count != 0 && episodes.Any())
                {
                    foreach (var person in persons)
                    {
                        foreach (var episode in episodes)
                        {
                            if (person["url"].ToString() == episode.ToString())
                            {
                                request.Response = "true";

                                SaveRequest(request);

                                return new ObjectResult(true);
                            }
                        }
                    }

                    request.Response = "false";
                    SaveRequest(request);
                    return new ObjectResult(false);
                }
                else
                {
                    request.Response = "404";
                    return new NotFoundResult();
                }
            }
            else
                return GetValueFromResponse(postRequest);
        }

        /// <summary>
        /// Determines whether the same search has already been performed.
        /// </summary>
        /// <param name="postRequest">A new request that will compare.</param>
        /// <returns>Existing request - if there was already one, null - if there was none</returns>
        private PostRequest GetPostRequestFromDB(PostRequest postRequest) => new ApiTestDbController<PostRequest>().Load().Where(req => req.episodeName == postRequest.episodeName && req.personName == postRequest.personName).FirstOrDefault();

        /// <summary>
        /// Gets all the characters that can be found by name.
        /// </summary>
        /// <param name="personName">Person name.</param>
        /// <returns>Сharacter list.</returns>
        private List<JToken> GetPerson(string personName)
        {
            var person = JsonController.GetRequest($"https://rickandmortyapi.com/api/character/?name={personName}")["results"];
            List<JToken> persons = new();

            if (person != null)
            {
                for (int count = 0; count <= person.Count() - 1; count++)
                {
                    if (person[count]["name"].ToString() == personName)
                    {
                        persons.Add(person[count]);
                    }
                }
            }

            return persons;
        }

        /// <summary>
        /// Retrieves all episodes for further action.
        /// </summary>
        /// <param name="episodeName">Episode name.</param>
        /// <returns>All episodes that exist.</returns>
        private JToken GetEpisode(string episodeName)
        {
            var episode = JsonController.GetRequest($"https://rickandmortyapi.com/api/episode")["results"];

            for (int count = 0; count < episode.Count() - 1; count++)
            {
                if (episode[count]["name"].ToString() == episodeName)
                {
                    return episode[count]["characters"];
                }
            }

            return new JObject();
        }

        /// <summary>
        /// Gets an instance of PostRequest, adds it to the list and stores it in the database.
        /// </summary>
        /// <param name="request">Request</param>
        private void SaveRequest(PostRequest request)
        {
            Requests.Clear();
            Requests.Add(request);
            new ApiTestDbController<PostRequest>().Save(Requests);
        }

        /// <summary>
        /// Identifies the response type.
        /// </summary>
        /// <param name="postRequest">Request.</param>
        /// <returns>true, false, 404, EmptyResults</returns>
        public static IActionResult GetValueFromResponse(PostRequest postRequest)
        {
            if (postRequest.Response == "true")
                return new ObjectResult(true);
            else if (postRequest.Response == "false")
                return new ObjectResult(false);
            else if (postRequest.Response == "404")
                return new NotFoundResult();
            else
                return new EmptyResult();
        }
    }
}
