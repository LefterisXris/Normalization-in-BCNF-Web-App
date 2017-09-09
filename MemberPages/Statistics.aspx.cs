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

    protected void Page_Load(object sender, EventArgs e)
    {
        getTableFromDB();

        List<string>[] list = dbConnect.getStatsforPies("nLoad");

        lbl_crct.Text = "50";
        lbl_incrct.Text = "40";
        lbl_incrct2.Text = "10";
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
    
}