<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
                <h1>Νέο σχήμα:
                    <asp:Label ID="lblSchemaName" runat="server" Text="Default"></asp:Label>
                    <small>γνωρίσματα και συναρτησιακές εξαρτήσεις. </small></h1>
            </div>

            <%-- ROW με Επιλογές Ενεργειών --%>
            <div class="row">

                <%--Εγκλεισμός (Επιλογή)--%>
                <div class="col-md-3">
                    <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalEglismos">Εγκλεισμός</button>

                    <!-- Modal εγκλεισμός-->
                    <div class="modal fade" id="modalEglismos" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Εγκλεισμός γνωρισμάτων</h4>
                                </div>
                                <div class="modal-body">

                                    <p>Επιλογή γνωρισμάτων</p>
                                    <asp:CheckBoxList ID="ClosureCheckBoxList" runat="server"></asp:CheckBoxList>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button4" Text="OK" class="btn btn-default" OnClick="btnCalculateClosureClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Modal--%>
                </div>

                <%--Υποψήφια κλειδιά (Επιλογή)--%>
                <div class="col-md-3">
                    <!--   <button type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalKeys">Υποψήφια κλειδιά</button> -->
                    <asp:Button class="btn btn-info btn-lg" ID="Button10" runat="server" Text="Υποψήφια κλειδιά" OnClick="btnCalculateKeysClick" />

                    <!-- Modal Υποψήφια κλειδιά-->
                    <%--TODO: Διαγραφή?? --%>
                    <div class="modal fade" id="modalKeys" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Υποψήφια κλειδιά</h4>
                                </div>
                                <div class="modal-body">

                                    <p>Click OK για υποψήφια κλειδιά. </p>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button5" Text="OK" class="btn btn-default" OnClick="btnCalculateKeysClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Modal--%>
                </div>

                <%--Διάσπαση BCNF (Επιλογή)--%>
                <div class="col-md-3">
                    <!--  <button type="button"  class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalDecompose">Διάσπαση BCNF</button> -->
                    <asp:Button class="btn btn-info btn-lg" ID="Button11" runat="server" Text="Διάσπαση BCNF" OnClick="btnDecomposeClick" />

                    <!-- Modal Διάσπαση BCNF-->
                    <%--TODO: Διαγραφή?? --%>
                    <div class="modal fade" id="modalDecompose" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Διάσπαση BCNF</h4>
                                </div>
                                <div class="modal-body">

                                    <p>Click OK για Διάσπαση BCNF. </p>

                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button8" Text="OK" class="btn btn-default" OnClick="btnDecomposeClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Modal--%>
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
                    <p>Γνωρίσματα </p>

                    <asp:ListBox ID="lboxAttr" runat="server" Rows="10" Width="100%"></asp:ListBox>
                    <div style="text-align: right; width: 100%;">
                        <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalNewAttribute">+</button>
                        <asp:Button class="btn btn-info btn-lg" ID="Button2" runat="server" Text="-" OnClick="btnDeleteAttrClick" />
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

                                    <p>Εισάγετε όνομα για το νέο γνώρισμα.</p>
                                    <asp:TextBox ID="tbxNewAttrName" runat="server" placeholder="Όνομα γνωρίσματος"></asp:TextBox>
                                    <p>Εισάγετε τύπο για το νέο γνώρισμα.</p>
                                    <asp:TextBox ID="tbxNewAttrType" runat="server" placeholder="Τύπος γνωρίσματος"></asp:TextBox>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnSaveImage" Text="OK" class="btn btn-default" OnClick="btnNewAttrClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Modal--%>
                </div>

                <%--Συναρτησιακές εξαρτήσεις--%>
                <div class="col-md-6">
                    <p>Συναρτησιακές εξαρτήσεις </p>

                    <asp:ListBox ID="lboxFD" runat="server" Rows="10" Width="100%"></asp:ListBox>

                    <div style="text-align: right; width: 100%;">
                        <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#modalNewFD">+</button>
                        <asp:Button class="btn btn-info btn-lg" ID="Button3" runat="server" Text="-" OnClick="btnDeleteFDClick" />
                    </div>

                    <!-- Modal νέα συναρτησιακή εξάρτηση-->
                    <div class="modal fade" id="modalNewFD" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Νέα συναρτησιακή εξάρτηση</h4>
                                </div>
                                <div class="modal-body">

                                    <p>Δομή συναρτησιακής εξάρτησης</p>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <p>Ορίζουσες</p>
                                            <asp:CheckBoxList ID="LeftFDCheckBoxListAttrSelection" runat="server"></asp:CheckBoxList>

                                        </div>
                                        <div class="col-md-4">
                                            <p>Εξαρτημένες</p>
                                            <asp:CheckBoxList ID="RightFDCheckBoxListAttrSelection" runat="server"></asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <%-- TODO: για διαγραφή λογικά, εκτός κι αν δουλέψει κάπως. --%>
                                        <p>Τελική μορφή συναρτησιακής εξάρτησης</p>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="Button1" Text="OK" class="btn btn-default" OnClick="btnNewFDClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Modal--%>
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
                        <button type="button" class="btn btn-success btn-lg" data-toggle="modal" data-target="#modalNew">Νέο Σχήμα</button>

                        <!-- Modal νέο σχήμα-->
                        <div class="modal fade" id="modalNew" role="dialog">
                            <div class="modal-dialog">

                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Νέο σχήμα</h4>
                                    </div>
                                    <div class="modal-body">
                                        <p>Εισάγετε όνομα για το νέο σχήμα.</p>
                                        <asp:TextBox ID="tbxNewSchemaName" runat="server" placeholder="Όνομα σχήματος"></asp:TextBox>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="Button7" Text="Δημιουργία" class="btn btn-default" OnClick="btnNewSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--Modal--%>
                    </div>
                    <div>
                        <asp:Button ID="btnLoadSchema" class="btn btn-success btn-lg" runat="server" Text="Φόρτωση Σχήματος" OnClick="btnLoadSchema_Click"  />
                     <%--   <button type="button" class="btn btn-success btn-lg" data-toggle="modal" data-target="#loadSchema">Φόρτωση Σχήματος</button>
                    --%>    <br />

                        <!-- Modal φόρτωση σχήματος-->
                        <div class="modal fade" id="loadSchemaModal" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Load</h4>
                                    </div>
                                    <div class="modal-body">

                                        <%--<p>Επιλέξτε αρχείο </p>
                                    <asp:FileUpload ID="FileUpload1" runat="server" />--%>
                                        <p>Επιλέξτε παράδειγμα για φόρτωση</p>
                                        <asp:DropDownList ID="schemaLoadDropDownList" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="Button6" Text="OK" class="btn btn-default" OnClick="btnLoadSelectedSchemaClick" UseSubmitBehavior="false" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--Modal--%>
                    </div>
                </div>

                <%-- // TODO: Προσθήκη δυνατότητας αποθήκευσης σχήματος σε αρχείο στον client. --%>
            </div>

        </div>
    </form>
</body>
</html>
