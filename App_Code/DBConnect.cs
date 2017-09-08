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

            string connectionString = "Server=" + server + ";" +"Port=3306;" + "Database=" + database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";" + "CharSet = greek;" ;
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
        public void TrackStat(string schemaName, string schemaId, string action)
        {
            int id = Int32.Parse(schemaId);

            string query = "UPDATE `Schemas` SET `"+ action +"`= `"+ action +"` + 1 WHERE `name`='"+ schemaName +"' AND `id`="+ id +"";
          
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
                lastEditSet(schemaId);

                //close connection
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Μέθοδος που θέτει την τρέχουσα ημερομηνία στο πεδίο lastEdit του αντίστοιχου σχήματος.
        /// </summary>
        /// <param name="schemaName">Το όνομα του σχήματος.</param>
        private void lastEditSet(string schemaId)
        {
            int id = Int32.Parse(schemaId);
            string query = "UPDATE `Schemas` SET `lastEdit`= now() WHERE `id`=" + id + "";
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

        /// <summary>
        /// Μέθοδος που παίρνει το όνομα του προεπιλεγμένου προς φόρτωση σχήματος.
        /// </summary>
        /// <returns>Το όνομα του προεπιλεγμένου σχήματος</returns>
        public string getDefaultSchemaName()
        {
            string query = "SELECT `name` FROM `Schemas` WHERE `isDefault`= true";

            string name = "";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    name = dataReader["name"] + "";
                }
                
                //close connection
                this.CloseConnection();
                return name;
            }
            return name;
        }

        /// <summary>
        /// Μέθοδος που παίρνει το id ενός σχήματος από τη ΒΔ.
        /// </summary>
        public int getSchemaId(string schemaName)
        {
            string query = "SELECT * FROM `Schemas` WHERE `name`='"+ schemaName  +"' ORDER BY `Schemas`.`dateCreated` DESC";
            int id = 0;
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                dataReader.Read();
                id = (int) dataReader["id"];
                

                //close connection
                this.CloseConnection();
                return id;
            }
            return id;

        }

        /// <summary>
        /// Μέθοδος που ενημερώνει ένα σχήμα ότι έχει αποθηκευτεί από τον admin.
        /// </summary>
        /// <param name="schemaId">Το id του σχήματος</param>
        public void updateSchemaByAdmin(string schemaId)
        {
            int id = Int32.Parse(schemaId);
            string query = "UPDATE `Schemas` SET `createdBy`= 'admin' WHERE `id`= "+ id +"";
            
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
                lastEditSet(schemaId);

                //close connection
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Μέθοδος που θέτει το προεπιλεγμένο σχήμα
        /// </summary>
        /// <param name="schemaName">Όνομα σχήματος</param>
        public void setDefaultSchema(string schemaName)
        {
            string query = "UPDATE `lefterisxris`.`Schemas` SET `isDefault` = 0";
            //UPDATE `lefterisxris`.`Schemas` SET `isDefault` = 1 WHERE `name`='Default.txt'
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

                query = "UPDATE `lefterisxris`.`Schemas` SET `isDefault` = 1 WHERE `name`='"+ schemaName +"'";
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }

        }

        /// <summary>
        /// Μέθοδος που ελέγχει αν ο χρήστης υπάρχει στη βάση δεδομένων, κι αν ναι, ενημερώνει τα δεδομένα του.
        /// </summary>
        /// <param name="username">Το όνομα χρήστη το οποίο ελέγχει</param>
        /// <returns></returns>
        public bool authenticateUser(string username)
        {
            string query = "SELECT EXISTS(SELECT 1 FROM `Admin` WHERE `name`='" + username + "') AS result";

            bool authenticationResult = false;

            //open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    int r = Int32.Parse(dataReader["result"] + "");
                    if (r == 1)
                        authenticationResult = true;
                }

                dataReader.Close(); // κλείνει η ανοιχτή σύνδεση ώστε να γίνουν τα επόμενα.

                
                this.CloseConnection();
            }

            return authenticationResult;
        }

        /// <summary>
        /// Μέθοδος που παίρνει την ημερομηνία τελευταίας πρόσβασης για έναν χρήστη.
        /// </summary>
        /// <param name="username">Το όνομα χρήστη</param>
        /// <returns></returns>
        public string getNsetLastLogin(string username)
        {
            string query = "SELECT `lastLogin` FROM `Admin` WHERE `name`='" + username + "'";

            string res = "";

            //open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    res = dataReader["lastLogin"] + "";
                }

                dataReader.Close(); // κλείνει η ανοιχτή σύνδεση ώστε να γίνουν τα επόμενα.

                // Ενημέρωση lastLogin
                query = "UPDATE `Admin` SET `lastLogin`= now() WHERE `name`='" + username + "' ";
                cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                // Ενημέρωση μετρητή
                query = "UPDATE `Admin` SET `loginCount`= `loginCount` + 1 WHERE `name`='" + username + "' ";
                cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                
                this.CloseConnection();
            }
            return res;
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


        #region Stats

        public List<string>[] getStatsforPies(string action)
        {
            string query = "SELECT `name`,`"+ action +"` FROM `Schemas`";

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
                    list[0].Add(dataReader["name"] + "");
                    list[1].Add(dataReader[action] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

            }
            return list;
        }

        /*
         
        SELECT `name`,`nLoad` FROM `Schemas`

        */
        #endregion

    }
}