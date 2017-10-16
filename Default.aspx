<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>BCNF</title>
    <link runat="server" rel="shortcut icon" href="favicon.ico" type="image/x-icon"/>
    <link runat="server" rel="icon" href="favicon.ico" type="image/ico"/>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
    <link rel="stylesheet" href="Style/bcnfStyle.css"/>
</head>

<body>

    <form id="form1" runat="server">
       
        <!-- Navigation menu fixed for Admins-->
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
                    <li><a href="Default.aspx"><span class="glyphicon glyphicon-home"></span></a></li>
		            <li><asp:Button ID="btnSaveSchema" class="btn btn-success btn-sm headerButtons" runat="server" Text="Αποθήκευση Σχήματος" OnClick="btnSaveSchema_Click"  /></li>
		            <li><asp:Button ID="btnSetDefaultSchema" class="btn btn-success btn-sm headerButtons showLoader" runat="server" Text="Επιλογή Προεπιλεγμένου" OnClick="btnSetDefaultSchemaSelect" /></li>
		            <li><asp:Button class="btn btn-success btn-sm headerButtons" ID="StatisticsHyperLink" runat="server" PostBackUrl="~/MemberPages/Statistics.aspx"  Text="Εμφάνιση στατιστικών"/></li>
                    <li><asp:Button ID="AdminPageHyperLink" class="btn btn-success btn-sm headerButtons" runat="server" Text="Admin Page" PostBackUrl="~/MemberPages/Admin.aspx" /></li>                
		        </ul>
		        
                <!-- Admin Login Status -->
                <ul class="nav navbar-nav navbar-right">
		            <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                        <asp:LoginName ID="LoginName1" runat="server" />
                        </a>
		            </li>
		            <li id="lgout">
                        <span class="glyphicon glyphicon-log-out"></span> 
                        <asp:LoginStatus ID="LoginStatus1" runat="server"  />
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
				        <li><a href="#"><i class="icon iconClosure"></i> <asp:Button ID="btnNavClosure" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Εγλεισμός" OnClick="btnFindClosureClick" /></a></li>
				        <li><a href="#"><i class="icon iconKeys"></i> <asp:Button ID="btnNavKeys" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Κλειδιά" OnClick="btnCalculateKeysClick" /></a></li>
				        <li><a href="#"><i class="icon iconDecompose"></i> <asp:Button ID="btnNavDecompose" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Διάσπαση" OnClick="btnDecomposeClick" /></a></li>
                        <li><a href="#"><i class="icon iconStepsDecompose"></i> <asp:Button ID="btnNavStepsDecompose" class="btn btn-success btn-sm navDropDownbtn showLoader" runat="server" Text="Σταδιακή Διάσπαση" OnClick="btnStepsDecomposeClick" /></a></li>
			          </ul>
			        </li>
		            <li class="dropdown">
			          <a class="dropdown-toggle" data-toggle="dropdown" href="#">Σχετικά <span class="caret"></span></a>
			          <ul class="dropdown-menu">
				        <li><a id="instructions" runat="server" onserverclick="btnAboutInstructions"><i class="icon iconInfo"></i> Οδηγίες Χρήσης</a></li>
				        <li><a id="diplomatic" runat="server" onserverclick="btnAboutDiplomatic"><i class="icon iconDiplomatic"></i> Η εργασία</a></li>
				        <li><a id="github" runat="server" onserverclick="btnAboutGithub"><i class="icon iconContribute"></i> Συνεισφορά</a></li>
			          </ul>
			        </li>
		            <li><a id="contactLink" href="#">Επικοινωνία</a></li>
		        </ul>
		        
                <!-- Admin Login Status -->
                <ul class="nav navbar-nav navbar-right">
		            <li><a href="#"><span class="glyphicon glyphicon-user"></span>
                        <asp:LoginName ID="LoginName2" runat="server" />
                        </a>
		            </li>
		            <li id="lgoutUser">
                        <span class="glyphicon glyphicon-log-out"></span> 
                        <asp:LoginStatus ID="LoginStatus2" runat="server"  />
		            </li>
		        </ul>

	        </div>
	        </div>
        </nav>

        <!-- Περιεχόμενο σελίδας -->
        <div class="container">

            <div class="loader hide-loader" id="loader"></div>

            
            <%-- HEADER (τίτλο, όνομα κλπ) --%>
            <div class="page-header">
                <h1>Σχήμα:
                    <asp:Label ID="lblSchemaName" runat="server" Text="Default"></asp:Label>
                    <asp:Label ID="lblSchemaId" runat="server" Text="" Hidden ="true"></asp:Label>
                    <small>γνωρίσματα και συναρτησιακές εξαρτήσεις.</small></h1>
                <h5><asp:Label ID="lblSchemaDescription" runat="server" Text="" Font-Italic="True" ForeColor="#669999"></asp:Label></h5>
            </div>

            <%-- ROW με Επιλογές Ενεργειών --%>
            <div class="row">

                <%--Εγκλεισμός (Επιλογή)--%>
                <div class="col-md-3">

                    <asp:Button class="btn btn-info btn-lg" ID="btnFindClosure" runat="server" Text="Εγκλεισμός" OnClick="btnFindClosureClick" />

                    <!-- Modal εύρεσης εγκλεισμού -->
                    <div class="modal fade" id="modalClosure" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Υπολογισμός Εγκλεισμού</h4>
                                </div>
                                <div class="modal-body">

                                   <p><strong>Επιλογή γνωρισμάτων </strong></p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-10">
                                            <asp:GridView ID="gridViewFindClosure" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                                 runat="server" AutoGenerateColumns="false" Width="100%">
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxFindClosure" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Name" HeaderText="Όνομα" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%"/>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                        
                                    </div>

                                    <div class="row">
                                        <div class="col-md-10">
                                            <br />
                                            <h6><strong>Επιλέξτε τα γνωρίσματα που θέλετε να υπολογιστεί ο εγκλεισμός τους.</strong></h6>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnClosureOK" Text="OK" class="btn btn-default btn-success showLoader" OnClick="btnCalculateClosureClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->
                </div>

                <%--Υποψήφια κλειδιά (Επιλογή)--%>
                <div class="col-md-3">

                    <asp:Button class="btn btn-info btn-lg showLoader" ID="btnCalculateKeys" runat="server" Text="Υποψήφια κλειδιά" OnClick="btnCalculateKeysClick" />

                </div>

                <%--Διάσπαση BCNF (Επιλογή)--%>
                <div class="col-md-3">

                    <asp:Button class="btn btn-info btn-lg showLoader" ID="btnDecompose" runat="server" Text="Διάσπαση BCNF" OnClick="btnDecomposeClick" />

                </div>

                <%--Σταδιακή διάσπαση BCNF (Επιλογή)--%>
                <div class="col-md-3">
                    <!--   <button type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalKeys">Σταδιακή διάσπαση BCNF</button> -->
                    <asp:Button class="btn btn-info btn-lg showLoader" ID="Button9" runat="server" Text="Σταδιακή διάσπαση BCNF" OnClick="btnStepsDecomposeClick" />
                </div>

            </div>

            <br />

            <%-- ROW με Grid Γνωρισμάτων και Συναρτησιακών εξαρτήσεων. --%>
            <div class="row">

                <%--Γνωρίσματα--%>
                <div class="col-md-6">
                <p><b>Γνωρίσματα </b></p>

                <asp:GridView CssClass="" ID="gridViewAttr" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                     runat="server" AutoGenerateColumns="false"  Width="100%" >

                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText=" Όνομα" ItemStyle-Width="55%"/>
                        <asp:BoundField DataField="Description" HeaderText=" Τύπος \ Περιγραφή" HeaderStyle-CssClass="text-center" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center"/>
                    </Columns>

                </asp:GridView>
                <asp:HiddenField ID="gridViewAttrHiddenField" runat="server" Value="-3" />

                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/add new.jpg" Visible="False" />

                <br />

                <!-- Buttons -->
                <div style="text-align: right; width: 100%;">
                    <asp:Button class="btn btn-info btn-lg showLoader" ID="btnAddAttr" runat="server" Text="Προσθήκη" OnClick="btnNewAttrClick" />
                    <asp:Button class="btn btn-info btn-lg showLoader" ID="btnEditAttr" runat="server" Text="Επεξεργασία" OnClick="btnEditAttrClick" />
                    <asp:Button class="btn btn-info btn-lg showLoader" ID="btnDeleteAttr" runat="server" Text="Διαγραφή" OnClick="btnDeleteAttrClick" />
                </div>

                <!-- Modal νέο γνώρισμα-->
                <div class="modal fade" id="modalNewAttribute" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Νέο γνώρισμα</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="control-label col-md-6" for="tbxNewAttrName">Εισάγετε όνομα για το νέο γνώρισμα.</label>
                                        <div class="col-md-6"> 
                                            <asp:TextBox ID="tbxNewAttrName" runat="server" placeholder="Όνομα γνωρίσματος"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">    
                                        <label class="control-label col-md-6" for="tbxNewAttrType">Εισάγετε τύπο για το νέο γνώρισμα.</label>
                                        <div class="col-md-6"> 
                                            <asp:TextBox ID="tbxNewAttrType" runat="server" placeholder="Τύπος γνωρίσματος"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div>
                                        <br />
                                        <h6>Για εισαγωγή πολλαπλών γνωρισμάτων χρησιμοποιείστε ώς διαχωριστικό το κόμμα (,) <br />
                                            Για παράδειγμα A, B, C
                                        </h6>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSaveAttr" Text="Αποθήκευση" class="btn btn-default btn-success showLoader" OnClick="btnNewAttrOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->

                <!-- Modal επεξεργασία γνωρίσματος -->
                <div class="modal fade" id="modalEditAttribute" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Επεξεργασία γνωρίσματος</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="control-label col-md-6" for="tbxEditAttrName">Εισάγετε όνομα για το γνώρισμα.</label>
                                        <div class="col-md-6"> 
                                            <asp:TextBox ID="tbxEditAttrName" runat="server" placeholder="Όνομα γνωρίσματος"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">    
                                        <label class="control-label col-md-6" for="tbxEditAttrType">Εισάγετε τύπο για το γνώρισμα.</label>
                                        <div class="col-md-6"> 
                                            <asp:TextBox ID="tbxEditAttrType" runat="server" placeholder="Τύπος γνωρίσματος"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnEditAttrOK" Text="Ενημέρωση" class="btn btn-default btn-success showLoader" OnClick="btnEditAttrΟΚClick" UseSubmitBehavior="false" data-dismiss="modal" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->
                </div>

                <%--Συναρτησιακές εξαρτήσεις--%>
                <div class="col-md-6">
                    <p><b>Συναρτησιακές εξαρτήσεις </b></p>

                   <%--  <asp:ListBox ID="lboxFD" runat="server" Rows="10" Width="100%"></asp:ListBox> --%>

                    <asp:GridView ID="gridViewFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        runat="server" AutoGenerateColumns="false" Width="100%">
                       
                         <Columns>
                            <asp:BoundField DataField="Description" HeaderText=" Περιγραφή" ItemStyle-Width="80%" />
                            <asp:BoundField DataField="Trivial" HeaderText=" Τετ" HeaderStyle-CssClass="text-center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                        </Columns>


                    </asp:GridView>

                    <asp:HiddenField ID="gridViewFDHiddenField" runat="server" Value="-3" />
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/add new.jpg" Visible="False" />

                    <br />

                    <!-- Buttons -->
                    <div style="text-align: right; width: 100%;">
                       <!-- <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalNewFD">+</button> -->
                       <!-- <asp:Button class="btn btn-info btn-lg" ID="Button3" runat="server" Text="-" OnClick="btnDeleteFDClick" /> -->

                        <asp:Button class="btn btn-info btn-lg showLoader" ID="btnNewFD" runat="server" Text="Προσθήκη" OnClick="btnNewFDClick" />
                        <asp:Button class="btn btn-info btn-lg showLoader" ID="btnEditFD" runat="server" Text="Επεξεργασία" OnClick="btnEditFDClick" />
                        <asp:Button class="btn btn-info btn-lg showLoader" ID="btnDeleteFD" runat="server" Text="Διαγραφή" OnClick="btnDeleteFDClick" />
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

                                   <p><strong>Δομή συναρτησιακής εξάρτησης</strong></p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewLeftFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                                runat="server" AutoGenerateColumns="false" Width="100%">
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxLeftFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Orizouses" HeaderText="Ορίζουσες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%"/>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewLeftFDHiddenField" runat="server" Value="-3"/>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewRightFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                                runat="server" AutoGenerateColumns="false" Width="100%" >
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxRightFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Eksartimenes" HeaderText="Εξαρτημένες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%" />
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewRightFDHiddenField" runat="server" Value="-3"/>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-10">
                                            <br />
                                            <p><strong>Τελική μορφή συναρτησιακής εξάρτησης: </strong></p>
                                            <asp:Label ID="lblPreviewFDtoCreateLeft" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblArrow" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblPreviewFDtoCreateRight" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnNewFDOK" Text="Αποθήκευση" class="btn btn-default btn-success showLoader" OnClick="btnNewFDOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
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

                                   <p><strong>Δομή συναρτησιακής εξάρτησης: </strong> </p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewEditLeftFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                                 runat="server" AutoGenerateColumns="false" Width="100%">
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxEditLeftFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Orizouses" HeaderText="Ορίζουσες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%"/>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewEditLeftFDHiddenField" runat="server" Value="-3"/>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:GridView ID="gridViewEditRightFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                                runat="server" AutoGenerateColumns="false" Width="100%" >
                       
                                                 <Columns>
                                                     <asp:templatefield HeaderText="Επιλογή" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                        <itemtemplate>
                                                            <asp:checkbox ID="checkBoxEditRightFD" runat="server"></asp:checkbox>
                                                        </itemtemplate>
                                                    </asp:templatefield>
                                                    <asp:BoundField DataField="Eksartimenes" HeaderText="Εξαρτημένες" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80%" />
                                                </Columns>

                                            </asp:GridView>
                                            <asp:HiddenField ID="gridViewEditRightFDHiddenField" runat="server" Value="-3"/>

                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-10">
                                            <br />
                                            <p><strong>Τελική μορφή συναρτησιακής εξάρτησης: </strong></p>
                                            <asp:Label ID="lblPreviewFDtoEditLeft" runat="server" CssClass="previewFD" Text=""></asp:Label>
                                            <asp:Label ID="lblArrow2" runat="server" CssClass="previewFD" Text=""></asp:Label>
                                            <asp:Label ID="lblPreviewFDtoEditRight" runat="server" CssClass="previewFD" Text=""></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnEditFDOK" Text="Ενημέρωση" class="btn btn-default btn-success showLoader" OnClick="btnEditFDΟΚClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->

                    
                </div>

            </div>

            <%-- ROW με Logging Console και New, Load Button --%>
            <div class="row">

                <%-- Logging Console --%>
                <div class="col-md-6">
                    <br />
                    <button type="button" id="btnOpenResultsModal" class="btn btn-success btn-lg"  data-toggle="modal" data-target="#modalResults">Παράθυρο αποτελεσμάτων</button>

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

                    <!-- Modal Αποτελέσματα -->
                    <div class="modal fade" id="modalResults" role="dialog">
                        <div class="modal-dialog" runat="server" id="resultModalSize">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <asp:Label class="modal-title" ID="resultTitle" runat="server" Text="Αποτελέσματα" Font-Size="16px"></asp:Label>
                                </div>
                                <div class="modal-body myModalBody">
                                    <div class="form-horizontal">  
                                        <textarea runat="server" id="log" class="form-control myForm" rows="15" ></textarea>
                                        <asp:Label ID="lblAlter" runat="server" Text="Εναλλακτική" Visible="false" ForeColor="Blue" style="margin-top:10px;" Font-Bold="True" Font-Italic="True"></asp:Label>
                                        <textarea runat="server" id="logAlter" class="form-control myForm" rows="15" Visible="false" ></textarea>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnDecomposeAlternative" Text="Εναλλακτική" class="btn btn-default showLoader" OnClick="btnDecomposeClick" UseSubmitBehavior="false" Visible="false" Style="float:left;" />
                                    <button type="button" class="btn btn-default btn-success" data-dismiss="modal">ΟΚ</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal -->

                </div>

                <%-- Load Button --%>
                <div class="col-md-6">
                    <div>
                       <br />
                        <asp:Button ID="btnNewSchema" class="btn btn-success btn-lg showLoader" runat="server" Text="Νέο Σχήμα" OnClick="btnNewSchemaClick"/>
                        
                        <!-- Modal νέο σχήμα -->
                        <div class="modal fade" id="modalNewSchema" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Νέο σχήμα</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal"> 
                                            <div class="form-group">
                                                <label class="control-label col-md-6" for="tbxNewSchemaName">Εισάγετε όνομα για το νέο σχήμα:</label>
			                                    <div class="col-md-6"> 
				                                    <asp:TextBox ID="tbxNewSchemaName" runat="server" placeholder="Όνομα σχήματος"></asp:TextBox>
			                                    </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-md-6" for="tbxNewSchemaDescription">Εισάγετε περιγραφή για το νέο σχήμα:</label>
			                                    <div class="col-md-6"> 
				                                    <asp:TextBox ID="tbxNewSchemaDescription" runat="server" placeholder=" Περιγραφή σχήματος"></asp:TextBox>
			                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btnNewSchemaOK" Text="Δημιουργία" class="btn btn-default btn-success showLoader" OnClick="btnNewSchemaOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal -->

                        <asp:Button ID="btnLoadSchema" class="btn btn-success btn-lg showLoader" runat="server" Text="Φόρτωση Σχήματος" OnClick="btnLoadSchema_Click"  />

                        <!-- Modal φόρτωση σχήματος-->
                        <div class="modal fade" id="loadSchemaModal" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Φόρτωση σχήματος</h4>
                                    </div>
                                    <div class="modal-body">
                                        <p>Επιλέξτε παράδειγμα για φόρτωση</p>
                                        <asp:DropDownList ID="schemaLoadDropDownList" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btnLoadSchemaClick" Text="OK" class="btn btn-default btn-success showLoader" OnClick="btnLoadSelectedSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal -->

                        
                    </div>
                </div>

            </div>
            
            <!-- Modal επιλογή προεπιλεγμένου σχήματος-->
            <div class="modal fade" id="SetDefaultSchemaModal" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Επιλογή Προεπιλεγμένου σχήματος</h4>
                        </div>
                        <div class="modal-body">
                            <p><strong>Διαλέξτε ένα παράδειγμα για προεπιλεγμενο στην εφαρμογή:</strong></p>
                            <asp:DropDownList ID="SetSchemaDefaultDropDownList" runat="server"></asp:DropDownList>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSetSchema" Text="SET" class="btn btn-default btn-success showLoader" OnClick="btnSetDefaultSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal -->

            <!-- Modal Επικοινωνία -->
            <div class="modal fade" id="modalContact" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Επικοινωνία</h4>
                        </div>
                        <div class="modal-body">
                            <p><strong>Για οποιαδήποτε απορία ή για αναφορά σφάλματος ή για προτάσεις βελτίωσης, μπορείτε να επικοινωνήσετε στα παρακάτω email: </strong></p>
                            <asp:Label ID="Label1" runat="server" Text="lefterisxris@gmail.com" Font-Bold="True"  ForeColor="Blue" Font-Size="Medium"></asp:Label> 
                            <br /> 
                            <asp:Label ID="Label2" runat="server" Text="gkoloniari@uom.edu.gr" Font-Size="Medium" Font-Bold="True" ForeColor="Blue"></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default btn-success" data-dismiss="modal">ΟΚ</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal -->

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
        
        // Καλείται κατά την οποιαδήποτε αλλαγή σε οποιοδήποτε checkbox για την αναπαράσταση της FD
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

        // Κατά την φόρτωση (και επαναφόρτωση) της σελίδας, χρωματίζεται η τελευταία επιλεγμένη γραμμή των gridView
        $(document).ready(function () {
            var rowId = parseInt($("#gridViewAttrHiddenField").val()) + 2;
            $("#gridViewAttr tr:nth-child(" + rowId + ")").addClass("highlight");

            var rowId = parseInt($("#gridViewFDHiddenField").val()) + 2;
            $("#gridViewFD tr:nth-child(" + rowId + ")").addClass("highlight");
        });
    
        // Τα elements αυτής της κλάσης, στο κλικ προκαλούν την εμφάνιση του Loader. Αποκρύπτεται αυτόματα κατά την ολοκλήρωση του PostBack.   
        $(".showLoader").click(function () {
            $("#loader").removeClass("hide-loader");
        });

        // Αντί να βάλω listeners στο κάθε GridView ξεχωριστά, βάζω έναν και αναγνωρίζω σε ποιο απευθύνεται.     
        $("table tr").click(function () {
           
            var selected = $(this).hasClass("highlight");
            var gridView = $(this).closest("table").attr("id");
            var hiddenField = gridView.concat("HiddenField");

            if (gridView == "gridViewAttr" || gridView == "gridViewFD") {

                $("#"+ gridView +" tr").removeClass("highlight");
                if (!selected) {
                    $(this).addClass("highlight");
                    $("#" + hiddenField).val($(this).index() - 1);
                }
                else {
                    $("#" + hiddenField).val(-3);
                }
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

        //Εμφάνιση modal Επικοινωνίας
        $("#contactLink").click(function () {
            $('#modalContact').modal();
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
