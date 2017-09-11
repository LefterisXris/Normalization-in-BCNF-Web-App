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

    <style>
        .highlight { background-color: #A1DCF2; }
        #gridViewDatabase tr:hover{background-color: #A1DCF2;}
        .btn, .alert{
            margin-left: 5px;
            margin-top: 5px;
        }
        .nav>li>a{display:inline-block !important; padding-left: 0 !important;}
        .myGlyph{color:white;}
        .hea{
            margin-top:10px !important;
        }
        .nav-pills>li>a{padding: 10px 15px !important;}
        .fixed-top{position:fixed; right:0; left:0; z-index:1030;}
        .page-header {margin: 100px 0 20px;}
    </style>
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
		    <a class="navbar-brand" href="#" style="color:#669999;">Επιλογές Διαχειριστή</a>
	    </div>

            
	    <div class="collapse navbar-collapse" id="myNavbar">

            <!-- Admin Buttons -->
		    <ul class="nav navbar-nav">
                <li><a href="../Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
		        <li><asp:Button ID="btnSaveSchema" class="btn btn-success btn-sm hea" runat="server" Text="Αποθήκευση Σχήματος" Enabled="False" /></li>
		        <li><asp:Button ID="btnSetDefaultSchema" class="btn btn-success btn-sm hea showLoader" runat="server" Text="Επιλογή Προεπιλεγμένου" Enabled="False" /></li>
		        <li><asp:Button class="btn btn-success btn-sm hea" ID="StatisticsHyperLink" runat="server" PostBackUrl="~/MemberPages/Statistics.aspx"  Text="Εμφάνιση στατιστικών"/></li>
                <li><asp:Button ID="Button1" class="btn btn-success btn-sm hea" runat="server" Text="Admin Page" PostBackUrl="~/MemberPages/Admin.aspx" /></li>                
		    </ul>
		        
            <!-- Admin Login Status -->
            <ul class="nav navbar-nav navbar-right">
		        <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                    <asp:LoginName ID="LoginName1" runat="server" />
                    </a>
		        </li>
		        <li id="lgout">
                    <span class="glyphicon glyphicon-log-out" style="color:#9d9d9d"></span> 
                    <asp:LoginStatus ID="LoginStatus1" runat="server"  />
		        </li>
		    </ul>

	    </div>
	    </div>
    </nav>

    <div class="container">
    
        <%-- HEADER (Τίτλο, Pager) --%>
        <div class="page-header">
            <h1>Δεδομένα χρήσης Εφαρμογής
                <small>Στατιστικά και μετρήσεις</small></h1>
            <!-- Pager menu -->
            <ul id="navPills" class="nav nav-pills">
		        <li class="active"><a data-toggle="pill" href="#Stats">Πίνακας Στατιστικών</a></li>
		        <li><a data-toggle="pill" href="#charts">Γραφήματα</a></li>
                <li><a data-toggle="pill" href="#Manage">Διαχείριση</a></li>
		    </ul>
            <!-- Pager menu -->
        </div>

		<div class="tab-content">

            <!-- Πίνακας με δεδομένα -->
			<div id="Stats" class="tab-pane fade in active">
				<h3>Πίνακας Στατιστικών</h3>
                    
                <asp:GridView CssClass="table" ID="gridViewDatabase" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="2px">            
                    <Columns>
                
                    </Columns>
                </asp:GridView>

                <asp:HiddenField ID="gridViewDatabaseHiddenField" runat="server" Value="-3" />

			</div>

            <!-- Γραφήματα -->
			<div id="charts" class="tab-pane fade in">
				<h3>Γραφήματα</h3>
                
                <div id="chartSpace">
                    <h3 id="protropiH3"><small>(Επιλέξτε από τις διαθέσιμες επιλογές, για δημιουργία γραφήματος)</small></h3>
                    <canvas id="myChart" ></canvas>          
                </div>
                     
			    <!-- Λίστες για επιλογή γραφημάτων -->
                <h5><asp:Label ID="lblSchemaDescription" runat="server" Text="Επιλογές για γράφημα:" Font-Italic="True" ForeColor="#669999"></asp:Label></h5>
                <asp:DropDownList ID="SourceDropDownList" runat="server" ></asp:DropDownList>
                <asp:DropDownList ID="SchemaNamesDropDownList" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ActionsDropDownList" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ChartTypeDropDownList" runat="server"></asp:DropDownList>
                <button type="button" class="btn btn-info btn-lg" id="btnGenerateChart">Δημιουργία γραφήματος!</button>

            </div>

            <!-- Διαχείριση δεδομένων -->
			<div id="Manage" class="tab-pane fade in">
				<h3>Διαχείριση Δεδομένων</h3>
                <h3><small>Επιλέξτε δεδομένα για μηδενισμό</small></h3>

                Επιλογή: <asp:DropDownList ID="SchemaNamesDropDownList2" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ActionsDropDownList2" runat="server"></asp:DropDownList>
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                <asp:Button ID="btnClearData" class="btn btn-info btn-lg" runat="server" Text="Μηδενισμός" OnClick="btnClearData_Click" />
			</div>

		</div>

    </div>
    </form>
</body>

<script>
	    
    // Η συνάρτηση ενημερώνει την ορατότητα των λιστών ανάλογα με την επιλογή του χρήστη. 
    function setVisibilityOnDDLs() {
        var selectedVal = $('#SourceDropDownList option:selected').attr('value');
        var isActionSelected = selectedVal[4] == "A";

        if (isActionSelected) {
            $("#SchemaNamesDropDownList").hide();
            $("#ActionsDropDownList").show();
        }
        else {
            $("#ActionsDropDownList").hide();
            $("#SchemaNamesDropDownList").show();
        }
    }

	$(document).ready(function () {
	    setVisibilityOnDDLs();
	});

    // Δημιουργία γραφήματος.
	$("#btnGenerateChart").click(function () {

	    $("#protropiH3").hide();
	    var dbData = new Array(); // Ο πίνακας στον οποίο θα αποθηκευτούν τα δεδομένα.
	    var source = $("#SourceDropDownList option:selected").text(); // Η επιλογή για το είδος απεικόνισης.

	    var isAction = source[4] == "A"; // Αν έχει επιλεγει Per Action=αληθής.

	    var selectedSchemaName = $("#SchemaNamesDropDownList option:selected").text(); // Το επιλεγμένο σχήμα.
	    var action = $("#ActionsDropDownList option:selected").text(); // Η επιλογή ενέργειας για οπτικοποίηση (nLoad κλπ).
	    var chartType = $("#ChartTypeDropDownList option:selected").text(); // Ο τύπος του γραφήματος.
	        
        // Για κάθε γραμμή του πίνακα, κρατάω τα δεδομένα που με ενδιαφέρουν ώστε να τα απεικονίσω.
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
	            
            // Αποθηκεύω στον πίνακα, αντικείμενα με τρόπο key=value.
	        dbData.push({ name: sc_name, nLoad: sc_nLoad, nClosure: sc_nClosure, nFindKeys: sc_nFindKeys, nDecompose: sc_nDecompose, nStepsDecompose: sc_nStepsDecompose });
	           
	    });
	        
        // Επειδή δεν δουλεύει η ανανέωση του γραφήματος, κάθε φορά διαγράφω τον καμβά και φτιάχνω άλλον.
	    $("#myChart").remove(); 
	    $('#chartSpace').append('<canvas id="myChart" height="100%"><canvas>');
	        
	    var names = []; // Τα δεδομένα για τον άξονα X.
	    var values = []; // Τα δεδομένα για τον άξονα Y.
	    var chartLabel;

	    if (isAction) {
	        for (i = 1; i < dbData.length; i++) {
	            names.push(dbData[i]["name"]);
	            values.push(dbData[i][action]);
	        }
	        chartLabel = "Number of " + action;
	    }
	    else { // Όταν βρεθεί το σχήμα, πάρε τις τιμές που θέλεις.
	        for (i = 1; i < dbData.length; i++) {
	            if (dbData[i]["name"] == selectedSchemaName) {
	                names = ["nLoad", "nClosure", "nFindKeys", "nDecompose", "nStepsDecompose"];
	                for (j = 0; j < names.length; j++)
	                    values.push(dbData[i][names[j]]);
	            }
	        }
	        chartLabel = "Schema " + selectedSchemaName + " metrics";
	    }
	        
	    var ctx = document.getElementById("myChart").getContext('2d'); // παίρνω τον καμβά που θα μπει το γράφημα.
	        
        // Δημιουργία γραφήματος
	    var myChart = new Chart(ctx, {
	        type: chartType, // Τύπος του γραφήματος.
	        data: {
	            labels: names, // tooltips για κάθε δεδομένο.
	            datasets: [{
	                label: chartLabel,//'number of ' + action, // ετικέτα ως τίτλος και ως tooltip.
	                data: values, // Τα δεδομένα φορτώνονται από εδώ.
	                backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(98, 255, 236, 0.2)',
                        'rgba(155, 255, 0, 0.2)',
                        'rgba(21, 37, 211, 0.2)',
                        'rgba(98, 151, 255, 0.2)',
                        'rgba(109, 255, 98, 0.2)',
                        'rgba(255, 101, 98, 0.2)',
                        'rgba(184, 98, 255, 0.2)',
                        'rgba(255, 168, 30, 0.2)',
                        'rgba(202, 255, 30, 0.2)',
                        'rgba(98, 255, 236, 0.2)',
                        'rgba(187, 0, 255, 0.2)',
                        'rgba(195, 255, 98, 0.2)',
                        'rgba(255, 98, 239, 0.2)',
                        'rgba(98, 151, 255, 0.2)',
                        'rgba(109, 255, 98, 0.2)',
                        'rgba(255, 101, 98, 0.2)',
                        'rgba(184, 98, 255, 0.2)',
                        'rgba(255, 168, 30, 0.2)',
                        'rgba(202, 255, 30, 0.2)'
	                ],
	                borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(98, 255, 236, 1)',
                        'rgba(155, 255, 0, 1)',
                        'rgba(21, 37, 211, 1)',
                        'rgba(98, 151, 255, 1)',
                        'rgba(109, 255, 98, 1)',
                        'rgba(255, 101, 98, 1)',
                        'rgba(184, 98, 255, 1)',
                        'rgba(255, 168, 30, 1)',
                        'rgba(202, 255, 30, 1)',
                        'rgba(98, 255, 236, 1)',
                        'rgba(187, 0, 255, 1)',
                        'rgba(195, 255, 98, 1)',
                        'rgba(255, 98, 239, 1)',
                        'rgba(98, 151, 255, 1)',
                        'rgba(109, 255, 98, 1)',
                        'rgba(255, 101, 98, 1)',
                        'rgba(184, 98, 255, 1)',
                        'rgba(255, 168, 30, 1)',
                        'rgba(202, 255, 30, 1)'
	                ],
	                borderWidth: 1
	            }]
	        },
	        options: {} 
	    });
	         
	});

    // Κατά την αλλαγή της επιλογής Per Action - Per Schema ενημερώνεται η ορατότητα των αντίστοιχων λιστών.
	$('#SourceDropDownList').change(function () {
	    setVisibilityOnDDLs();
	});

	$("table tr").click(function () {

	    var selected = $(this).hasClass("highlight");
	    var gridView = $(this).closest("table").attr("id");
	    var hiddenField = gridView.concat("HiddenField");

	    $("#" + gridView + " tr").removeClass("highlight");
	    if (!selected) {
	        $(this).addClass("highlight");
	        $("#" + hiddenField).val($(this).index() - 1);
	    }
	    else {
	        $("#" + hiddenField).val(-3);
	    }
	    
	});

	// Χρωματισμός του glyphicon
	$(".nav>#lgout>a").mouseover(function () {
	    $(".glyphicon-log-out").css("color", "white");
	});

	// Αναίρεση χρωματισμού του glyphicon
	$(".nav>#lgout>a").mouseout(function () {
	    $(".glyphicon-log-out").css("color", "#9d9d9d"); //
	});


</script>

</html>
