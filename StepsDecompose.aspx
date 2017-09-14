<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StepsDecompose.aspx.cs" Inherits="StepsDecompose" EnableEventValidation="false"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BCNF</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
    <link rel="stylesheet" href="Style/bcnfStyle.css"/>
</head>

<body>

    <form id="form1" runat="server">

         <!-- Navigation menu fixed for Admins -->
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
		                <li><asp:Button ID="btnStatistics" class="btn btn-success btn-sm headerButtons" runat="server" PostBackUrl="~/MemberPages/Statistics.aspx" Text="Εμφάνιση στατιστικών"/></li>
                        <li><asp:Button ID="btnAdmin" class="btn btn-success btn-sm headerButtons" runat="server" Text="Admin Page" PostBackUrl="~/MemberPages/Admin.aspx" /></li>                
		            </ul>
		        
                    <!-- Admin Login Status -->
                    <ul class="nav navbar-nav navbar-right">
		                <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                            <asp:LoginName ID="LoginName2" runat="server" />
                            </a>
		                </li>
		                <li id="lgout">
                            <span class="glyphicon glyphicon-log-out"></span> 
                            <asp:LoginStatus ID="LoginStatus2" runat="server"  />
		                </li>
		            </ul>

	            </div>
	        </div>
        </nav>

        <!-- Navigation menu fixed for Users -->
        <nav id="navUsers" class="navbar navbar-toggleable-md navbar-inverse bg-inverse fixed-top" runat="server">
	        <div class="container-fluid">
	        
            <div class="navbar-header">
		        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbarUser">
		        <span class="icon-bar"></span>
		        <span class="icon-bar"></span>
		        <span class="icon-bar"></span>
		        </button>
		        <a class="navbar-brand navTitle" href="#">Normalization BCNF</a>
	        </div>

            
	        <div class="collapse navbar-collapse" id="myNavbarUser">

                <!-- User Buttons -->
		        <ul class="nav navbar-nav">
                    <li><a href="Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
		            <li class="dropdown">
			          <a class="dropdown-toggle" data-toggle="dropdown" href="#">Κανονικοποίηση <span class="caret"></span></a>
			          <ul class="dropdown-menu">
				        <li><a href="#"><i class="icon iconShowBCNF"></i> <asp:Button ID="btnNavShowBCNF" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Προβολή BCNF πινάκων" OnClick="btnShowBCNFTablesClick" /></a></li>
                        <li><a href="#"><i class="icon iconPreview"></i> <asp:Button ID="btnNavPreview" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Προεπισκόπηση" OnClick="btnPreview_Click" /></a></li>
				        <li><a href="#"><i class="icon iconDecompose"></i> <asp:Button ID="btnNavDecompose" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Διάσπαση" OnClick="btnDecompose_Click" /></a></li>
                        <li><a href="#"><i class="icon iconClear"></i> <asp:Button ID="btnNavClear" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Καθαρισμός" OnClick="btnClearResults_Click" /></a></li>
                        <li><a href="#"><i class="icon iconReset"></i> <asp:Button ID="btnNavReset" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Reset" OnClick="btnResetClick" /></a></li>
			          </ul>
			        </li>
		            <li class="dropdown">
			          <a class="dropdown-toggle" data-toggle="dropdown" href="#">Σχετικά <span class="caret"></span></a>
			          <ul class="dropdown-menu">
				        <li><a href="#"><i class="icon iconInfo"></i> Οδηγίες Χρήσης</a></li>
				        <li><a href="#"><i class="icon iconDiplomatic"></i> Η εργασία</a></li>
				        <li><a href="https://github.com/LefterisXris/Normalization-in-BCNF-Web-App"><i class="icon iconContribute"></i> Συνεισφορά</a></li>
			          </ul>
			        </li>
		            <li><a href="#">Επικοινωνία</a></li>
		        </ul>
		        
                <!-- Admin Login Status -->
                <ul class="nav navbar-nav navbar-right">
		            <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                        <asp:LoginName ID="LoginName1" runat="server" />
                        </a>
		            </li>
		            <li id="lgoutUser">
                        <span class="glyphicon glyphicon-log-out"></span> 
                        <asp:LoginStatus ID="LoginStatus1" runat="server"  />
		            </li>
		        </ul>

	        </div>
	        </div>
        </nav>

        <!-- Περιεχόμενο Σελίδας -->
        <div class="container">

            <div class="loader hide-loader" id="loader"></div>

            <%-- HEADER (τίτλο, όνομα κλπ) --%>
            <div class="page-header">
                <h1>Σταδιακή διάσπαση:
                    <asp:Label ID="lblSchemaName" runat="server" Text="Default"></asp:Label>
                    <small>γνωρίσματα και συναρτησιακές εξαρτήσεις. </small></h1>
                <h5><asp:Label ID="lblSchemaDescription" runat="server" Text="" Font-Italic="True" ForeColor="#669999"></asp:Label></h5>
            </div>

            <%-- ROW (πανελ με πίνακες, αποτελέσματα) --%>
            <div class="row">

                <%-- Στοιχεία σχήματος (πίνακες, συναρτησιακές, κλειδιά) --%>
                <div class="col-md-6 col-sm-6">
                    <div>

                        <p><b>Πίνακες</b></p>
                   
                        <asp:GridView ID="gridViewRelation" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                            runat="server" AutoGenerateColumns="false"  Width="100%">

                            <Columns>
                                <asp:BoundField DataField="BCNF" HeaderText=" BCNF" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="10%"/>
                                <asp:BoundField DataField="Relation" HeaderText="Πίνακες Γνωρισμάτων" ItemStyle-Width="70%" />
                            </Columns>

                        </asp:GridView>
                        
                        <asp:HiddenField ID="gridViewRelationHiddenField" runat="server" Value="-3" />

                        <asp:Button ID="btnShowBCNFtables" class="btn btn-info btn-lg showLoader" runat="server" Text="Προβολή BCNF πινάκων" Style="float: right;" OnClick="btnShowBCNFTablesClick" />

                    </div>
                    <div style="margin-top: 35px;">

                        <p><b>Συναρτησιακές εξαρτήσεις</b></p>

                        <asp:GridView ID="gridViewFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                            runat="server" AutoGenerateColumns="false"  Width="100%">

                            <Columns>
                                <asp:BoundField DataField="Excluded" HeaderText="Χ" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="10%"/>
                                <asp:BoundField DataField="Description" HeaderText="Περιγραφή" ItemStyle-Width="60%" />
                                <asp:BoundField DataField="Trivial" HeaderText="Τετ"  HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                            </Columns>

                        </asp:GridView>

                        <asp:HiddenField ID="gridViewFDHiddenField" runat="server" Value="-3" />

                        <div class="row" style="float:right;">
                            <asp:Button ID="btnNewFD" class="btn btn-info btn-md showLoader" runat="server" Text="Προσθήκη" OnClick="btnNewFDClick" />
                            <asp:Button ID="btnEditFD" class="btn btn-info btn-md showLoader" runat="server" Text="Επεξεργασία" OnClick="btnEditFDClick" />
                            <asp:Button ID="btnDeleteFD" class="btn btn-info btn-md showLoader" runat="server" Text="Διαγραφή" OnClick="btnDeleteFDClick" />
                        </div>

                    <!-- Modal νέας συναρτησιακής εξάρτησης -->
                    <div class="modal fade" id="modalNewFD" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Νέα συναρτησιακή εξάρτηση</h4>
                                </div>
                                <div class="modal-body">

                                   <p>Δομή συναρτησιακής εξάρτησης</p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewLeftFD" runat="server" AutoGenerateColumns="false" Width="100%">
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxLeftFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Orizouses" HeaderText="Ορίζουσες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%"/>
                                                </Columns>

                                            </asp:GridView>

                                            <asp:HiddenField ID="gridViewLeftFDHiddenField" runat="server" Value="-3" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewRightFD" runat="server" AutoGenerateColumns="false" Width="100%" >
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxRightFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Eksartimenes" HeaderText="Εξαρτημένες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%" />
                                                </Columns>

                                            </asp:GridView>

                                            <asp:HiddenField ID="gridViewRightFDHiddenField" runat="server" Value="-3" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-10">
                                            <p>Τελική μορφή συναρτησιακής εξάρτησης: </p>
                                            <asp:Label ID="lblPreviewFDtoCreateLeft" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblArrow" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblPreviewFDtoCreateRight" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnNewFDOK" Text="OK" class="btn btn-default showLoader" OnClick="btnNewFDOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->

                    <!-- Modal επεξεργασίας συναρτησιακής εξάρτησης -->
                    <div class="modal fade" id="modalEditFD" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Επεξεργασία συναρτησιακής εξάρτησης</h4>
                                </div>
                                <div class="modal-body">

                                   <p>Δομή συναρτησιακής εξάρτησης:</p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewEditLeftFD" runat="server" AutoGenerateColumns="false" Width="100%">
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxEditLeftFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Orizouses" HeaderText="Ορίζουσες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%"/>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewEditLeftFDHiddenField" runat="server" Value="-3" />

                                        </div>
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewEditRightFD" runat="server" AutoGenerateColumns="false" Width="100%" >
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxEditRightFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Eksartimenes" HeaderText="Εξαρτημένες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%" />
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewEditRightFDHiddenField" runat="server" Value="-3" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-10">
                                            <p>Τελική μορφή συναρτησιακής εξάρτησης:</p>
                                            <asp:Label ID="lblPreviewFDtoEditLeft" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblArrow2" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblPreviewFDtoEditRight" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnEditFDOK" Text="Ενημέρωση" class="btn btn-default showLoader" OnClick="btnEditFDΟΚClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->


                    </div>
                    <div style="margin-top: 20px;">
                        <p><b>Υποψήφια κλειδιά</b></p>
                        <!-- <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label> -->
                        <asp:TextBox ID="tbxKeys" runat="server" Width="100%"></asp:TextBox>
                    </div>

                    <div>
                        
                        <div class="alert alert-success fade in" id="alertBoxSuccess" hidden="hidden">
				            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <h4 id="alertBoxSuccessText"> <strong>Success!</strong> This alert box could indicate a successful or positive action. </h4>
			            </div>
                        <div class="alert alert-warning fade in" id="alertBoxWarning" hidden="hidden">
				            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
				            <h4 id="alertBoxWarningText"> <strong>Warning!</strong> This alert box could indicate a warning that might need attention. </h4>
			            </div>
                        <div class="alert alert-danger fade in" id="alertBoxFail" hidden="hidden">
				            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
				            <h4 id="alertBoxFailText"> <strong>Danger!</strong> This alert box could indicate a dangerous or potentially negative action. </h4>
			            </div>

                    </div>
                </div>

                <%-- Αποτελέσματα  --%>
                <div class="col-md-6 col-sm-6">
                    <div>
                        <p><b>Αποτελέσματα διάσπασης</b></p>
                        <textarea runat="server" id="resultsArea" style="width: 100%;" rows="15"></textarea>
                        <%--  <asp:Label ID="resultsArea" runat="server" Text=""></asp:Label> --%>
                    </div>
                </div>

            </div>

            <%-- ROW με επιλογές ενεργειών (κουμπιά) --%>
            <div class="row" style="margin-top: 20px;"> 
                <div class="col-md-2 col-sm-2"></div>
                <div class="col-md-6 col-sm-6">

                    <asp:Button ID="btnPreview" class="btn btn-success btn-lg showLoader" runat="server" Text="Προεπισκόπηση" OnClick="btnPreview_Click" />

                    <!-- Modal Προεπισκόπησης-->
                    <div class="modal fade" id="modalPreview" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <asp:Label class="modal-title" ID="resultTitle" runat="server" Text="Προεπισκόπηση διάσπασης σε BCNF" Font-Size="16px"></asp:Label>
                                </div>
                                <div class="modal-body myModalBody">
                                    <div class="form-horizontal"> 
                                        <textarea runat="server" id="log" class="form-control myForm" rows="15" style="min-width: 100%; min-height: 100%;" ></textarea>
                                    </div>
                                    
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default btn-success" data-dismiss="modal">Κλείσιμο</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal -->


                    <asp:Button ID="btnDecompose" class="btn btn-success btn-lg showLoader" runat="server" Text="Διάσπαση" OnClick="btnDecompose_Click" />
                    <asp:Button ID="btnClearResults" class="btn btn-success btn-lg showLoader" runat="server" Text="Καθαρισμός" OnClick="btnClearResults_Click" />
                </div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnReset" class="btn btn-success btn-lg showLoader" runat="server" Text="Reset" OnClick="btnResetClick" />
                </div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnCloseStepsDecompose" class="btn btn-danger btn-lg showLoader" runat="server" Text="Κλείσιμο" Style="width: 100%;" OnClick="btnCloseStepsDecompose_Click" />
                </div>

            </div>

        </div>
   
        <!-- Footer -->
        <footer class="text-center myFooter">

            <a class="up-arrow" href="#" data-toggle="tooltip" title="TO TOP">
			<span class="glyphicon glyphicon-chevron-up"></span>
		    </a>

			<a href="http://www.uom.gr/index.php?tmima=6&categorymenu=2"><p>University of Macedonia &copy; 2017</p></a>
		
		</footer>

    </form>


    <!-- Script με λειτουργίες της εφαρμογής -->
    <script>
        
        // Καλείται κατά την οποιαδήποτε αλλαγή σε οποιοδήποτε checkbox
        $(':checkbox').change(function () {

            var value = $(this).closest('td').next('td').html(); // Παίρνω την τιμή του επόμενου κελιού.
            var str = this.id.toString(); // παίρνω το id του checkbox που άλλαξε (επιλογή ή αποεπιλογή).
            
            if (str.indexOf("Edit") != -1) // Εάν πρόκειτε για την επεξεργασία συναρτησιακής εξάρτησης.
            {
                if (str.indexOf("Left") != -1) // Εάν πρόκειτε για το αριστερό μέλος.
                {
                    if (this.checked)  // αν είναι τσεκαρισμένο το checkbox προσθέτω το γνώρισμα στην προεπισκόπηση.
                    {
                        // Ελέγχω αν είναι το πρώτο γνώρισμα ώστε να προσθέσω το κόμα.
                        var l = $("#lblPreviewFDtoEditLeft").text().length;
                        if (l > 0) {
                            $("#lblPreviewFDtoEditLeft").append(", " + value);
                        }
                        else {
                            $("#lblPreviewFDtoEditLeft").append(value);
                        }
                    }
                    else // αν δεν είναι τσεκαρισμένο αφαιρώ το γνώρισμα από την προεπισκόπηση ελέγχοντας τη θέση του για το κόμα.
                    {
                        var pVal = $("#lblPreviewFDtoEditLeft").text();
                        var s;
                        if (pVal.indexOf(", " + value) != -1) {
                            s = pVal.replace((", " + value), "");
                        }
                        else if (pVal.length == value.length) { // αν είναι το πρώτο στοιχείο.
                            s = pVal.replace(value, "");
                        }
                        else {
                            s = pVal.replace((value + ", "), "");
                        }
                        $("#lblPreviewFDtoEditLeft").text(s);
                    }
                }
                else { // Εάν πρόκειτε για το δεξί μέλος.
                    if (this.checked) { // αν είναι τσεκαρισμένο το checkbox προσθέτω το γνώρισμα στην προεπισκόπηση.

                        // Ελέγχω αν είναι το πρώτο γνώρισμα ώστε να προσθέσω το κόμα.
                        var l = $("#lblPreviewFDtoEditRight").text().length;
                        if (l > 0) {
                            $("#lblPreviewFDtoEditRight").append(", " + value); 
                        }
                        else {
                            $("#lblPreviewFDtoEditRight").append(value);
                        }
                    }
                    else { // αν δεν είναι τσεκαρισμένο αφαιρώ το γνώρισμα από την προεπισκόπηση ελέγχοντας τη θέση του για το κόμα.
                        var pVal = $("#lblPreviewFDtoEditRight").text();
                        var s;
                        if (pVal.indexOf(", " + value) != -1) {
                            s = pVal.replace((", " + value), "");
                        }
                        else if (pVal.length == value.length) { // αν είναι το πρώτο στοιχείο.
                            s = pVal.replace(value, "");
                        }
                        else {
                            s = pVal.replace((value + ", "), "");
                        }
                        $("#lblPreviewFDtoEditRight").text(s);
                    }
                }
                // Εάν δεν είναι κανένα γνώρισμα επιλεγμένο τότε το βέλος δεν θα εμφανίζεται.
                if ($("#lblPreviewFDtoEditLeft").text().length == 0 && $("#lblPreviewFDtoEditRight").text().length == 0) {
                    $("#lblArrow2").text("");
                }
                else {
                    $("#lblArrow2").text("\u2192");
                }
                
            }
            else {

                if (str.indexOf("Left") != -1) // Εάν πρόκειτε για το αριστερό μέλος.
                {
                    if (this.checked) // αν είναι τσεκαρισμένο το checkbox προσθέτω το γνώρισμα στην προεπισκόπηση.
                    {
                        // Ελέγχω αν είναι το πρώτο γνώρισμα ώστε να προσθέσω το κόμα.
                        var l = $("#lblPreviewFDtoCreateLeft").text().length;
                        if (l > 0) {
                            $("#lblPreviewFDtoCreateLeft").append(", " + value);
                        }
                        else {
                            $("#lblPreviewFDtoCreateLeft").append(value);
                        }
                    }
                    else // αν δεν είναι τσεκαρισμένο αφαιρώ το γνώρισμα από την προεπισκόπηση ελέγχοντας τη θέση του για το κόμα.
                    {
                        var pVal = $("#lblPreviewFDtoCreateLeft").text();
                        var s;
                        if (pVal.indexOf(", " + value) != -1) {
                            s = pVal.replace((", " + value), "");
                        }
                        else if (pVal.length == value.length) { // αν είναι το πρώτο στοιχείο.
                            s = pVal.replace(value, "");
                        }
                        else {
                            s = pVal.replace((value + ", "), "");
                        }
                        $("#lblPreviewFDtoCreateLeft").text(s);
                    }
                }
                else {// Εάν πρόκειτε για το δεξί μέλος.
                    if (this.checked) {  // αν είναι τσεκαρισμένο το checkbox προσθέτω το γνώρισμα στην προεπισκόπηση.

                        // Ελέγχω αν είναι το πρώτο γνώρισμα ώστε να προσθέσω το κόμα.
                        var l = $("#lblPreviewFDtoCreateRight").text().length;
                        if (l > 0) {
                            $("#lblPreviewFDtoCreateRight").append(", " + value); // προσθέτω την τιμή στην προεπισκόπηση.
                        }
                        else {
                            $("#lblPreviewFDtoCreateRight").append(value);
                        }
                    }
                    else { // αν δεν είναι τσεκαρισμένο αφαιρώ το γνώρισμα από την προεπισκόπηση ελέγχοντας τη θέση του για το κόμα.
                        var pVal = $("#lblPreviewFDtoCreateRight").text();
                        var s;
                        if (pVal.indexOf(", " + value) != -1) {
                            s = pVal.replace((", " + value), "");
                        }
                        else if (pVal.length == value.length) {  // αν είναι το πρώτο στοιχείο.
                            s = pVal.replace(value, "");
                        }
                        else {
                            s = pVal.replace((value + ", "), "");
                        }
                        $("#lblPreviewFDtoCreateRight").text(s);
                    }
                }
                // Εάν δεν είναι κανένα γνώρισμα επιλεγμένο τότε το βέλος δεν θα εμφανίζεται.
                if ($("#lblPreviewFDtoCreateRight").text().length == 0 && $("#lblPreviewFDtoCreateLeft").text().length == 0) {
                    $("#lblArrow").text("");
                }
                else {
                    $("#lblArrow").text("\u2192");
                }
                
            }
        });

        // Αντί να βάλω listeners στο κάθε GridView ξεχωριστά, βάζω έναν και αναγνωρίζω σε ποιο απευθύνεται.     
        $("table tr").click(function () {
            var selected = $(this).hasClass("highlight");
            var gridView = $(this).closest("table").attr("id");
            var hiddenField = gridView.concat("HiddenField");

            if (gridView == "gridViewRelation" || gridView == "gridViewFD") {

                $("#" + gridView + " tr").removeClass("highlight");
                if (!selected) {
                    $(this).addClass("highlight");
                    $("#" + hiddenField).val($(this).index() - 1);
                }
                else {
                    $("#" + hiddenField).val(-3);
                }
            }
        });

        // Κατά την φόρτωση (και επαναφόρτωση) της σελίδας, χρωματίζεται η τελευταία επιλεγμένη γραμμή των gridView
        $(document).ready(function () {
            var rowId = parseInt($("#gridViewRelationHiddenField").val()) + 2;
            $("#gridViewRelation tr:nth-child(" + rowId + ")").addClass("highlight");

            var rowId = parseInt($("#gridViewFDHiddenField").val()) + 2;
            $("#gridViewFD tr:nth-child(" + rowId + ")").addClass("highlight");
        });

        // Τα elements αυτής της κλάσης, στο κλικ προκαλούν την εμφάνιση του Loader. Αποκρύπτεται αυτόματα κατά την ολοκλήρωση του PostBack.   
        $(".showLoader").click(function () {
            $("#loader").removeClass("hide-loader");
        });

    </script>

    <!-- Script για Analytics -->
    <script>
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
      (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
      m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
      })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

      ga('create', 'UA-104919102-1', 'auto');
      ga('send', 'pageview');

    </script>

</body>
</html>
