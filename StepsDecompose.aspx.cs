using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Normalization;
using System.Drawing;
using System.Data;

public partial class StepsDecompose : System.Web.UI.Page
{
    private List<Attr> attrList = new List<Attr>(); // Λίστα με αντικείμενα Attr, για τα γνωρίσματα.
    private List<FD> fdList = new List<FD>(); // Λίστα με αντικείμενα FD, για τις συναρτησιακές εξαρτήσεις.
    private List<Key> keyList; // τα κλειδιά του πίνακα.
    private List<Relation> relList = new List<Relation>(); // λίστα με τους πίνακες Relation.

    protected void Page_Load(object sender, EventArgs e)
    {
        // Φορτώνονται οι μεταβλητές που χρειάζονται κατά την επαναφόρτωση της εφαρμογής.
        #region ViewStates Load
        if (ViewState["attrListVS"] != null)
            attrList = (List<Attr>)ViewState["attrListVS"];

        if (ViewState["fdListVS"] != null)
            fdList = (List<FD>)ViewState["fdListVS"];

        if (ViewState["keyListVS"] != null)
            keyList = (List<Key>)ViewState["keyListVS"];

        if (ViewState["relListVS"] != null)
            relList = (List<Relation>)ViewState["relListVS"];

        
        #endregion

        if (!IsPostBack)
        {
            // Όταν καλείται η σελίδα αυτή, τότε πρέπει οπωσδήποτε να φορτωθούν και οι λίστες 
            // με τα γνωρίσματα και τις συναρτησιακές εξαρτηήσεις, για να γίνει η σταδιακή διάσπαση.
            #region Φόρτωση λιστών
            if (Session["attrListSE"] != null && Session["fdListSE"] != null && Session["schemaName"] != null)
            {
                attrList = (List<Attr>)Session["attrListSE"];
                fdList = (List<FD>)Session["fdListSE"];
                lblSchemaName.Text = (string)Session["schemaName"];

                if (Session["schemaDescription"] != null)
                    lblSchemaDescription.Text = (string)Session["schemaDescription"];

                setInitialValues();

            }
            #endregion Φόρτωση λιστών

        }

    }

    /// <summary>
    /// Εκτελείται λίγο πριν ανανεωθεί η σελίδα.
    /// </summary
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Φορτώνονται οι μεταβλητές που αποθηκεύω μέσω ViewState.
        ViewState.Add("attrListVS", attrList);
        ViewState.Add("fdListVS", fdList);
        ViewState.Add("keyListVS", keyList);
        ViewState.Add("relListVS", relList);

    }

    /// <summary>
    /// Με βάση τις λίστες attrList και fdList, δημιουργείται ο πίνακας relation και τα υποψήφια κλειδιά του
    /// ώστε να ξεκινήσει η διαδικασία της σταδιακής διάσπασης.
    /// </summary>
    private void setInitialValues()
    {
        // δημιουργείται νέος πίνακας Relation ο οποίος αρχικά περιλαμβάνει όλα τα γνωρίσματα του σχήματος
        // και προσττίθεται στη λίστα των πινάκων Relation.
        Relation relInitial = new Relation(attrList);
        relList.Add(relInitial);
        relInitial.Name = "R";

        // γέμισμα συναρτησιακών εξαρτήσεων.
        populateFdGridView(fdList);

        // γέμισμα υποψήφιων κλειδιών.
        //προσδιορίζονται τα κλειδιά του πίνακα και εμφανίζονται στο txtKeys
        var result = Global.findKeys(attrList, fdList, false);
        keyList = result.Item1;

        List<string> names = new List<string>();
        foreach (Key key in keyList)
        {
            names.Add(key.ToString());
        }
        names.Sort();
        string str = string.Join("   ", names);
        tbxKeys.Text = str;

        // Ολοκλήρωση πινάκων
        relInitial.SetKey(keyList[0]);

        populateRelationGridView(relList);
    }

    #region Actions for Decompose (Preview, Decompose, ShowBCNFTables)

    /// <summary>
    /// Μέθοδος που εκτελείται με το πάτημα του κουμπιού Προεπισκόπηση.
    /// Δείχνει την διάσπαση που θα γίνει με βάση τον επιλεγμένο πίνακα και την επιλεγμένη συναρτησιακή εξάρτηση.
    /// </summary>
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (CheckTick())
            DecomposeInSteps(true);
        
        ClientScript.RegisterStartupScript(Page.GetType(), "modalPreview", "$('#modalPreview').modal();", true);

    }

    /// <summary>
    /// Action για το πάτημα του κουμπιού Διάσπαση.
    /// </summary>
    protected void btnDecompose_Click(object sender, EventArgs e)
    {
        if (CheckTick())
            DecomposeInSteps(false);

        ClientScript.RegisterStartupScript(Page.GetType(), "modalPreview", "$('#modalPreview').modal();", true);
    }

    /// <summary>
    /// Μέθοδος στην οποία γίνεται η επιλεγμένη διάσπαση.
    /// </summary>
    /// <param name="isPreview">Αν είναι αληθής τότε εμφανίζονται στο modal τα αποτελέσματα.</param>
    private void DecomposeInSteps(bool isPreview)
    {
        int iRel = 0;
        int iFD = 0;

        UpdateExcludedFD();

        // προσδιορίζεται ο πίνακας που έχει επιλεγεί.
        Relation rel = null;
        iRel = gridViewRelation.SelectedIndex;
        rel = relList[iRel];

        // αν ο πίνακας που έχει επιλεγεί δεν μπορεί να διασπαστεί περαιτέρω, βγαίνει σχετικό μήνυμα.
        if (!isPreview && rel.Excluded)
        {
            log.InnerText = "Ο πίνακας\n\n" + rel.ToString() + "\n\nέχει αποκλειστεί από περαιτέρω διασπάσεις.";
            return;
        }

        // προσδιορίζεται η συναρτησιακή εξάρτηση που έχει επιλεγεί.
        FD fd = null;
        iFD = gridViewFD.SelectedIndex;
        fd = fdList[iFD];

        // εξετάζεται αν η συναρτησιακή εξάρτηση είναι τετριμμένη.
        if (fd.IsTrivial)
        {
            log.InnerText = "Η συναρτησιακή εξάρτηση\n\n\""  + fd.ToString() + "\"\n\nείναι τετριμμένη, επομένως δεν χρησιμοποιείται για διάσπαση πινάκων.";
            return;
        }

        //αν το αριστερό σκέλος της συναρτησιακής εξάρτησης περιλαμβάνει κλειδί, τότε δεν χρησιμοποιείται
        //ο έλεγχος γίνεται με τη βοήθεια της τομής.
        foreach (Key key in keyList)
        {
            if (fd.GetLeft().Intersect(key.GetAttrs(), Global.comparer).Count() >= key.GetAttrs().Count)
            {
                log.InnerText = "Η συναρτησιακή εξάρτηση\n\n\"" + fd.ToString() + "\"\n\nπεριλαμβάνει υποψήφιο κλειδί στο αριστερό σκέλος της, επομένως δεν παραβιάζει την BCNF μορφή και δεν χρησιμοποιείται για διάσπαση πινάκων.";
                return;
            }
        }

        // επίσης ελέγχεται αν η συναρτησιακή εξάρτηση έχει ως ορίζουσα υποψήφιο κλειδί του προς διάσπαση πίνακα.
        List<Key> tempoRelKey = new List<Key>();
        var resultTemp = Global.findKeys(rel.GetList(), fdList, false);
        tempoRelKey = resultTemp.Item1;

        foreach (Key key in tempoRelKey)
        {
            if (fd.GetLeft().Intersect(key.GetAttrs(), Global.comparer).Count() >= key.GetAttrs().Count)
            {
                log.InnerText = "Η συναρτησιακή εξάρτηση\n\n\"" + fd.ToString() + "\"\n\nπεριλαμβάνει υποψήφιο κλειδί του πίνακα " + rel.Name + ", επομένως δεν μπορεί να τον διασπάσει.";
                return;
            }
        }

        // αν η τομή x του συνόλου των γνωρισμάτων της συναρτησιακής εξάρτησης και των γνωρισμάτων του πίνακα είναι μικρότερη σε αριθμό από το πλήθος των γνωρισμάτων του πίνακα και ίση με το πλήθος των γνωρισμάτων της συναρτησιακής εξάρτησης, τότε παραβιάζεται η BCNF μορφή και ο πίνακας μπορεί να διασπαστεί.
        int x = fd.GetAll().Intersect(rel.GetList(), Global.comparer).Count();

        if (x < rel.GetList().Count && x == fd.GetAll().Count)
        {

            // παρακάτω δημιουργούνται δύο νέοι πίνακες, ο rel1 και ο rel2.

            // ο rel1 πίνακας παίρνει τα γνωρίσματα της συναρτησιακής εξάρτησης.
            Relation rel1 = new Relation(fd.GetAll());

            // ο rel2 πίνακας παίρνει τα γνωρίσματα από το αριστερό σκέλος της συναρτησιακής εξάρτησης, συν τα γνωρίσματα του πίνακα που διασπάστηκε, πλην αυτών που βρίσκονται στο δεξί σκέλος της συναρτησιακής εξάρτησης.
            List<Attr> tempo = new List<Attr>();
            tempo.AddRange(fd.GetLeft());
            tempo.AddRange(rel.GetList().Except(fd.GetRight(), Global.comparer));
            Relation rel2 = new Relation(tempo);

            // δημιουργούνται δύο κλειδιά, ένα για τον καθένα πίνακα.
            Key key1 = new Key();
            Key key2 = new Key();

            // το κλειδί του πρώτου πίνακα είναι η ορίζουσα της συναρτησιακής εξάρτησης που προκάλεσε την διάσπαση.
            key1.AddToKey(fd.GetLeft());
            rel1.SetKey(key1);

            // προσδιορίζουμε το κλειδί του δεύτερου πίνακα (αυτό που δίνει όλα τα γνωρίσματά του)
            // δημιουργούμε μια τοπική λίστα κλειδιών και ως κλειδί του δεύτερου πίνακα ορίζεται το πρώτο κλειδί της λίστας.
            List<Key> tempoKeyList = new List<Key>();
            var resultTemp2 = Global.findKeys(rel2.GetList(), fdList, false);
            tempoKeyList = resultTemp2.Item1;

            key2.AddToKey(tempoKeyList[0].GetAttrs());
            rel2.SetKey(key2);

            // ορίζονται τα ονόματα των δύο νέων πινάκων.
            if (Relation.isAA)
            {
                rel1.Name = "R" + ++Relation.aa;
                rel2.Name = "R" + ++Relation.aa;
                if (isPreview)
                {
                    Relation.aa = Relation.aa - 2;
                }
            }
            else
            {
                rel1.Name = rel.Name + "1";
                rel2.Name = rel.Name + "2";
            }

            // οι δύο νέοι πίνακες προστίθενται στη λίστα.
            if (!isPreview)
            {
                //προστίθενται οι δύο νέοι πίνακας στο CheckboxList
                relList.Add(rel1);
                relList.Add(rel2);

                //εμφανίζονται τα σχετικά μηνύματα στα Αποτελέσματα.
                resultsArea.InnerText += "\nΜε την \"" + fd.ToString() + "\" ο " + rel.Name + " διασπάται σε:\n\n" + rel1.ToString() + RelBCNF(rel1) + "\n\n" + rel2.ToString() + RelBCNF(rel2) + "\n\n";
                resultsArea.InnerText += "==============================\n\n";


                //μπαίνει Χ στα Grid για τον πίνακα rel και τη συναρτησιακή εξάρτηση fd
                rel.IsBCNF = false;
                rel.Excluded = true;
                fdList[iFD].Excluded = true;

                log.InnerText = "Έγινε διάσπαση σε δύο νέους πίνακες, τον " + rel1.Name + " και τον " + rel2.Name + ".";
            }
            else
            {
                log.InnerText = "Με την \"" + fd.ToString() + "\" ο " + rel.ToString() + " διασπάται σε:\n\n" + rel1.ToString() + RelBCNF(rel1) + "\n\n" + rel2.ToString() + RelBCNF(rel2) + "\n\n";
                log.InnerText += "==============================\n\n";
            }

            // Ελέγχονται και ενημερώνονται οι πίνακες που είναι σε BCNF μορφή.
            CheckBCNF();
            UpdateExcludedFD();

            populateRelationGridView(relList);
            populateFdGridView(fdList);

        }
        else // σε διαφορετική περίπτωση η BCNF δεν παραβιάζεται και εμφανίζεται σχετικό μήνυμα
        {
            log.InnerText = "Η συναρτησιακή εξάρτηση\n\n\"" + fd.ToString() + "\"\n\nδεν σχετίζεται με τον πίνακα\n\n" + rel.ToString() + "\n\nκαι επομένως δεν γίνεται διάσπαση.";
        }


    }

    /// <summary>
    /// Ανανεώνεται η κατάσταση excluded των συναρτησιακών εξαρτήσεων
    /// </summary>
    private void UpdateExcludedFD()
    {
        //αποκλείονται οι τετριμμένες συναρτησιακές εξαρτήσεις
        foreach (FD fd in fdList)
            if (fd.IsTrivial) fd.Excluded = true;

        //εξαιρούνται οι συναρτησιακές εξαρτήσεις που το αριστερό σκέλος τους περιλαμβάνει κλειδί
        //ο έλεγχος γίνεται με τη βοήθεια της τομής
        foreach (FD fd in fdList)
            foreach (Key key in keyList)
                if (!fd.Excluded && fd.GetLeft().Intersect(key.GetAttrs(), Global.comparer).Count() >= key.GetAttrs().Count)
                {
                    fd.Excluded = true;
                    break;
                }
    }

    /// <summary>
    /// Ελέγχει αν ο πίνακας Rel είναι BCNF
    /// </summary>
    private string RelBCNF(Relation rel)
    {
        foreach (FD fd in fdList)
        {
            if (fd.Excluded) continue;
            //αν η τομή x του συνόλου των γνωρισμάτων της συναρτησιακής εξάρτησης και των γνωρισμάτων του πίνακα είναι μικρότερη σε αριθμό από το πλήθος των γνωρισμάτων του πίνακα και ίση με το πλήθος των γνωρισμάτων της συναρτησιακής εξάρτησης, τότε παραβιάζεται η BCNF μορφή και ο πίνακας μπορεί να διασπαστεί
            //σε διαφορετική περίπτωση ο πίνακας είναι BCNF
            int x = fd.GetAll().Intersect(rel.GetList(), Global.comparer).Count();
            if (x < rel.GetList().Count && x == fd.GetAll().Count)
                return "";
        }
        rel.IsBCNF = true;
        return "    (BCNF)";
    }

    /// <summary>
    /// Ελέγχεται ποιοι πίνακες είναι BCNF
    /// </summary>
    private void CheckBCNF()
    {
        for (int i = 0; i < relList.Count; i++)
        {
            if (relList[i].IsBCNF | relList[i].Excluded) continue;
            foreach (FD fd in fdList)
            {
                if (fd.Excluded) continue;
                //αν η τομή x του συνόλου των γνωρισμάτων της συναρτησιακής εξάρτησης και των γνωρισμάτων του πίνακα είναι μικρότερη σε αριθμό από το πλήθος των γνωρισμάτων του πίνακα και ίση με το πλήθος των γνωρισμάτων της συναρτησιακής εξάρτησης, τότε παραβιάζεται η BCNF μορφή και ο πίνακας μπορεί να διασπαστεί
                //σε διαφορετική περίπτωση ο πίνακας είναι BCNF
                int x = fd.GetAll().Intersect(relList[i].GetList(), Global.comparer).Count();
                if (x < relList[i].GetList().Count && x == fd.GetAll().Count)
                {
                    relList[i].IsBCNF = false;
                    break;
                }
                else
                {
                    relList[i].IsBCNF = true;
                }
            }
        }
    }

    /// <summary>
    /// Ελέγχει αν έχει επιλεγεί ένας πίνακας και μια συναρτησιακή εξάρτηση.
    /// </summary>
    /// <returns></returns>
    private bool CheckTick()
    {
        int indexRel = gridViewRelation.SelectedIndex;
        int indexFd = gridViewFD.SelectedIndex;

        if (indexRel >= 0 && indexFd >= 0)
        {
            return true;
        }

        log.InnerText = "Πρέπει να επιλέξετε έναν πίνακα και μια συναρτησιακή εξάρτηση.";
        return false;
    }

    /// <summary>
    /// Εμφανίζει, αν υπάρχουν, τους πίνακες που είναι σε BCNF.
    /// </summary>
    protected void btnShowBCNFTablesClick(object sender, EventArgs e)
    {
        //εμφανίζονται στο txtOut οι πίνακες που είναι σε BCNF μορφή

        string msg = "";
        bool oneBCNF = false;
        foreach (Relation rel in relList)
            if (rel.IsBCNF)
                oneBCNF = true;

        msg += "==============================\n\n";
        if (oneBCNF)
        {
            msg += "Οι πίνακες BCNF είναι οι εξής:\n\n";
            foreach (Relation rel in relList)
                if (rel.IsBCNF)
                {
                    msg += rel.ToString() + "\n\n";
                }
        }
        else
        {
            msg += "Δεν υπάρχουν πίνακες BCNF\n\n";
        }
        msg += "==============================\n\n";
        log.InnerText = msg;

        ClientScript.RegisterStartupScript(Page.GetType(), "modalPreview", "$('#modalPreview').modal();", true);

    }


    #endregion

    #region Other Actions (Clear, Reset, Exit)

    /// <summary>
    /// Μέθοδος που καλείται με το πάτημα του κουμπιού Καθαρισμός.
    /// Αδειάζει την περιοχή αποτελεσμάτων.
    /// </summary>
    protected void btnClearResults_Click(object sender, EventArgs e)
    {
        resultsArea.InnerText = "";
        log.InnerText = "";
    }

    /// <summary>
    /// Επαναφέρει την αρχική κατάσταση των πινάκων.
    /// </summary>
    protected void btnResetClick(object sender, EventArgs e)
    {
        resultsArea.InnerText = "";
        log.InnerText = "";

        relList.Clear();
        foreach (FD fd in fdList)
        {
            fd.Excluded = false;
        }

        setInitialValues();
    }

    /// <summary>
    /// Μέθοδος που καλέιται με το πάτημα του κουμπιού Κλείσιμο. 
    /// Επιστρέφει στην αρχική σελίδα.
    /// </summary>
    protected void btnCloseStepsDecompose_Click(object sender, EventArgs e)
    {
        Session["schemaName"] = lblSchemaName.Text;

        //  Response.Redirect("http://ilust.uom.gr:9000/Default.aspx");
        Response.Redirect("Default.aspx");
    }

    #endregion

    #region GridViews

    #region Populate

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τα Relation.
    /// </summary>
    /// <param name="relList">Η λίστα με τα relation τα οποία θα φορτώσει.</param>
    private void populateRelationGridView(List<Relation> relList)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("BCNF", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Relation", typeof(string)));

        foreach (Relation rel in relList)
        {
            string bcnf = "";
            if (rel.IsBCNF)
            {
                bcnf = "BCNF";
            }
            else if (rel.Excluded)
            {
                bcnf = "X";
            }

            dataTable.Rows.Add(bcnf, rel.ToString());
        }

        gridViewRelation.DataSource = dataTable;
        gridViewRelation.DataBind();

    }

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τις συναρτησιακές εξαρτήσεις.
    /// </summary>
    /// <param name="fdList">Οι συναρτησιακές εξαρτήσεις που θα φορτώσει.</param>
    private void populateFdGridView(List<FD> fdList)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Excluded", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Description", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Trivial", typeof(string)));

        foreach (FD fd in fdList)
        {
            string trivial = "";
            string excluded = "";

            if (fd.IsTrivial)
                trivial = true.ToString();
            if (fd.Excluded)
                excluded = "X";

            dataTable.Rows.Add(excluded, fd.ToString(), trivial);
        }

        gridViewFD.DataSource = dataTable;
        gridViewFD.DataBind();
    }

    #endregion

    #region Management (Update selected row)

    /// <summary>
    /// Προσθέτει λειτουργικότητα στις γραμμές του gridViewRelation μόλις προστεθεί περιεχόμενο.
    /// </summary>
    protected void OnRowDataBoundRelation(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridViewRelation, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    /// <summary>
    /// Ενημερώνει την επιλεγμένη γραμμή για το gridViewRelation.
    /// </summary>
    protected void OnSelectedIndexChangedRelation(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gridViewRelation.Rows)
        {
            if (row.RowIndex == gridViewRelation.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = string.Empty;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }

    /// <summary>
    /// Προσθέτει λειτουργικότητα στις γραμμές του gridViewFD μόλις προστεθεί περιεχόμενο.
    /// </summary>
    protected void OnRowDataBoundFD(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridViewFD, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    /// <summary>
    /// Ενημερώνει την επιλεγμένη γραμμή για το gridViewFD.
    /// </summary>
    protected void OnSelectedIndexChangedFD(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gridViewFD.Rows)
        {
            if (row.RowIndex == gridViewFD.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = string.Empty;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }

    #endregion

    #endregion

    #region FOR DELETE???

    /// <summary>
    /// mono fix for lost checkboxlist states
    /// </summary>
    private void setCheckBoxStates(CheckBoxList cbl)
    {
        if (IsPostBack)
        {
            string cblFormID = cbl.ClientID.Replace("_", "$");
            int i = 0;
            foreach (var item in cbl.Items)
            {
                string itemSelected = Request.Form[cblFormID + "$" + i];
                if (itemSelected != null && itemSelected != String.Empty)
                    ((ListItem)item).Selected = true;
                i++;
            }
        }
    }

    #endregion
    
}
