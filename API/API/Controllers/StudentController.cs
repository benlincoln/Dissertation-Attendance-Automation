using API.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        
        // GET api/<ValuesController>
        [HttpGet]
        public Student Get()
        {
            string id = Request.Headers["username"];
            string password = Request.Headers["password"];
            
            // Connection String
            var connectionString = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var connection = new NpgsqlConnection(connectionString);
            // Connect to the DB
            connection.Open();

            string sql = $"PREPARE login (text, text) AS SELECT * FROM students WHERE studentno = $1 " +
                $"AND password = crypt($2, password); EXECUTE login ('{id}','{password}');";
            using var sqlCommand = new NpgsqlCommand(sql, connection);
            using NpgsqlDataReader reader = sqlCommand.ExecuteReader();
            reader.Read();
            Student returnObj = new Student();
            // If returns a value, add the names from that row to the model
            try
            {
                returnObj.name = $"{reader.GetString(2)} {reader.GetString(3)}";
                returnObj.studentID = id;
                reader.Close();
            }
            catch
            {
                returnObj.name = "Error";
                returnObj.studentID = "Error";
                return returnObj;
            }


            // Build a list of the classes
            List<string> classList = new List<string>();
            sqlCommand.CommandText = $"SELECT * FROM classlist";
            sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                classList.Add(reader.GetString(0));
            }
            reader.Close();
            // Iterate through the classes to find which the student is enrolled in
            string matches = null;
            foreach (string curr in classList)
            {
                sqlCommand.CommandText = $"SELECT * FROM {curr} WHERE studentno = '{id}'";
                sqlCommand.ExecuteReader();
                reader.Read();
                try
                { // If the GetString function does not return an exception, a row has been found.
                    reader.GetString(0);
                    if (matches != null)
                    {
                        matches += (",");
                    }
                    matches += ("\'"+curr+"\'");
                }
                catch
                {
                    reader.Close();
                    continue;
                }
                reader.Close();
            }
            returnObj.enrolledClasses = matches;

            // Get the attendance
            // Gets past events in the enrolled classes
            sqlCommand.CommandText = $"SELECT eventid FROM events WHERE class in ({returnObj.enrolledClasses}) AND datetime < LOCALTIMESTAMP";
            sqlCommand.ExecuteReader();
            List<string> pastEvents = new List<string>();
            while (reader.Read())
            {
                pastEvents.Add(reader.GetString(0));
            }
            if (pastEvents.Count == 0)
            {
                returnObj.attendance = "null";
                return returnObj;
            }
            reader.Close();
            int abscent = 0;
            int total = 0;
            foreach (string currID in pastEvents) {
                // The below stops events that have just started being marked as not present
                sqlCommand.CommandText = $"SELECT studentno from {"event" + currID} where attended = 'u'";
                sqlCommand.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    abscent++;
                }
                total++;
                reader.Close();
                }
            int attendance;
            try
            {
                attendance = 100 - ((abscent / total) * 100);
            }
            catch (DivideByZeroException)
            {
                attendance = 100;
            }
            returnObj.attendance = attendance.ToString();
            return returnObj;
            
        }

}
}
