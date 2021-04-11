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
            // Drop dependency tables first
            cmd.CommandText = "DROP TABLE IF EXISTS class1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS class2;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS event1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS event0;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS event2;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS event3;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS events;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS students;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS classlist;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE students (studentno VARCHAR(8) PRIMARY KEY, password VARCHAR(255), forename VARCHAR(255), surname VARCHAR(255));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS locations;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE locations (locationid VARCHAR(12) PRIMARY KEY, locationname VARCHAR(100), locationip VARCHAR(20));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE events (eventid VARCHAR(12) PRIMARY KEY, locationid VARCHAR(12), eventname VARCHAR(100), class VARCHAR(100), datetime TIMESTAMP);";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE event1 (studentno VARCHAR(8),attended char(1));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE event0 (studentno VARCHAR(8),attended char(1));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE event2 (studentno VARCHAR(8),attended char(1));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE event3 (studentno VARCHAR(8),attended char(1));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE class1 (studentno VARCHAR(8));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE class2 (studentno VARCHAR(8));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE classlist (classes VARCHAR(255));";
            cmd.ExecuteNonQuery();
            Console.WriteLine("All tables created successfully");

            // Assign Foreign Keys
            cmd.CommandText = ("ALTER TABLE class1 ADD CONSTRAINT stuno FOREIGN KEY (studentno) REFERENCES students(studentno);");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ("ALTER TABLE event0 ADD CONSTRAINT stuno FOREIGN KEY (studentno) REFERENCES students(studentno);");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ("ALTER TABLE event1 ADD CONSTRAINT stuno FOREIGN KEY (studentno) REFERENCES students(studentno);");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ("ALTER TABLE event2 ADD CONSTRAINT stuno FOREIGN KEY (studentno) REFERENCES students(studentno);");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ("ALTER TABLE event3 ADD CONSTRAINT stuno FOREIGN KEY (studentno) REFERENCES students(studentno);");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ("ALTER TABLE events ADD CONSTRAINT locid FOREIGN KEY (locationid) REFERENCES locations(locationid);");
            cmd.ExecuteNonQuery();
            Console.WriteLine("F Keys added successfully");

            // Insert Values
            // Students
            cmd.CommandText = "INSERT INTO students (studentno, password, forename, surname)VALUES('12345678',crypt('password1', gen_salt('bf')), 'first', 'firstsur')," +
                "('22345678',crypt('password2', gen_salt('bf')), 'second', 'secondsur')," +
                "('23345678',crypt('password3', gen_salt('bf')), 'third', 'thirdsur')," +
                "('21445678',crypt('password4', gen_salt('bf')), 'fourth', 'fourthsur')," +
                "('22352478',crypt('password5', gen_salt('bf')), 'fifth', 'fifthsur')," +
                "('24245678',crypt('password6', gen_salt('bf')), 'sixth', 'sixthsur')," +
                "('12385678',crypt('password7', gen_salt('bf')), 'seventh', 'seventhsur')," +
                "('22345879',crypt('password8', gen_salt('bf')), 'eighth', 'eighthsur')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Populated students");

            // Locations
            cmd.CommandText = "INSERT INTO locations (locationid, locationname, locationip) VALUES('1','real','192.168.1.1')," +
                "('2','fake','192.168.1.2')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Populated locations");

            // Classes
            cmd.CommandText = "INSERT INTO class1 (studentno) VALUES('12345678'),('21445678'),('12385678')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO class2 (studentno) VALUES('22345678'),('22352478'),('22345879')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO classlist (classes) VALUES('class1'),('class2')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Populated class tables");

            // Events
            cmd.CommandText = "INSERT INTO events (eventid, locationid, eventname, class,datetime) VALUES('1','1','eventNow','class1',LOCALTIMESTAMP(0))," +
                "('0','1','pastAttended','class1',LOCALTIMESTAMP(0) - interval '1 day'),('2','1','future','class1',LOCALTIMESTAMP(0) + interval '1 day'),('3','2','test','class2',LOCALTIMESTAMP(0) + interval '1 hour')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO event1 (studentno) SELECT studentno FROM class1";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO event0 (studentno) SELECT studentno FROM class1";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE event0 SET attended = 'a'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO event2 (studentno) SELECT studentno FROM class1";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO event3 (studentno) SELECT studentno FROM class2";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Populated event tables");

            Console.WriteLine("Set up complete! You may now close this application.");
        }
    }
}