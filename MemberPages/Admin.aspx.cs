using System;
using Normalization;

public partial class MemberPages_Admin : System.Web.UI.Page
{
    DBConnect dbConnect = new DBConnect();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblLastLogin.Text = dbConnect.getNsetLastLogin(User.Identity.Name.ToString());
        
    }
}