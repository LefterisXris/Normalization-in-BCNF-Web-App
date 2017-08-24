<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Login Page</h1>
        <p>
            <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate">
            </asp:Login>
        </p>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login1" />

        <asp:Label id="Msg" ForeColor="maroon" runat="server" />&nbsp;<br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Return" />
        <br />
    
    </div>
    </form>
</body>
</html>
