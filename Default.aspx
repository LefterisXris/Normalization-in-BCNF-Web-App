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
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">

            <%-- HEADER (τίτλο, όνομα κλπ) --%>
            <div class="page-header">
                <h1>Νέο σχήμα:
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

                                   <p>Επιλογή γνωρισμάτων</p>
                                   
                                    <div class="form-horizontal">
                                        <div class="col-md-10">
                                            <asp:GridView ID="gridViewFindClosure" runat="server" AutoGenerateColumns="false" Width="100%">
                       
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
                                            Επιλέξτε τα γνωρίσματα που θέλετε για να υπολογιστεί ο εγκλεισμός τους.
                                        </div>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button1" Text="OK" class="btn btn-default" OnClick="btnCalculateClosureClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->
                </div>

                <%--Υποψήφια κλειδιά (Επιλογή)--%>
                <div class="col-md-3">

                    <asp:Button class="btn btn-info btn-lg" ID="btnCalculateKeys" runat="server" Text="Υποψήφια κλειδιά" OnClick="btnCalculateKeysClick" />

                </div>

                <%--Διάσπαση BCNF (Επιλογή)--%>
                <div class="col-md-3">

                    <asp:Button class="btn btn-info btn-lg" ID="Button11" runat="server" Text="Διάσπαση BCNF" OnClick="btnDecomposeClick" />

                </div>

                <%--Σταδιακή διάσπαση BCNF (Επιλογή)--%>
                <div class="col-md-3">
                    <!--   <button type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalKeys">Σταδιακή διάσπαση BCNF</button> -->
                    <asp:Button class="btn btn-info btn-lg" ID="Button9" runat="server" Text="Σταδιακή διάσπαση BCNF" OnClick="btnStepsDecomposeClick" />
                </div>

            </div>

            <br />

            <%-- ROW με panel Γνωρισμάτων και Συναρτησιακών εξαρτήσεων. --%>
            <div class="row">

                <%--Γνωρίσματα--%>
                    <div class="col-md-6">
                    <p><b>Γνωρίσματα </b></p>

                    <asp:GridView ID="gridViewAttr" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        runat="server" AutoGenerateColumns="false"  Width="100%"
                        OnRowDataBound="OnRowDataBoundAttr" OnSelectedIndexChanged="OnSelectedIndexChangedAttr">

                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText=" Όνομα" ItemStyle-Width="55%"/>
                            <asp:BoundField DataField="Description" HeaderText=" Τύπος \ Περιγραφή" HeaderStyle-CssClass="text-center" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center"/>
                        </Columns>

                    </asp:GridView>

                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/add new.jpg" Visible="False" />

                    <!-- Buttons -->
                    <div style="text-align: right; width: 100%;">
                        <asp:Button class="btn btn-info btn-lg" ID="btnAddAttr" runat="server" Text="Προσθήκη" OnClick="btnNewAttrClick" />
                        <asp:Button class="btn btn-info btn-lg" ID="btnEditAttr" runat="server" Text="Επεξεργασία" OnClick="btnEditAttrClick" />
                        <asp:Button class="btn btn-info btn-lg" ID="btnDeleteAttr" runat="server" Text="Διαγραφή" OnClick="btnDeleteAttrClick" />
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
                                            <h6>Για εισαγωγή πολλαπλών γνωρισμάτων χρησιμοποιείστε ώς διαχωριστικό το κόμμα (,) <br />
                                                Για παράδειγμα A, B, C
                                            </h6>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnSaveAttr" Text="OK" class="btn btn-default" OnClick="btnNewAttrOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
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
                                    <asp:Button runat="server" ID="btnEditAttrOK" Text="Ενημέρωση" class="btn btn-default" OnClick="btnEditAttrΟΚClick" UseSubmitBehavior="false" data-dismiss="modal" />
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
                        runat="server" AutoGenerateColumns="false" Width="100%"
                        OnRowDataBound="OnRowDataBoundFD" OnSelectedIndexChanged="OnSelectedIndexChangedFD">
                       
                         <Columns>
                            <asp:BoundField DataField="Description" HeaderText=" Περιγραφή" ItemStyle-Width="80%" />
                            <asp:BoundField DataField="Trivial" HeaderText=" Τετ" HeaderStyle-CssClass="text-center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                        </Columns>

                    </asp:GridView>

                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/add new.jpg" Visible="False" />

                    <!-- Buttons -->
                    <div style="text-align: right; width: 100%;">
                       <!-- <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalNewFD">+</button> -->
                       <!-- <asp:Button class="btn btn-info btn-lg" ID="Button3" runat="server" Text="-" OnClick="btnDeleteFDClick" /> -->

                        <asp:Button class="btn btn-info btn-lg" ID="btnNewFD" runat="server" Text="Προσθήκη" OnClick="btnNewFDClick" />
                        <asp:Button class="btn btn-info btn-lg" ID="btnEditFD" runat="server" Text="Επεξεργασία" OnClick="btnEditFDClick" />
                        <asp:Button class="btn btn-info btn-lg" ID="btnDeleteFD" runat="server" Text="Διαγραφή" OnClick="btnDeleteFDClick" />
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
                                    <asp:Button runat="server" ID="btnNewFDOK" Text="OK" class="btn btn-default" OnClick="btnNewFDOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
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
                                    <asp:Button runat="server" ID="btnEditFDOK" Text="Ενημέρωση" class="btn btn-default" OnClick="btnEditFDΟΚClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--Modal -->

                    
                </div>

            </div>

            <%-- ROW με Logging Console και Load Button --%>
            <div class="row">

                <%-- Logging Console --%>
                <div class="col-md-6">
                    <button type="button" id="btnOpenResultsModal" class="btn btn-info btn-lg"  data-toggle="modal" data-target="#modalResults">Results</button>

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
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Αποτελέσματα</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal"> 
                                        <textarea runat="server" id="log" class="form-control" rows="15" style="min-width: 100%; min-height: 100%;" ></textarea>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal -->

                </div>

                <%-- Load Button --%>
                <div class="col-md-6">
                    <div>
                       
                         <asp:Button ID="btnNewSchema" class="btn btn-success btn-lg" runat="server" Text="Νέο Σχήμα" OnClick="btnNewSchemaClick"/>
                        
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
                                        <asp:Button runat="server" ID="btnNewSchemaOK" Text="Δημιουργία" class="btn btn-default" OnClick="btnNewSchemaOKClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal -->

                    </div>
                    <div>
                        <asp:Button ID="btnLoadSchema" class="btn btn-success btn-lg" runat="server" Text="Φόρτωση Σχήματος" OnClick="btnLoadSchema_Click"  />
                        <br />
                        <asp:Button ID="btnSaveSchema" class="btn btn-success btn-lg" runat="server" Text="Αποθήκευση Σχήματος" OnClick="btnSaveSchema_Click"  />
                        <br />
                        <asp:Button ID="btnSetDefaultSchema" class="btn btn-success btn-lg" runat="server" Text="Επιλογή Προεπιλεγμένου" OnClick="btnSetDefaultSchemaSelect" />
                    
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
                                        <asp:Button runat="server" ID="btnLoadSchemaClick" Text="OK" class="btn btn-default" OnClick="btnLoadSelectedSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal -->

                        <!-- Modal επιλογή προεπιλεγμένου σχήματος-->
                        <div class="modal fade" id="SetDefaultSchemaModal" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Επιλογή Προεπιλεγμένου σχήματος</h4>
                                    </div>
                                    <div class="modal-body">
                                        <p>Διαλέξτε ένα παράδειγμα για προεπιλεγμενο</p>
                                        <asp:DropDownList ID="SetSchemaDefaultDropDownList" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btnSetSchema" Text="SET" class="btn btn-default" OnClick="btnSetDefaultSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal -->


                        <br />
                        <asp:LoginStatus ID="LoginStatus1" runat="server" />
                        <br />
                        <asp:Button ID="btnLoadDB" class="btn btn-success btn-lg" runat="server" Text="Paradeigmata"  OnClick="btnGetSchemasClick" />
                    </div>
                </div>

            </div>

            

        </div>
    </form>

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

        function HideLabel() {
        var seconds = 5;
        setTimeout(function () {
            //document.getElementById("#alertBoxSuccess").style.display = "none";
            document.getElementById("<%=lblSchemaName.ClientID %>").style.display = "none";
        }, seconds * 1000);
    };

    </script>
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
