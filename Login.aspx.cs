using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {

        if (Login1.UserName.Length > 5 && Login1.Password.Length > 3)
        {
            Msg.Text = "Wellcome back!!.";
            FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
        }
        else
            Msg.Text = "Login failed. Please check your user name and password and try again.";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //  Response.Redirect("http://ilust.uom.gr:9000/Default.aspx");
        Response.Redirect("Default.aspx");
    }
}