<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StepsDecompose.aspx.cs" Inherits="StepsDecompose" %>

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
            </div>

            <%-- ROW (πανελ με πίνακες, αποτελέσματα) --%>
            <div class="row">

                <%-- Στοιχεία σχήματος (πίνακες, συναρτησιακές, κλειδιά) --%>
                <div class="col-md-6 col-sm-6">
                    <div>

                        <p><b>Πίνακες</b></p>
                   
                        <asp:RadioButtonList ID="TablesRadioButtonList" runat="server"></asp:RadioButtonList>
                        <asp:Button ID="btnShowBCNFtables" class="btn btn-info btn-lg" runat="server" Text="Προβολή BCNF πινάκων" Style="float: right;" OnClick="btnShowBCNFtables_Click" />

                    </div>
                    <div style="margin-top: 35px;">

                        <p><b>Συναρτησιακές εξαρτήσεις</b></p>

                        <asp:RadioButtonList ID="FDsRadioButtonList" runat="server"></asp:RadioButtonList>

                    </div>
                    <div style="margin-top: 20px;">
                        <p><b>Υποψήφια κλειδιά</b></p>
                        <!-- <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label> -->
                        <asp:TextBox ID="tbxKeys" runat="server" Width="100%"></asp:TextBox>
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
                    <!--  <button type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalPreview">Προεπισκόπηση</button> -->

                    <!-- Modal Προεπισκόπησης-->
                    <div class="modal fade" id="modalPreview" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Προεπισκόπηση διάσπασης σε BCNF</h4>
                                </div>
                                <div class="modal-body">

                                    <asp:Label ID="lblPreviewResults" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button1" Text="Κλείσιμο" class="btn btn-default" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal -->


                    <asp:Button ID="Button3" class="btn btn-info btn-lg" runat="server" Text="Διάσπαση" OnClick="btnDecompose_Click" />
                    <asp:Button ID="btnClearResults" class="btn btn-info btn-lg" runat="server" Text="Καθαρισμός" OnClick="btnClearResults_Click" />
                </div>
                <div class="col-md-2 col-sm-2"></div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnCloseStepsDecompose" class="btn btn-danger btn-lg" runat="server" Text="Κλείσιμο" Style="width: 100%;" />
                </div>

            </div>

        </div>
    </form>
</body>
</html>
