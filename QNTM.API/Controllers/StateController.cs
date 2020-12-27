using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QNTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "six", "nine" };
        }

        [HttpGet("{id}")]
        public ActionResult<int> Get(int id)
        {
            return id;
        }


        //notes:
            /*
            Filename is the hash(MD5 or something) of the name + datetime
            
            do not accept strangely unicode file names/extentions 
            regular expression: [a-zA-Z0-9]{1,200}\.[a-zA-Z0-9]{1,10}

            min size: ???? max size: 5mb...this is only POC afterall

            file type whitelist

            integrate virus total api to check files?

            logs for safety? or keep the wild west vibes of no logs ever

            contents will be encrypted blobs by the time they are seen,
                some of these point may be worth ignoring?
            */

    }
}