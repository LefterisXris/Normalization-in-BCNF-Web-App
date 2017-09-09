<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="MemberPages_Statistics" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statistics</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="../Scripts/Chart.min.js"></script>
</head>

<body>
    <form id="form1" runat="server">
    <div class="container">
    
        <%-- HEADER (τίτλο, όνομα κλπ) --%>
        <div class="page-header">
            <h1>Δεδομένα χρήσης Εφαρμογής
                <small>Στατιστικά και μετρήσεις</small></h1>
            <ul class="nav nav-pills">
		    <li class="active"><a data-toggle="pill" href="#Stats">Πίνακας Στατιστικών</a></li>
		    <li><a data-toggle="pill" href="#charts">Γραφήματα</a></li>
		</ul>
        </div>

		<div class="tab-content">
			<div id="Stats" class="tab-pane fade in active">
				<h3>Πίνακας Στατιστικών</h3>
                    
                <asp:GridView CssClass="table table-hover table-striped" ID="gridViewDatabase" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="2px">            
                    <Columns>
                
                    </Columns>
                </asp:GridView>

                <div>
                        <asp:HyperLink ID="AdminPageHyperLink" runat="server" NavigateUrl="~/MemberPages/Admin.aspx">Back to Admin Page</asp:HyperLink>
                </div>

			</div>

			<div id="charts" class="tab-pane fade">
				<h3>Γραφήματα</h3>

                <canvas id="doughNutChartLoc" height="300" width="300"></canvas>

                <asp:Label ID="lbl_crct" runat="server" Text="" ></asp:Label>
                <asp:Label ID="lbl_incrct" runat="server" Text="" ></asp:Label>
                <asp:Label ID="lbl_incrct2" runat="server" Text=""></asp:Label>

                <canvas id="canvas" height="300" width="300"></canvas>
                <canvas id="d" height="300" width="300"></canvas>
                <canvas id="myChart" width="400" height="400"></canvas>

                
			</div>

			    
		</div>

        
        <br />
        

    </div>
    </form>
</body>

    <script>
	        
        var crct = $('#<%= lbl_crct.ClientID %>').text();
        var incrct = $('#<%= lbl_incrct.ClientID %>').text();
        var incrct2 = $('#<%= lbl_incrct2.ClientID %>').text();
        var doughnutData = [
                {
                    value: parseInt(incrct2),
                    color: "#F7464A"
                },
                {
                    value: parseInt(crct),
                    color: "#8cc63f"
                },
                {
                    value: parseInt(incrct),
                    color: "#8AA63f"
                }

        ];

        var myDoughnut2 = new Chart(document.getElementById("canvas").getContext("2d")).Doughnut(doughnutData);
        
	    var DoughNutChartData = [
            {
                value: parseInt(crct),
                color:"lightblue"
                
            },
            {
                value: parseInt(incrct2),
                color: "red"

            },
            {
                value: parseInt(incrct),
                color: "green"

            }
	    ]
	    var myDoughnut = new Chart(document.getElementById("doughNutChartLoc").getContext("2d")).Doughnut(DoughNutChartData);
	    //var bla = new Chart(document.getElementById("doughNutChartLoc").getContext("2d")).Pie(DoughNutChartData);
	    
	    $(document).ready(function () {
	        $("table tr th").click(function () {
	            $(this).html("Marketting Document/URL");
	            alert("Data: " + $(this).text());
	        });

	        var table = $("table");
	        table.find('tr').each(function (i, el) {
	            var $tds = $(this).find('td'),
                    id = $tds.eq(0).text(),
                    name = $tds.eq(1).text(),
                    nLoad = $tds.eq(2).text();
	            // do something with productId, product, Quantity
	           // alert("id: " + id + " name: " + name + " nLoad: " + nLoad + " i =" + i);
	        });

	    });


        </script>


</html>
