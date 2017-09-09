<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="MemberPages_Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        .logout{float:right;}
    </style>
</head>

<body>
    <form id="form1" runat="server">
    <div class="container">
        
        <!-- Header -->
        <div class="page-header">
            <h1>Welcome to the members-only page.
                <small>Welcome, 
                    <asp:LoginName ID="LoginName1" runat="server" />
                </small>
            </h1>
        </div>
        
        <div class="row">
            <div class="col-md-8">
                <asp:Label ID="Label1" runat="server" Text="Ημερομηνία τελευταίας πρόσβασης: "></asp:Label>
                <asp:Label ID="lblLastLogin" runat="server" Text=""></asp:Label><br />

                <br /><br />
                
                
            </div>
        </div>

        <h3 style="color:#669999;"> Επιλογές διαχειριστή:</h3> 
        <hr style="height:2px; background-color:grey;"/>

         <div class="row">
            <div class="col-md-12">
                <asp:HyperLink class="btn btn-success btn-lg" ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Διαχείριση αρχικής σελίδας</asp:HyperLink>
                <asp:HyperLink class="btn btn-success btn-lg" ID="StatisticsHyperLink" runat="server" NavigateUrl="~/MemberPages/Statistics.aspx">Εμφάνιση στατιστικών</asp:HyperLink>

                <asp:LoginStatus ID="LoginStatus1" runat="server" ForeColor="Red" CssClass="btn btn-danger btn-lg logout" />
            </div>
        </div>

        <hr style="height:2px; background-color:grey;"/>

    </div>
    </form>

</body>
</html>
