using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Normalization;

public partial class Login : System.Web.UI.Page
{
    DBConnect dbConnect = new DBConnect();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (dbConnect.authenticateUser(Login1.UserName) && Login1.Password.Length == 5)
        {
           
            Msg.Text = "Wellcome back!!.";
            FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
            
        }
        else
            Msg.Text = "Login failed. Please check your user name and password and try again.";
        }

}