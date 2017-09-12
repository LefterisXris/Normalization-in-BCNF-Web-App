using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Normalization;
using MySql.Data.MySqlClient;

public partial class MemberPages_Statistics : System.Web.UI.Page
{
    DBConnect dbConnect = new DBConnect(true); // Αντικείμενο που επιτρέπει την κλήση μεθόδου που παίρνει το connectionString.
    List<string> actions = new List<string> { "nLoad", "nClosure", "nFindKeys", "nDecompose", "nStepsDecompose" };

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            getTableFromDB();

            List<string> schemaNames = dbConnect.getAllSchemaNames();
            List<string> chartTypes = new List<string> { "bar", "pie", "doughnut" };
            List<string> source = new List<string> { "Per Action", "Per Schema" };

            SchemaNamesDropDownList2.Items.Add("all");
            ActionsDropDownList2.Items.Add("all");
            
            foreach (string s in schemaNames)
            {
                SchemaNamesDropDownList.Items.Add(s);
                SchemaNamesDropDownList2.Items.Add(s);
            }

            foreach (string s in actions)
            {
                ActionsDropDownList.Items.Add(s);
                ActionsDropDownList2.Items.Add(s);
            }
                
            foreach (string s in chartTypes)
                ChartTypeDropDownList.Items.Add(s);

            foreach (string s in source)
                SourceDropDownList.Items.Add(s);

        }
    }

    /// <summary>
    /// Μέθοδος που γεμίζει το gridView με τα δεδομένα από τη ΒΔ.
    /// </summary>
    private void getTableFromDB()
    {
        using (MySqlConnection con = dbConnect.getC())
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `Schemas`"))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    da.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        da.Fill(dt);
                        gridViewDatabase.DataSource = dt;
                        gridViewDatabase.DataBind();
                    }
                }
            }
        }
        
    }


    protected void btnUpdateData_Click(object sender, EventArgs e)
    {
        string schema = SchemaNamesDropDownList2.SelectedValue;
        string action = ActionsDropDownList2.SelectedValue;
        int val =  Int32.Parse(tbxValueToSet.Value);


        if(action.Equals("all"))
        {
            foreach (string s in actions)
            {
                dbConnect.ResetStatistics(schema, s, val);
            }
        }
        else
            dbConnect.ResetStatistics(schema, action, val);

    }
}