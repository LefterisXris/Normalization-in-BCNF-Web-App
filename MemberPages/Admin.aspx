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
    <link rel="stylesheet" href="../Style/bcnfStyle.css"/>
    
</head>

<body>
    <form id="form1" runat="server">

    <!-- Navigation menu fixed -->
    <nav id="nav" class="navbar navbar-toggleable-md navbar-inverse bg-inverse fixed-top" runat="server">
	    <div class="container-fluid">
	    <div class="navbar-header">
		    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
		    <span class="icon-bar"></span>
		    <span class="icon-bar"></span>
		    <span class="icon-bar"></span>
		    </button>
		    <a class="navbar-brand navTitle" href="#">Επιλογές Διαχειριστή</a>
	    </div>

            
	    <div class="collapse navbar-collapse" id="myNavbar">

            <!-- Admin Buttons -->
		    <ul class="nav navbar-nav">
                <li><a href="../Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
		        <li><asp:Button ID="btnSaveSchema" class="btn btn-success btn-sm headerButtons" runat="server" Text="Αποθήκευση Σχήματος" Enabled="False" /></li>
		        <li><asp:Button ID="btnSetDefaultSchema" class="btn btn-success btn-sm headerButtons showLoader" runat="server" Text="Επιλογή Προεπιλεγμένου" Enabled="False" /></li>
		        <li><asp:Button ID="Button1" runat="server" class="btn btn-success btn-sm headerButtons" PostBackUrl="~/MemberPages/Statistics.aspx"  Text="Εμφάνιση στατιστικών"/></li>
                <li><asp:Button ID="Button2" class="btn btn-success btn-sm headerButtons" runat="server" Text="Admin Page" PostBackUrl="~/MemberPages/Admin.aspx" /></li>                
		    </ul>
		        
            <!-- Admin Login Status -->
            <ul class="nav navbar-nav navbar-right">
		        <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                    <asp:LoginName ID="LoginName2" runat="server" />
                    </a>
		        </li>
		        <li id="lgout">
                    <span class="glyphicon glyphicon-log-out" style="color:#9d9d9d"></span> 
                    <asp:LoginStatus ID="LoginStatus2" runat="server"  />
		        </li>
		    </ul>

	    </div>
	    </div>
    </nav>

    <div class="container">
        
        <!-- Header -->
        <div class="page-header">
            <h1>Welcome to the members-only page.
                <small>Welcome, 
                    <asp:LoginName ID="LoginName1" runat="server" />
                </small>
            </h1>
        </div>
        
        <!-- Row με Ημ/νία -->
        <div class="row">
            <div class="col-md-8">
                <asp:Label ID="Label1" runat="server" Text="Ημερομηνία τελευταίας πρόσβασης: "></asp:Label>
                <asp:Label ID="lblLastLogin" runat="server" Text=""></asp:Label>

                <br /> <br /> <br />
            </div>
        </div>

        <h3 style="color:#669999;"> Επιλογές διαχειριστή:</h3> 
        <hr style="height:2px; background-color:grey;"/>

        <!-- Row με επιλογές διαχειριστή -->
         <div class="row">
            <div class="col-md-12">
                <asp:HyperLink class="btn btn-success btn-lg" ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx" ToolTip="Εξτρά δυνατότητες ">Διαχείριση αρχικής σελίδας</asp:HyperLink>
                <asp:HyperLink class="btn btn-success btn-lg" ID="StatisticsHyperLink" runat="server" NavigateUrl="~/MemberPages/Statistics.aspx">Εμφάνιση στατιστικών</asp:HyperLink>

                <asp:LoginStatus ID="LoginStatus1" runat="server" ForeColor="Red" CssClass="btn btn-danger btn-lg logoutRedBtn" />
            </div>
        </div>

        <hr style="height:2px; background-color:grey;"/>

    </div>

    <!-- Footer -->
    <footer class="text-center">

        <a class="up-arrow" href="#" data-toggle="tooltip" title="TO TOP">
		<span class="glyphicon glyphicon-chevron-up"></span>
		</a>

		<a href="http://www.uom.gr/index.php?tmima=6&categorymenu=2"><p>University of Macedonia &copy; 2017</p></a>
		
	</footer>

    </form>

    <script>
        // Χρωματισμός του glyphicon
        $(".nav>#lgout>a").mouseover(function () {
            $(".glyphicon-log-out").css("color", "white");
        });

        // Αναίρεση χρωματισμού του glyphicon
        $(".nav>#lgout>a").mouseout(function () {
            $(".glyphicon-log-out").css("color", "#9d9d9d"); //
        });
    </script>
</body>
</html>
