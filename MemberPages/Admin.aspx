<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="MemberPages_Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BCNF</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <h1>Welcome to the members-only page.Welcome,
            <asp:LoginName ID="LoginName1" runat="server" />
        </h1>
        <p>
            <asp:LoginStatus ID="LoginStatus1" runat="server" />
&nbsp;<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Διαχείριση αρχικής σελίδας</asp:HyperLink>
        </p>
        <p>
            &nbsp;</p>
        <p>
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/MemberPages/Stats.aspx">Εμφάνιση στατιστικών</asp:HyperLink>
            
            </p>
        <p>
            &nbsp;</p>
    
    </div>
    </form>
</body>
</html>
