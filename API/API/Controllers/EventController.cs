using API.Models;
using Microsoft.AspNetCore.Http;
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
    public class EventController : ControllerBase
    {


        // GET api/event
        // Get next event and current
        [HttpGet]
        public List<Event> Get()
        {

            string classes = Request.Headers["classes"];
            // Connection String
            var conString = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var con = new NpgsqlConnection(conString);
            // Connect to the DB
            con.Open();
            // Will use index 0 as closest event, 1 for if there is a current event to return the next event
            List<Event> Events = new List<Event>();
            string sql = $"SELECT * FROM events WHERE class in ({classes}) AND (DATE_PART('minute', datetime - LOCALTIMESTAMP)) <= 30" +
                " AND (DATE_PART('day', datetime - LOCALTIMESTAMP)) = 0 AND (LOCALTIMESTAMP > datetime)";
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            try
            {
                Event newEvent = new Event { EventID = reader.GetString(0), LocationID = reader.GetString(1), EventName = reader.GetString(2) };
                Events.Add(newEvent);
            }
            // Exception is thrown if there is nothing to read
            catch { }
            reader.Close();
            // Get the next event
            cmd.CommandText = $"SELECT* FROM events WHERE class in ({classes}) AND datetime > LOCALTIMESTAMP ORDER BY datetime ASC";
            cmd.ExecuteReader();
            reader.Read();
            try
            {
                Event newEvent = new Event { EventID = reader.GetString(0), LocationID = reader.GetString(1), EventName = reader.GetString(2) };
                Events.Add(newEvent);
            }
            // Exception is thrown if there is nothing to read
            catch { }
            return Events;
        }

        // PATCH api/<ValuesController>/id
        [HttpPatch("{eventid}")]
        public void Patch(string eventid)
        {
            string studentID = Request.Headers["studentID"];

            // Connection String
            var conString = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var con = new NpgsqlConnection(conString);

            string sql = $"UPDATE {eventid} set attended = 'a' WHERE studentno = '{studentID}' ";

            using var cmd = new NpgsqlCommand(sql, con);
        }

    }
}
