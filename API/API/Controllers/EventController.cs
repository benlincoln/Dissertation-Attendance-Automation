using API.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;

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
            string conString = Program.getConString();
            

            using var con = new NpgsqlConnection(conString);
            // Connect to the DB
            con.Open();
            // Will use index 0 as closest event, 1 for if there is a current event to return the next event
            List<Event> Events = new List<Event>();
            string sql = $"SELECT * FROM events WHERE class in ({classes}) AND (DATE_PART('minute', datetime - LOCALTIMESTAMP)) <= 30" +
                " AND (DATE_PART('day', datetime - LOCALTIMESTAMP)) = 0 AND (DATE_PART('hour', datetime - LOCALTIMESTAMP)) = 0";
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            try
            {
                DateTime time = (DateTime)reader.GetTimeStamp(4);
                Event newEvent = new Event { EventID = reader.GetString(0), LocationID = reader.GetString(1), EventName = reader.GetString(2), Time = time.ToString(), Current = true };
                Events.Add(newEvent);
            }
            // Exception is thrown if there is nothing to read
            catch { }
            reader.Close();
            // Get the next event
            cmd.CommandText = $"SELECT * FROM events WHERE class in ({classes}) AND datetime > LOCALTIMESTAMP ORDER BY datetime ASC";
            cmd.ExecuteReader();
            reader.Read();
            try
            {
                DateTime time = (DateTime)reader.GetTimeStamp(4);
                Event newEvent = new Event { EventID = reader.GetString(0), LocationID = reader.GetString(1), EventName = reader.GetString(2), Time = time.ToString(), Current = false };
                Events.Add(newEvent);
            }
            // Exception is thrown if there is nothing to read
            catch { }
            return Events;
        }

        // PATCH api/<ValuesController>
        // Used to mark students present
        [HttpPatch]
        public Event Patch()
        {
            string studentID = Request.Headers["studentID"];
            string eventid = "event"+Request.Headers["eventID"];
            // Connection String
            var conString = Program.getConString();

            using var con = new NpgsqlConnection(conString);
            con.Open();
            // Gets the event's table and marks the student present
            string sql = $"UPDATE {eventid} set attended = 'a' WHERE studentno = '{studentID}' ";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            return new Event { EventID = eventid };
        }

        [HttpPatch]
        [Route("api/event/update")]
        public void MarkAbscent()
        {
            var conString = Program.getConString();

            using var con = new NpgsqlConnection(conString);
            con.Open();
            // Gets all events in the past day
            string sql = $"SELECT eventid FROM events WHERE AND (DATE_PART('day', datetime - LOCALTIMESTAMP)) = 0";
            using var cmd = new NpgsqlCommand(sql, con);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            List<string> eventList = new List<string>();
            while (reader.Read())
            {
                eventList.Add(reader.GetString(0));
            }
            foreach(string currEvent in eventList)
            {
                string currEventTbl = $"event{currEvent}";
                cmd.CommandText = $"UPDATE {currEventTbl} set attended = 'u' WHERE attended = 'null' ";
                cmd.ExecuteNonQuery();
            }

        }

    }
}
