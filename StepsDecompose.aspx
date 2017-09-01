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
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
      (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
      m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
      })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

      ga('create', 'UA-104919102-1', 'auto');
      ga('send', 'pageview');

    </script>

</body>
</html>
