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
    <script src="../Scripts/Chart.js"></script>
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
		    <li><a data-toggle="pill" href="#Stats">Πίνακας Στατιστικών</a></li>
		    <li class="active"><a data-toggle="pill" href="#charts">Γραφήματα</a></li>
		</ul>
        </div>

		<div class="tab-content">
			<div id="Stats" class="tab-pane fade in">
				<h3>Πίνακας Στατιστικών</h3>
                    
                <asp:GridView CssClass="table table-hover table-striped" ID="gridViewDatabase" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="2px">            
                    <Columns>
                
                    </Columns>
                </asp:GridView>

                <div>
                        <asp:HyperLink ID="AdminPageHyperLink" runat="server" NavigateUrl="~/MemberPages/Admin.aspx">Back to Admin Page</asp:HyperLink>
                </div>

			</div>

			<div id="charts" class="tab-pane fade in active">
				<h3>Γραφήματα</h3>

                 <canvas id="myChart" ></canvas>              
			</div>
            <asp:DropDownList ID="SchemaNamesDropDownList" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="ActionsDropDownList" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="ChartTypeDropDownList" runat="server"></asp:DropDownList>
            <button type="button" id="btn1">Click To create bar chart!</button>
		</div>

        
        <br />
        

    </div>
    </form>
</body>

    <script>
	    
	    $(document).ready(function () {
	        $("table tr th").click(function () {
	            $(this).html("Marketting Document/URL");
	            alert("Data: " + $(this).text());
	        });

	        var dbData = new Array();

	        var table = $("table");
	        table.find('tr').each(function (i, el) {
	            var $tds = $(this).find('td'),
                    id = $tds.eq(0).text(),
                    sc_name = $tds.eq(1).text(),
                    sc_nLoad = $tds.eq(2).text(),
                    sc_nClosure = $tds.eq(3).text(),
                    sc_nFindKeys = $tds.eq(4).text(),
                    sc_nDecompose = $tds.eq(5).text(),
                    sc_nStepsDecompose = $tds.eq(6).text();
	            // do something with productId, product, Quantity
	            // alert("id: " + id + " name: " + name + " nLoad: " + nLoad + " i =" + i);
	            dbData.push({ name: sc_name, nLoad: sc_nLoad, nClosure: sc_nClosure, nFindKeys: sc_nFindKeys, nDecompose: sc_nDecompose, nStepsDecompose: sc_nStepsDecompose });
	            //alert("Schema: " + dbData[i]["name"] + " nLoad = " + dbData[i]["nLoad"]);
	        });




	    });

	    $("#btn1").click(function () {

	        var dbData = new Array();
	        var action = $("#ActionsDropDownList option:selected").text();
	        var chartType = $("#ChartTypeDropDownList option:selected").text();

	        var table = $("table");
	        table.find('tr').each(function (i, el) {
	            var $tds = $(this).find('td'),
                    id = $tds.eq(0).text(),
                    sc_name = $tds.eq(1).text(),
                    sc_nLoad = $tds.eq(2).text(),
                    sc_nClosure = $tds.eq(3).text(),
                    sc_nFindKeys = $tds.eq(4).text(),
                    sc_nDecompose = $tds.eq(5).text(),
                    sc_nStepsDecompose = $tds.eq(6).text();
	            // do something with productId, product, Quantity
	            // alert("id: " + id + " name: " + name + " nLoad: " + nLoad + " i =" + i);
	            dbData.push({ name: sc_name, nLoad: sc_nLoad, nClosure: sc_nClosure, nFindKeys: sc_nFindKeys, nDecompose: sc_nDecompose, nStepsDecompose: sc_nStepsDecompose });
	            //alert("Schema: " + dbData[i]["name"] + " nLoad = " + dbData[i]["nLoad"]);
	        });
	        
	        $("#myChart").remove(); 
	        $('#charts').append('<canvas id="myChart" height="100%"><canvas>');




	        var ctx = document.getElementById("myChart").getContext('2d');
	        
	        var myChart = new Chart(ctx, {
	            type: chartType, // 'bar' 'doughnut'
	            data: {
	                labels: [dbData[0]["name"], dbData[1]["name"], dbData[2]["name"], dbData[3]["name"], dbData[4]["name"], dbData[5]["name"], dbData[6]["name"], dbData[7]["name"], dbData[8]["name"], dbData[9]["name"]],
	                datasets: [{
	                    label: 'number of ' + action,
	                    data: [dbData[0][action], dbData[1][action], dbData[2][action], dbData[3][action], dbData[4][action], dbData[5][action], dbData[6][action], dbData[7][action], dbData[8][action], dbData[9][action]],
	                    backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
	                    ],
	                    borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
	                    ],
	                    borderWidth: 1
	                }]
	            },
	            options: {
	            }
	        });
	        
	        
	    });


        
        /*
	    var ctx = document.getElementById("myChart").getContext('2d');
	   /* var chart = new Chart(ctx, {
	        // The type of chart we want to create
	        type: 'line',

	        // The data for our dataset
	        data: {
	            labels: ["January", "February", "March", "April", "May", "June", "July"],
	            datasets: [{
	                label: "My First dataset",
	                backgroundColor: 'rgb(255, 99, 132)',
	                borderColor: 'rgb(255, 99, 132)',
	                data: [0, 10, 5, 2, 20, 30, 45],
	            }]
	        },

	        // Configuration options go here
	        options: {}
	    });*/

	/*    var myChart = new Chart(ctx, {
	        type: 'bar',
	        data: {
	            labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
	            datasets: [{
	                label: '# of Votes',
	                data: [12, 19, 3, 5, 2, 3],
	                backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
	                ],
	                borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
	                ],
	                borderWidth: 1
	            }]
	        },
	        options: {
	            scales: {
	                yAxes: [{
	                    ticks: {
	                        beginAtZero: true
	                    }
	                }]
	            }
	        }
	    });
        */
        </script>


</html>
