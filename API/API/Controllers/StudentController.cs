﻿using API.Models;
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
    public class StudentController : ControllerBase
    {
        // GET: api/student
        [HttpGet]
        public string Get()
        {
            return "Error when using student controller, no username/password provided";
        }

        // GET api/<ValuesController>/studentid/password
        [HttpGet("{id}/{password}")]
        public Student Get(string id, string password)
        {
            // Connection String
            var connectionString = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var con = new NpgsqlConnection(connectionString);
            // Connect to the DB
            con.Open();

            string sql = $"SELECT * FROM students WHERE studentno = '{id}' AND password = crypt('{password}', password)";
            using var sqlCommand = new NpgsqlCommand(sql, con);
            using NpgsqlDataReader reader = sqlCommand.ExecuteReader();
            reader.Read();
            Student returnObj = new Student();
            // If returns a value, add the names from that row to the model
            try
            {
                returnObj.name = $"{reader.GetString(2)} {reader.GetString(3)}";
                returnObj.stundentID = id;
                reader.Close();
            }
            catch
            {
                returnObj.name = "Error";
                returnObj.stundentID = "Error";
                return returnObj;
            }


            // Build a list of the classes
            List<string> classList = new List<string>();
            sqlCommand.CommandText = $"SELECT * FROM classlist ";
            sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                classList.Add(reader.GetString(0));
            }
            reader.Close();
            // Iterate through the classes to find which the student is enrolled in
            List<string> matches = new List<string>();
            foreach (string curr in classList)
            {
                sqlCommand.CommandText = $"SELECT * FROM {curr} WHERE studentno = '{id}'";
                sqlCommand.ExecuteReader();
                reader.Read();
                try
                { // If the GetString function does not return an exception, a row has been found.
                    reader.GetString(0);
                    matches.Add(curr);
                }
                catch
                {
                    reader.Close();
                    continue;
                }
                reader.Close();
            }
            returnObj.enrolledClasses = matches.ToArray();
            return returnObj;
            
        }

}
}
