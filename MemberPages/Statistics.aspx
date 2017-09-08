<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="MemberPages_Statistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statistics</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>

<body>
    <form id="form1" runat="server">
    <div class="container">
    
        <%-- HEADER (τίτλο, όνομα κλπ) --%>
        <div class="page-header">
            <h1>Δεδομένα χρήσης Εφαρμογής
                <small>Στατιστικά και μετρήσεις</small></h1>
        </div>

        <asp:GridView CssClass="table table-hover table-striped" ID="gridViewDatabase" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="2px">            
            <Columns>
                
            </Columns>
        </asp:GridView>

        <div>
             <asp:HyperLink ID="AdminPageHyperLink" runat="server" NavigateUrl="~/MemberPages/Admin.aspx">Back to Admin Page</asp:HyperLink>
        </div>
    </div>
    </form>
</body>
</html>
