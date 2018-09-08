using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VersionUsingUrl.Models.v2;

namespace VersionUsingUrl.Controllers.v2
{    
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]    // leave this if default version route is set to v2 in startup otherwise will not work
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
