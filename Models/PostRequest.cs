using Microsoft.AspNetCore.Mvc;

namespace Api_Test.Models
{
    public class PostRequest
    {
        public int Id { get; set; }
        public string personName { get; set; }
        public string episodeName { get; set; }
        public string Response { get; set; }
    }
}
