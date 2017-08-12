<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BCNF</title>
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
                    <small>γνωρίσματα και συναρτησιακές εξαρτήσεις. </small></h1>
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

                                   <h2>Επιλογή γνωρισμάτων</h2>
                                   
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

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnCalculateClosure" Text="OK" class="btn btn-default" OnClick="btnCalculateClosureClick" UseSubmitBehavior="false" data-dismiss="modal" />
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
                                            Τελική μορφή συναρτησιακής εξάρτησης
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

                                   <p>Δομή συναρτησιακής εξάρτησης</p>
                                   
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
                                            Τελική μορφή συναρτησιακής εξάρτησης
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
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                    <textarea runat="server" id="log" cols="40" rows="20"></textarea>
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
                                        <p>Εισάγετε όνομα για το νέο σχήμα:</p>
                                        <asp:TextBox ID="tbxNewSchemaName" runat="server" placeholder="Όνομα σχήματος"></asp:TextBox>
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
                        <asp:Button ID="btnSaveSchema" class="btn btn-success btn-lg" runat="server" Text="Αποθήκευση Σχήματος" OnClick="btnSaveSchema_Click"  />
                    
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
                    </div>
                </div>

            </div>

        </div>
    </form>

    <script>/* <%--
        $("#lblSchemaName").dblclick(function () {
            var txt = $("#lblSchemaName").text();
            $("#lblSchemaName").replaceWith("<input id='lblSchemaName'/>");
            $("#lblSchemaName").val(txt);
        });

        $("#lblSchemaName1").blur(function () {
            var txt = $(this).val();
            $(this).replaceWith("<asp:Label ID='lblSchemaName' runat='server'></asp:Label>");
            $("#lblSchemaName").text(txt);
        });*/--%>
    </script>
</body>
</html>
