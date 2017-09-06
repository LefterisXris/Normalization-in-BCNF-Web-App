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
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">

            <%-- HEADER (τίτλο, όνομα κλπ) --%>
            <div class="page-header">
                <%--TODO: Στην δημιουργία σχήματος να περνάς το όνομα εδώ στο label --%>
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
                            runat="server" AutoGenerateColumns="false"  Width="100%"
                            OnRowDataBound="OnRowDataBoundRelation" OnSelectedIndexChanged="OnSelectedIndexChangedRelation">

                            <Columns>
                                <asp:BoundField DataField="BCNF" HeaderText=" BCNF" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="10%"/>
                                <asp:BoundField DataField="Relation" HeaderText="Πίνακες Γνωρισμάτων" ItemStyle-Width="70%" />
                            </Columns>

                        </asp:GridView>
                        
                        <asp:Button ID="btnShowBCNFtables" class="btn btn-info btn-lg" runat="server" Text="Προβολή BCNF πινάκων" Style="float: right;" OnClick="btnShowBCNFTablesClick" />

                    </div>
                    <div style="margin-top: 35px;">

                        <p><b>Συναρτησιακές εξαρτήσεις</b></p>

                        <asp:GridView ID="gridViewFD" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                            runat="server" AutoGenerateColumns="false"  Width="100%"
                            OnRowDataBound="OnRowDataBoundFD" OnSelectedIndexChanged="OnSelectedIndexChangedFD">

                            <Columns>
                                <asp:BoundField DataField="Excluded" HeaderText="Χ" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="10%"/>
                                <asp:BoundField DataField="Description" HeaderText="Περιγραφή" ItemStyle-Width="60%" />
                                <asp:BoundField DataField="Trivial" HeaderText="Τετ"  HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                            </Columns>

                        </asp:GridView>

                        <div class="row" style="float:right;">
                            <asp:Button ID="btnNewFD" class="btn btn-info btn-sm" runat="server" Text="Προσθήκη" OnClick="btnNewFDClick" />
                            <asp:Button ID="btnEditFD" class="btn btn-info btn-sm" runat="server" Text="Επεξεργασία" OnClick="btnEditFDClick" />
                            <asp:Button ID="btnDeleteFD" class="btn btn-info btn-sm" runat="server" Text="Διαγραφή" OnClick="btnDeleteFDClick" />
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

                    <asp:Button ID="btnPreview" class="btn btn-info btn-lg" runat="server" Text="Προεπισκόπηση" OnClick="btnPreview_Click" />

                    <!-- Modal Προεπισκόπησης-->
                    <div class="modal fade" id="modalPreview" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Προεπισκόπηση διάσπασης σε BCNF</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal"> 
                                        <textarea runat="server" id="log" class="form-control" rows="15" style="min-width: 100%; min-height: 100%;" ></textarea>
                                    </div>
                                    
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button1" Text="Κλείσιμο" class="btn btn-default" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal -->


                    <asp:Button ID="btnDecompose" class="btn btn-info btn-lg" runat="server" Text="Διάσπαση" OnClick="btnDecompose_Click" />
                    <asp:Button ID="btnClearResults" class="btn btn-info btn-lg" runat="server" Text="Καθαρισμός" OnClick="btnClearResults_Click" />
                </div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnReset" class="btn btn-info btn-lg" runat="server" Text="Reset" OnClick="btnResetClick" />
                </div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnCloseStepsDecompose" class="btn btn-danger btn-lg" runat="server" Text="Κλείσιμο" Style="width: 100%;" OnClick="btnCloseStepsDecompose_Click" />
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
