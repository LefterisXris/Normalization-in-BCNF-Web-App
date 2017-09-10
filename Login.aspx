<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>

<body>
    <form id="form1" runat="server">
    <div class="container">

        <!-- HEADER -->
        <div class="page-header">
            <h1>Login Page</h1>
        </div>

        <!-- Row με login item -->
        <div class="row">
            <div class="col-md-8">
                <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" EnableTheming="True" TitleText="Σύνδεση διαχειριστή">
                    <LoginButtonStyle CssClass="btn btn-md btn-info" />
                    <TitleTextStyle HorizontalAlign="Left" />
                </asp:Login>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login1" />

                <asp:Label id="Msg" ForeColor="maroon" runat="server" />
                <br />
                <asp:HyperLink class="btn btn-success btn-lg" ID="ReturnHyperLink" runat="server" NavigateUrl="~/Default.aspx" ToolTip="Επιστροφή στην Default.aspx">Επιστροφή</asp:HyperLink>
                
            </div>
        </div>

    
    </div>
    </form>
</body>
</html>
