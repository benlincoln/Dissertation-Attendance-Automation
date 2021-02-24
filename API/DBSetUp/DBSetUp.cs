using System;
using Npgsql;

namespace Version
{
    class DBSetUp
    {
        static void Main(string[] args)
        {
            // Connection String
            var cs = "Host=localhost;User ID=postgres;Password=password;Database=postgres;Port=5433";

            using var con = new NpgsqlConnection(cs);
            // Connect to the DB
            con.Open();

            // SQL Command to get ther version
            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);
            // Execute the SQL and print its output
            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"Successfully connected to the database. PostgreSQL version: {version}");

            // Table reset
            cmd.CommandText = "DROP TABLE IF EXISTS students;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE students (studentno VARCHAR(8) PRIMARY KEY, password VARCHAR(255), forename VARCHAR(20), surname VARCHAR(20));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS locations;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE locations (locationid SERIAL PRIMARY KEY, locationname VARCHAR(100), locationip VARCHAR(20));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS events;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE events (eventid SERIAL PRIMARY KEY, locationid INT, eventname VARCHAR(100), class VARCHAR(100));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS event1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE event1 (studentno VARCHAR(8),attended char(1));";
            Console.WriteLine("All tables created successfully");

            // Insert Values
            cmd.CommandText = "INSERT INTO students (studentno, password, forename, surname)VALUES('12345678',crypt('password1', gen_salt('bf')), 'first', 'firstsur')" +
                "('22345678',crypt('password2', gen_salt('bf')), 'second', 'secondsur')";



            cmd.ExecuteNonQuery();
        }
    }
}