using API.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        
        // GET api/<ValuesController>
        [HttpGet]
        public Location Get()
        {
            string locationID = Request.Headers["locationid"];
            // Connection String
            var connectionString = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var connection = new NpgsqlConnection(connectionString);
            // Connect to the DB
            connection.Open();

            string sql = $"SELECT * FROM locations WHERE locationid = '{locationID}'";
            using var sqlCommand = new NpgsqlCommand(sql, connection);
            using NpgsqlDataReader reader = sqlCommand.ExecuteReader();
            reader.Read();
            Location returnObj = new Location();
            // If returns a value, add the names from that row to the model
            try
            {
                returnObj.locationName = reader.GetString(1);
                returnObj.locationIP = reader.GetString(2);
                reader.Close();
            }
            catch
            {
                returnObj.locationName = "Error";
                returnObj.locationIP = "Error";
                return returnObj;
            }
            return returnObj;
            
        }

}
}
