using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VersionUsingUrl.Models.v3;

namespace VersionUsingUrl.Controllers.v3
{    
    [Route("api/v{version:apiVersion}/[controller]")]    
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { new Dog().ToString(), new Dog().ToString() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return new Dog().ToString();
        }


    }
}
