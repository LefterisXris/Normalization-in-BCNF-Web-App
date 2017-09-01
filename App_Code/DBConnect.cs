using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Normalization
{
    /// <summary>
    /// Summary description for DBConnect
    /// </summary>
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DBConnect()
        {
            //Initialize();
            MySqlDbInit();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "lefterisxris";
            uid = "lefterisxris";
            password = "";
            string connectionString;
            connectionString = "Server=" + server + ";" + "Port=3306;" + "Database=" + database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";" + "Protocol=memory;" + "Shared Memory Name=MYSQL;";

            connection = new MySqlConnection(connectionString);
        }

        private void MySqlDbInit()
        {
            server = "195.251.211.81"; 
            database = "lefterisxris";
            uid = "lefterisxris";
            password = "";

            string connectionString = "Server=" + server + ";" +"Port=3306;" + "Database=" + database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        //  MessageBox.Show("Cannot connect to server.  Contact administrator") ;
                        Console.WriteLine("Cannot connect to server.  Contact administrator: " + ex.Message);
                        break;

                    case 1045:
                        // MessageBox.Show("Invalid username/password, please try again");
                        Console.WriteLine("Invalid username/password, please try again: " + ex.Message);
                        break;
                    case 1042:
                        Console.WriteLine("Error " + ex.Number + " has occured: " + ex.Message);
                        break;

                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
             //   MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Select statement
        public List<string> Select(string query)
        {
            // string query = "SELECT `name` FROM `Schemas` WHERE id>10";
            
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                 /* while (dataReader.Read())
                  {
                      list[0].Add(dataReader["id"] + "");
                      list[1].Add(dataReader["name"] + "");
                  }*/
                List<string> yo = new List<string>();
                while (dataReader.Read())
                {
                    //yo.Add(dataReader["id"] + "");
                    yo.Add(dataReader["name"] + "");
                }
                    
                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return yo;
            }
            else
            {
                List<string> yo = new List<string>();
                yo.Add("Fail");
                return yo;
            }
        }

        /// <summary>
        /// Μέθοδος που ενημερώνει το αντίστοιχο πεδίο στη βάση για αποθήκευση των Στατιστικών.
        /// </summary>
        /// <param name="schemaName">Το όνομα του σχήματος</param>
        /// <param name="action">Η ενέργεια (φόρτωση, διάσπαση, εγλεισμός κλπ)</param>
        public void TrackStat(string schemaName, string action)
        {
            string query = "UPDATE `Schemas` SET `"+ action +"`= `"+ action +"` + 1 WHERE `name`='"+ schemaName +"'";
            //"UPDATE `Schemas` SET `nLoad`= `nLoad` + 1 WHERE `name`='Default.txt'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();

                //Assign the query using CommandText
                cmd.CommandText = query;

                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                // Ενημέρωση ημερομηνίας.
                lastEditSet(schemaName);

                //close connection
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Μέθοδος που θέτει την τρέχουσα ημερομηνία στο πεδίο lastEdit του αντίστοιχου σχήματος.
        /// </summary>
        /// <param name="schemaName">Το όνομα του σχήματος.</param>
        private void lastEditSet(string schemaName)
        {
            string query = "UPDATE `Schemas` SET `lastEdit`= now() WHERE `name`='" + schemaName + "'";
            //UPDATE `Schemas` SET `lastEdit`=now() WHERE 1

            //create mysql command
            MySqlCommand cmd = new MySqlCommand();
            //Assign the query using CommandText
            cmd.CommandText = query;
            //Assign the connection using Connection
            cmd.Connection = connection;

            //Execute query
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Μέθοδος η οποία παίρνει τα ονόματα των διαθέσιμων σχημάτων.
        /// </summary>
        /// <returns>Λίστα με τα ονόματα αυτά.</returns>
        public List<string> getSchemaNames()
        {
            string query = "SELECT `name` FROM `Schemas` WHERE `createdBy` = 'admin'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                List<string> names = new List<string>();
                while (dataReader.Read())
                {
                    names.Add(dataReader["name"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return names;
            }
            return new List<string>(); 
            
        }

        /// <summary>
        /// Αποθηκεύει το όνομα του νέου σχήματος στην ΒΔ.
        /// </summary>
        /// <param name="schemaName">Όνομα νέου σχήματος</param>
        public void saveNewSchemaOnDB(string schemaName)
        {
            string query = "INSERT INTO `Schemas`(`name`, `nLoad`, `nClosure`, `nFindKeys`, `nDecompose`, `nStepsDecompose`, `dateCreated`, `lastEdit`) VALUES('"+ schemaName +"', 1, 0, 0, 0, 0, now(), now() )";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }



        /* SELECT
        //Select statement
        public List<string> TrackStat(string query)
        {
           // string query = "SELECT `name` FROM `Schemas` WHERE id>10";

            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                  while (dataReader.Read())
                  {
                      list[0].Add(dataReader["id"] + "");
                      list[1].Add(dataReader["name"] + "");
                  }
        List<string> yo = new List<string>();
                while (dataReader.Read())
                {
                    //yo.Add(dataReader["id"] + "");
                    yo.Add(dataReader["name"] + "");
                }

    //close Data Reader
    dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return yo;
    }
            else
            {
                List<string> yo = new List<string>();
    yo.Add("Fail");
                return yo;
            }
        } 

          */


    }
}