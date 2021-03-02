using API.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        // GET: api/event
        [HttpGet]
        public Event Get()
        {
            return new Event { EventID = "00000", EventName = "PlaceholderEvent", Room = "PlaceholderRoom", Class = "Placeholder Class", EventTime = DateTime.Now };
        }

        // GET api/event/id
        // Get next event
        [HttpGet("{studentid}")]
        public string Get(string studentid)
        {
            // Connection String
            var cs = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var con = new NpgsqlConnection(cs);
            // Connect to the DB
            con.Open();
            
            string sql = "SELECT * FROM events";
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();


            return $"{studentid}";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
