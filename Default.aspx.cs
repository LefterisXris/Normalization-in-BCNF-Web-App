using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Normalization;
using System.IO;
using System.Data;
using System.Drawing;

/// <summary>
/// Default: Περιλαμβάνει όλες τις λειτουργίες της Εφαρμογής.
/// </summary>
public partial class _Default : System.Web.UI.Page
{
    private List<Attr> attrList = new List<Attr>(); // Λίστα με αντικείμενα Attr, για τα γνωρίσματα.
    private List<FD> fdList = new List<FD>(); // Λίστα με αντικείμενα FD, για τις συναρτησιακές εξαρτήσεις.
    private string msg = ""; // Μεταβλητή που τυπώνει στην Logging Console. (βοηθητική) // TODO: άλλος σχεδιασμός.

    protected void Page_Load(object sender, EventArgs e)
    {
        // Αρχικοποίηση της Logging Console.
        msg += "Logging Console...";
        log.InnerText = msg;

        // Αρχικοποίηση μερικών γνωρισμάτων και συναρτησιακών εξαρτήσεων μόνο την πρώτη φορά που τρέχει η εφαρμογή.
        if (IsPostBack == false)
        {
            LoadSelectedSchema("Default.txt");
        }
        
        #region ViewStates Load
        if (ViewState["attrListVS"] != null)
            attrList = (List<Attr>)ViewState["attrListVS"];

        if (ViewState["fdListVS"] != null)
            fdList = (List<FD>)ViewState["fdListVS"];

        if (ViewState["logVS"] != null)
            msg = (string)ViewState["logVS"];

        #endregion
        
    }

    /// <summary>
    /// Εκτελείται λίγο πριν ανανεωθεί η σελίδα.
    /// </summary
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Φορτώνονται οι μεταβλητές που αποθηκεύω μέσω ViewState.
        ViewState.Add("attrListVS", attrList);
        ViewState.Add("fdListVS", fdList);
        ViewState.Add("logVS", msg);
      
    }

    #region ADD NEW (Attr & Fd)

    #region Attr

    /// <summary>
    /// Εμφανίζει το Modal για την προσθήκη νέου γνωρίσματος.
    /// </summary>
    protected void btnNewAttrClick(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "modalNewAttribute", "$('#modalNewAttribute').modal();", true);
    }

    /// <summary>
    /// Καλείται όταν πατηθεί το ΟΚ από το Modal νέου Γνωρίσματος.
    /// </summary>
    protected void btnNewAttrOKClick(object sender, EventArgs e)
    {
        string attrName = tbxNewAttrName.Text.Trim();
        
        log.InnerText = ""; msg = "";
        string[] attrss = attrName.Split(',');
       

        for (int i = 0; i < attrss.Length; i++)
        {
            attrss[i].Trim(); // Απαλοίφω τον κενό χαρακτήρα.
            if (AttrCreate(attrss[i].Trim(), tbxNewAttrType.Text.Trim()))
            {
                populateAttrGridView(attrList);
                msg += "\nNew attribute inserted: " + attrss[i].Trim();
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Success!</strong> New attribute inserted!'); $('#alertBoxSuccess').show();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "alert", "HideLabel();", true);
            }
            else
            {
                msg += "\nCannot create attribute: Attribute already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", "$('#alertBoxFailText').html('<strong>Fail!</strong> Cannot create attribute: Attribute already exists..'); $('#alertBoxFail').show();", true);
            }

            log.InnerText = msg;
        }
    }

    /// <summary>
    /// Μέθοδος δημιουργίας νέου γνωρίσματος και προσθήκης του στην attrList. Επιστρέφει false αν το όνομα χρησιμοποιείται ήδη για άλλο αντικείμενο
    /// </summary>
    /// <param name="name">Ονομασία του γνωρίσματος</param>
    /// <param name="type">Τύπος του γνωρίσματος</param>
    private bool AttrCreate(string name, string type)
    {
        //ελέγχεται αν το όνομα του νέου γνωρίσματος χρησιμοποιείται ήδη, κι αν ναι, επιστρέφεται η ένδειξη false
        if (AttrExists(name, null)) return false;

        // ελέγχεται αν δεν έχει όνομα.
        if (name.Equals("")) return false;

        //δημιουργείται αντικείμενο τύπου Attr και προστίθεται στην attrList
        Attr attr = new Attr(name, type);
        attrList.Add(attr);

        //επιστρέφεται η ένδειξη true
        return true;
    }

    /// <summary>
    /// Ελέγχει και επιστρέφει true αν το όνομα name χρησιμοποιείται ήδη για άλλο γνώρισμα
    /// </summary>
    /// <param name="name">Η ονομασία του γνωρίσματος που ελέγχουμε</param>
    /// <param name="attr">Η αναφορά στο αντικείμενο του γνωρίσματος</param>
    private bool AttrExists(string name, Attr attr)
    {
        for (int i = 0; i < attrList.Count; i++)
            if (attrList[i].Name == name && attr != attrList[i]) return true; //το όνομα χρησιμοποιείται ήδη
        return false; //το όνομα δεν χρησιμοποιείται
    }

    #endregion

    #region FD

    /// <summary>
    /// Ανοίγει το Modal για προσθήκη νέας συναρτησιακής εξάρτησης.
    /// </summary>
    protected void btnNewFDClick(object sender, EventArgs e)
    {
        populateLeftAndRightFDGridView();
        ClientScript.RegisterStartupScript(Page.GetType(), "modalNewFD", "$('#modalNewFD').modal();", true);
    }

    /// <summary>
    /// Καλείται όταν πατηθεί ΟΚ από το Modal Συναρτησιακής Εξάρτησης.
    /// </summary>
    protected void btnNewFDOKClick(object sender, EventArgs e)
    {
        FD fd = new FD();

        foreach (GridViewRow item in gridViewLeftFD.Rows)
        {
            if ((item.Cells[0].FindControl("checkBoxLeftFD") as CheckBox).Checked)
            {
                fd.AddLeft(attrList[item.RowIndex]);
            }
        }

        foreach (GridViewRow item in gridViewRightFD.Rows)
        {
            if ((item.Cells[0].FindControl("checkBoxRightFD") as CheckBox).Checked)
            {
                fd.AddRight(attrList[item.RowIndex]);
            }
        }

        if (FDCreate(fd))
        {
            populateFdGridView(fdList);
            log.InnerText = "FD inserted: " + fd.ToString();
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Success!</strong> New FD inserted!'); $('#alertBoxSuccess').show();", true);
        }
        else
        {
            log.InnerText = "Cannot insert FD: FD already exists..";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Fail!</strong> Cannot insert FD: FD already exists..'); $('#alertBoxFail').show();", true);
        }
        
    }

    /// <summary>
    /// Μέθοδος ελέγχου και προσθήκης νέας συναρτησιακής εξάρτησης στην FDList
    /// </summary>
    /// <param name="fd">Το αντικείμενο συναρτησιακής εξάρτησης fd θα ελεγχθεί αν υπάρχει κάποιο άλλο παρόμοιο με αυτό πριν καταχωρηθεί.</param>
    public bool FDCreate(FD fd)
    {
        //ελέγχεται αν η νέα συναρτησιακή εξάρτηση είναι παρόμοια με μια άλλη, κι αν ναι, επιστρέφεται η ένδειξη false
        if (FDExists(fd, -1)) return false;

        //το νέο αντικείμενο προστίθεται στην FDList
        fdList.Add(fd);

        //επιστρέφεται η ένδειξη true
        return true;
    }

    /// <summary>
    /// Ελέγχει και επιστρέφει true αν υπάρχει παρόμοια συναρτησιακή εξάρτηση
    /// </summary>
    /// <param name="fd">Το αντικείμενο της συναρτησιακής εξάρτησης που ελέγχουμε</param>
    /// <param name="id">Ο αύξοντας αριθμός της υπό επεξεργασία συναρτησιακής εξάρτησης</param>
    public bool FDExists(FD fd, int id)
    {
        for (int i = 0; i < fdList.Count; i++)
            if (fdList[i].ToString() == fd.ToString() && i != id) return true; //βρέθηκε παρόμοια συναρτησιακή εξάρτηση
        return false; //δεν υπάρχει παρόμοια συναρτησιακή εξάρτηση
    }

    #endregion

    #endregion

    #region EDIT (Attr & Fd)

    #region Attr

    /// <summary>
    /// Αν έχει επιλεχθεί κάποιο γνώρισμα, τότε φορτώνεται προς επεξεργασία.
    /// </summary>
    protected void btnEditAttrClick(object sender, EventArgs e)
    {
        int index = gridViewAttr.SelectedIndex;
        if (index >= 0)
        {
            tbxEditAttrName.Text = attrList[index].Name;
            tbxEditAttrType.Text = attrList[index].Type;

            ClientScript.RegisterStartupScript(Page.GetType(), "modalEditAttribute", "$('#modalEditAttribute').modal();", true);
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Warning!</strong> You must select an attribute first.'); $('#alertBoxWarning').show();", true);
            return;
        }

    }

    /// <summary>
    /// Όταν τελειώσει η επεξεργασία του γνωρίσματος, αποθηκεύω το γνώρισμα στην λίστα.
    /// </summary>
    protected void btnEditAttrΟΚClick(object sender, EventArgs e)
    {
        string name = tbxEditAttrName.Text.Trim();
        string type = tbxEditAttrType.Text.Trim();

        int index = gridViewAttr.SelectedIndex;
        if (index >= 0)
        {
            string prevName = attrList[index].Name;
            attrList[index].Name = ""; // για να μην βγάλει διπλότυπο.

            if (!AttrExists(name, (new Attr(name, type))))
            {
                attrList[index].Name = name;
                attrList[index].Type = type;

                populateAttrGridView(attrList);
                msg += "\nAttribute Edited!.";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Success!</strong> Attributed edited!'); $('#alertBoxSuccess').show();", true);
            }
            else
            {
                msg += "\nCannot create attribute: Attribute already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Fail!</strong> Cannot create attribute: Attribute already exists..'); $('#alertBoxFail').show();", true);
                attrList[index].Name = prevName;
            }
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Warning!</strong> You must select an attribute first.'); $('#alertBoxWarning').show();", true);
            return;
        }
        log.InnerText = msg;
    }

    #endregion

    #region FD
    
    /// <summary>
    /// Ελέγχει αν έχει επιλεγεί μια συναρτησιακή εξάρτηση και την φορτώνει για επεξεργασία.
    /// </summary>
    protected void btnEditFDClick(object sender, EventArgs e)
    {
        // Σβήνω το περιεχόμενο τη προεπισκόπησης.
        lblPreviewFDtoEditRight.Text = "";
        lblPreviewFDtoEditLeft.Text = "";

        int index = gridViewFD.SelectedIndex;
        if (index >= 0)
        {
            populateLeftAndRightEditFDGridView();

            // Τσεκάρω το αριστερό μέρος της fd.
            foreach (GridViewRow item in gridViewEditLeftFD.Rows)
            {
                foreach (Attr attr in fdList[index].GetLeft())
                {
                    if ((item.Cells[1].Text.Equals(attr.Name)))
                    {
                        CheckBox c = (CheckBox)item.Cells[0].FindControl("checkBoxEditLeftFD");
                        c.Checked = true;
                    }
                }
            }

            // Τσεκάρω το δεξί μέρος της fd.
            foreach (GridViewRow item in gridViewEditRightFD.Rows)
            {
                foreach (Attr attr in fdList[index].GetRight())
                {
                    if ((item.Cells[1].Text.Equals(attr.Name)))
                    {
                        CheckBox c = (CheckBox)item.Cells[0].FindControl("checkBoxEditRightFD");
                        c.Checked = true;
                    }
                }
            }

            // Γεμίζω το περιεχόμενο της προεπισκόπησης.
            foreach (Attr attr in fdList[index].GetLeft())
                lblPreviewFDtoEditLeft.Text += attr.Name + ", "; // Αριστερό μέρος
            foreach (Attr attr in fdList[index].GetRight())
                lblPreviewFDtoEditRight.Text += attr.Name + ", "; // Δεξί μέρος
            lblArrow2.Text = "\u2192"; // Βέλος.

            // Αφαιρώ το κόμμα από τα τελευταία γνωρίσματα.
            string s = lblPreviewFDtoEditLeft.Text;
            lblPreviewFDtoEditLeft.Text = s.Remove((s.Length - 2), 2);
            
            s = lblPreviewFDtoEditRight.Text;
            lblPreviewFDtoEditRight.Text = s.Remove((s.Length - 2), 2);

            ClientScript.RegisterStartupScript(Page.GetType(), "modalEditFD", "$('#modalEditFD').modal();", true);
        }
        else
        {
            log.InnerText = "You must select an FD first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Warning!</strong> You must select an FD first.'); $('#alertBoxWarning').show();", true);
            return;
        }
    }

    /// <summary>
    /// Η επεξεργασμένη πλέον συναρτησιακή εξάρτηση προστίθεται στην λίστα fdList.
    /// </summary>
    protected void btnEditFDΟΚClick(object sender, EventArgs e)
    {
        int index = gridViewFD.SelectedIndex;
        if (index >= 0)
        {
            FD fd = new FD(); // Η προσωρινή.

            foreach (GridViewRow item in gridViewEditLeftFD.Rows)
            {
                if ((item.Cells[0].FindControl("checkBoxEditLeftFD") as CheckBox).Checked)
                {
                    fd.AddLeft(attrList[item.RowIndex]);
                }
            }

            foreach (GridViewRow item in gridViewEditRightFD.Rows)
            {
                if ((item.Cells[0].FindControl("checkBoxEditRightFD") as CheckBox).Checked)
                {
                    fd.AddRight(attrList[item.RowIndex]);
                }
            }

            if (!FDExists(fd, index))
            {
                fdList[index] = fd;
                
                populateFdGridView(fdList);
                log.InnerText = "FD Updated: " + fd.ToString();
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Success!</strong> FD Updated!'); $('#alertBoxSuccess').show();", true);
            }
            else
            {
                log.InnerText = "Cannot insert FD: FD already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Success!</strong> Cannot insert FD: FD already exists..'); $('#alertBoxFail').show();", true);
            }
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Warning!</strong> You must select an attribute first.'); $('#alertBoxWarning').show();", true);
            return;
        }
        
    }

    #endregion

    #endregion

    #region DELETE (Attr & Fd)

    #region Attr

    /// <summary>
    /// Διαγράφει το επιλεγμένο γνώρισμα από τον πίνακα γνωρισμάτων.
    /// </summary>
    protected void btnDeleteAttrClick(object sender, EventArgs e)
    {
        int index = gridViewAttr.SelectedIndex;
        if (index >= 0)
        {
            attrList.RemoveAt(index);
            populateAttrGridView(attrList);
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
        }
                
    }

    #endregion

    #region FD

    /// <summary>
    /// Διαγράφει την επιλεγμένη συναρτησιακή εξάρτηση από τον πίνακα.
    /// </summary>
    protected void btnDeleteFDClick(object sender, EventArgs e)
    {
        int index = gridViewFD.SelectedIndex;
        if (index >= 0)
        {
            fdList.RemoveAt(index);
            populateFdGridView(fdList);
        }
        else
        {
            log.InnerText = "You must select an FD first.";
        }
    }

    #endregion

    #endregion

    #region ACTIONS (Closure, Keys, Decompose, StepsDecompose)

    // Μένει να εμφανίζω τα ενδιάμεσα αποτελέσματα και πληροφορίες κατά τον υπολογισμό.
    #region Closure

    /// <summary>
    /// Φορτώνει τα γνωρίσματα και εμφανίζει το Modal για την εύρεση του Εγκλεισμού.
    /// </summary>
    protected void btnFindClosureClick(object sender, EventArgs e)
    {
        populateFindClosureGridView(); // Φόρτωση γνωρισμάτων για επιλογή
        
        ClientScript.RegisterStartupScript(Page.GetType(), "modalClosure", "$('#modalClosure').modal();", true);
    }
    
    /// <summary>
    /// Υπολογίζει τον εγλεισμό των επιλεγμένων γνωρισμάτων.
    /// </summary>
    protected void btnCalculateClosureClick(object sender, EventArgs e)
    {
        List<Attr> attrListSelected = new List<Attr>();

        // Τα επιλεγμένα γνωρίσματα εισάγωνται στην λίστα attrListSelected.
        foreach (GridViewRow item in gridViewFindClosure.Rows)
        {
            if ((item.Cells[0].FindControl("checkBoxFindClosure") as CheckBox).Checked)
            {
                attrListSelected.Add(attrList[item.RowIndex]);
            }
        }
        
        msg = "";
        List<Attr> atr1 = Global.findClosure(attrListSelected, fdList);

        foreach (Attr attr in atr1)
        {
            msg += attr.Name + ", ";
        }
        log.InnerText = "Closure is: " + msg;
    }

    #endregion

    // Μένει να εμφανίζω τα ενδιάμεσα αποτελέσματα και πληροφορίες κατά τον υπολογισμό.
    #region CalculateKeys

    /// <summary>
    /// Υπολογίζει τα υποψήφια κλειδιά του σχήματος.
    /// </summary>
    protected void btnCalculateKeysClick(object sender, EventArgs e)
    {
        List<Key> keyList = new List<Key>();
        var result = Global.findKeys(attrList, fdList, true);

        keyList = result.Item1; // Είναι η λίστα με τα κλειδιά.

        msg = result.Item2.ToString(); // Είναι οι λεπτομέρειες.
        
        log.InnerText = msg;
    }

    #endregion

    #region Decompose

    /// <summary>
    /// Μέθοδος διάσπασης σε πίνακες BCNF
    /// </summary>
    protected void btnDecomposeClick(object sender, EventArgs e)
    {
        bool alter = false;
        // μηδενίζεται η αρίθμηση της Relation.
        Relation.aa = 0;

        // ορίζεται λίστα με τους πίνακες Relation.
        List<Relation> RelList = new List<Relation>();

        // δημιουργείται νέος πίνακας Relation ο οποίος αρχικά περιλαμβάνει όλα τα γνωρίσματα του σχήματος
        // και προσττίθεται στη λίστα των πίνάκων Relation.
        Relation relInitial = new Relation(attrList);
        RelList.Add(relInitial);
        relInitial.Name = "R";

        msg = ""; msg += "Θέλουμε να διασπάσουμε κατά BCNF τον αρχικό πίνακα " + relInitial.ToString() + "\n\n";

        // δημιουργείται backup όλων των συναρτησιακών εξαρτήσεων, γιατί υπάρχει περίπτωση να αλλάξουν στην μέθοδο αυτή.
        foreach (FD fd in fdList)
        {
            fd.Backup();
            fd.Excluded = false;
        }

        // αν η λύση είναι η εναλλακτική, αντιστρέφεται η σειρά των συναρτησιακών εξαρτήσεων.
        if (alter)
            fdList.Reverse();

        // ορίζεται η λίστα που παίρνει τα κλειδιά του πίνακα.
        List<Key> keyList = new List<Key>();
        // Closure closure = new Closure(attrList, fdList);
        // keyList = closure.KeysGet(fdList, attrList, false);
        var result = Global.findKeys(attrList, fdList, false);
        keyList = result.Item1;

        if (keyList.Count > 0)
        {
            msg += "Τα υποψήφια κλειδιά του R είναι:\n\n";
            Key newkey = new Key();
            foreach (Key key in keyList)
            {
                msg += key.ToString() + "\n\n";
                newkey.AddToKey(key.GetAttrs());
            }
            relInitial.SetKey(newkey);
        }
        else
        {
            Key key = new Key();
            for (int i = 0; i < attrList.Count; i++)
                key.AddToKey(attrList[i]);

            keyList.Add(key);
            relInitial.SetKey(key);
            msg += "Επιλέγεται ως υποψήφιο κλειδί το σύνολο των γνωρισμάτων του R, καθώς δεν εντοπίστηκαν ως κλειδιά μεμονωμένα γνωρίσματα ή συνδυασμοί αυτών.";
        }

        // αποκλείονται οι τετριμμένες συναρτησιακές εξαρτήσεις.
        foreach (FD fd in fdList)
            if (fd.IsTrivial)
                fd.Excluded = true;

        // εξαιρούνται οι συναρτησιακές εξαρτήσεις που το αριστερό σκέλος τους περιλαμβάνει κλειδί.
        // ο έλεγχος γίνεται με τη βοήθεια της τομής.
        foreach (FD fd in fdList)
            foreach (Key key in keyList)
                if (!fd.Excluded && fd.GetLeft().Intersect(key.GetAttrs(), Global.comparer).Count() >= key.GetAttrs().Count)
                {
                    fd.Excluded = true;
                    break;
                }

        // ελέγχεται αν ισχύει ο κανόνας της μεταβατικότητας μεταξύ κάποιων συναρτησιακών εξαρτήσεων
        // κι αν ναι, τότε το δεξί σκέλος αλλάζει και παίρνει τον  εγκλεισμό του αριστερού σκέλους (πλην τα γνωρίσματα του αριστερού).
        for (int i = 0; i < fdList.Count; i++)
        {
            for (int j = 0; j < fdList.Count; j++)
            {
                if (fdList[i] == fdList[j] | fdList[i].Excluded | fdList[j].Excluded)
                    continue;
                int x = 0;
                foreach (Attr attr in fdList[i].GetRight())
                    if (fdList[j].GetLeft().Contains(attr, Global.comparer))
                        x++;
                if (x >= fdList[j].GetLeft().Count)
                {
                    // αν όλα τα γνωρίσματα του δεξιού σκέλους βρέθηκαν στο αριστερό, τότε η συναρτησιακή εξάρτηση τροποποιείται,
                    // με τα ίδια γνωρίσματα στα αριστερά και τον εγκλεισμό του αριστερού σκέλους στα δεξιά.
                    Closure newClosure = new Closure(attrList, fdList);
                    fdList[i].AddRight(Global.findClosure(fdList[i].GetLeft(), fdList));
                    //  fdList[i].AddRight(newClosure.attrClosure(fdList[i].GetLeft(), false));
                    fdList[i].RemoveRight(fdList[i].GetLeft());
                }
            }
        }

        msg += "==============================\n\n";
        msg += "Οι συναρτησιακές εξαρτήσεις που θα χρησιμοποιηθούν για τη διάσπαση, μετά:\n(1) την αφαίρεση των τετριμμένων,\n(2) την αφαίρεση αυτών που δεν παραβιάζουν την BCNF μορφή και\n(3) τη μετατροπή αυτών που ικανοποιούν τον κανόνα της μεταβατικότητας,\nείναι οι εξής:\n\n";

        bool chk = false;
        foreach (FD fd in fdList)
        {
            if (!fd.Excluded)
            {
                msg += fd.ToString() + "\n\n";
                chk = true;
            }
        }

        if (!chk)
        {
            msg += "Καμία!!!\n\nΕπομένως, ο αρχικός πίνακας R είναι σε BCNF μορφή και η διαδικασία σταματά.\n\n";
            relInitial.IsBCNF = true;
            goto SkipProcess;
        }

        msg += "==============================\n\n";

        // σαρώνονται όλοι οι πίνακες για να ελεγχθεί αν υπάρχει έστω και ένας πίνακας που δεν είναι BCNF ώστε να διασπαστεί.
        bool newRel = false;
        for (int i = 0; i < RelList.Count; i++)
        {
            if (!RelList[i].IsBCNF & !RelList[i].Excluded)
            {
                // σαρώνονται όλες οι διαθέσιμες συναρτησιακές εξαρτήσεις για να δούμε αν μπορούμε να τον διασπάσουμε.
                foreach (FD fd in fdList)
                {
                    if (fd.Excluded)
                        continue;
                    //αν η τομή x του συνόλου των γνωρισμάτων της συναρτησιακής εξάρτησης και των γνωρισμάτων του πίνακα είναι μικρότερη σε αριθμό από το πλήθος των γνωρισμάτων του πίνακα και ίση με το πλήθος των γνωρισμάτων της συναρτησιακής εξάρτησης, τότε παραβιάζεται η BCNF μορφή και ο πίνακας μπορεί να διασπαστεί
                    int x = fd.GetAll().Intersect(RelList[i].GetList(), Global.comparer).Count();
                    if (x < RelList[i].GetList().Count && x == fd.GetAll().Count)
                    {
                        //παρακάτω δημιουργούνται δύο νέοι πίνακες, ο rel1 και ο rel2

                        //ο rel1 πίνακας παίρνει τα γνωρίσματα της συναρτησιακής εξάρτησης
                        Relation rel1 = new Relation(fd.GetAll());

                        //ο rel2 πίνακας παίρνει τα γνωρίσματα από το αριστερό σκέλος της συναρτησιακής εξάρτησης, συν τα γνωρίσματα του πίνακα που διασπάστηκε, πλην αυτών που βρίσκονται στο δεξί σκέλος της συναρτησιακής εξάρτησης
                        List<Attr> temp = new List<Attr>();
                        temp.AddRange(fd.GetLeft());
                        temp.AddRange(RelList[i].GetList().Except(fd.GetRight(), Global.comparer));
                        Relation rel2 = new Relation(temp);

                        //δημιουργούνται δύο κλειδιά, ένα για τον καθένα πίνακα
                        Key key1 = new Key();
                        Key key2 = new Key();

                        //το κλειδί του πρώτου πίνακα είναι η ορίζουσα της συναρτησιακής εξάρτησης που προκάλεσε την διάσπαση
                        key1.AddToKey(fd.GetLeft());
                        rel1.SetKey(key1);

                        //προσδιορίζουμε το κλειδί του δεύτερου πίνακα (αυτό που δίνει όλα τα γνωρίσματά του)
                        //δημιουργούμε μια τοπική λίστα κλειδιών και ως κλειδί του δεύτερου πίνακα ορίζεται το πρώτο κλειδί της λίστας
                        List<Key> tempoKeyList = new List<Key>();
                        // Closure cl = new Closure(attrList, fdList);
                        // tempoKeyList = cl.KeysGet(fdList, rel2.GetList(), false);
                        var resultTemp = Global.findKeys(rel2.GetList(), fdList, false);
                        tempoKeyList = resultTemp.Item1;
                        key2.AddToKey(tempoKeyList[0].GetAttrs());
                        rel2.SetKey(key2);

                        //ορίζονται τα ονόματα των δύο νέων πινάκων
                        if (Relation.isAA)
                        {
                            rel1.Name = "R" + ++Relation.aa;
                            rel2.Name = "R" + ++Relation.aa;
                        }
                        else
                        {
                            rel1.Name = RelList[i].Name + "1";
                            rel2.Name = RelList[i].Name + "2";
                        }

                        //οι δύο νέοι πίνακες προστίθενται στη λίστα
                        RelList.Add(rel1);
                        RelList.Add(rel2);

                        // εμφανίζονται τα σχετικά μηνύματα.
                        msg += "Με την \"" + fd.ToString() + "\" ο " + RelList[i].Name + " διασπάται σε:\n\n" + rel1.ToString() + RelBCNF(rel1) + "\n\n" + rel2.ToString() + RelBCNF(rel2) + "\n\n";
                        msg += "==============================\n\n";


                        // ο πίνακας RelList[i] και η συναρτησιακή εξάρτηση fd αποκλείονται από περαιτέρω διασπάσεις
                        RelList[i].Excluded = true;
                        fd.Excluded = true;

                        newRel = true;
                        break;
                    }
                }

                //ελέγχεται αν δημιουργήθηκαν νέοι πίνακες, κι αν ναι, η σάρωση αρχίζει από την αρχή
                if (newRel)
                {
                    newRel = false;
                    i = -1;
                }
                else //ο πίνακας θεωρείται ότι είναι σε μορφή BCNF
                {
                    RelList[i].IsBCNF = true;
                    RelList[i].Excluded = true;
                }

            }
        }
    SkipProcess: //αν η διαδικασία της κανονικοποίησης λήξει πρόωρα, οδηγούμαστε σε αυτό το σημείο του κώδικα

        //επανέρχονται οι συναρτησιακές εξαρτήσεις στην αρχική τους κατάσταση
        foreach (FD fd in fdList)
            fd.Restore();

        //εμφανίζονται στην frmOut ποιοι είναι οι BCNF πίνακες
        if (RelList.Count > 0)
        {
            msg += "==============================\n\n";
            msg += "Οι πίνακες BCNF είναι οι εξής:\n\n";
            foreach (Relation relation in RelList)
                if (relation.IsBCNF)
                    msg += relation.ToString() + "\n\n";
            msg += "==============================\n\n";
        }

        //αν η διάσπαση ήταν με εναλλακτική σειρά, τότε αντιστρέφεται ξανά η σειρά των συναρτησιακών εξαρτήσεων
        if (alter)
            fdList.Reverse();

        //αν κάποια από τις συναρτησιακές εξαρτήσεις δεν χρησιμοποιήθηκε, εμφανίζεται το όνομά της και δίνεται η δυνατότητα εναλλακτικής διάσπασης
        foreach (FD fd in fdList)
            if (!fd.Excluded)
            {
                msg += "Η συναρτησιακή εξάρτηση \"" + fd.ToString() + "\" δεν χρησιμοποιήθηκε.\n\n";
                if (!alter)
                    msg += "υπάρχει ενναλακτική.";
            }
        log.InnerText = msg;
    }

    #endregion

    #region StepsDecompose

    /// <summary>
    /// Ανοίγει νέα σελίδα όπου γίνεται η υλοποίηση της σταδιακής διάσπασης.
    /// </summary>
    protected void btnStepsDecomposeClick(object sender, EventArgs e)
    {
        // Αν έχει προηγηθεί διάσπαση προηγουμένως, τότε πρέπει να αναιρεθούν τα ενδιάμεσα αποτελέσματα.Σ
        foreach (FD fd in fdList)
        {
            fd.Excluded = false;
        }

        Session["attrListSE"] = attrList;
        Session["fdListSE"] = fdList;
        //  Response.Redirect("http://ilust.uom.gr:9000/StepsDecompose.aspx");
        Response.Redirect("StepsDecompose.aspx");

    }

    #endregion


    #region Helping Methods for Actions
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
        return "    (BCNF)";
    }

    #endregion

    #endregion

    #region Schemas Management (New, Load, Save Schema)

    #region New

    /// <summary>
    /// Φορτώνει το Modal νέου σχήματος.
    /// </summary>
    protected void btnNewSchemaClick(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "modalNewSchema", "$('#modalNewSchema').modal();", true);
    }

    /// <summary>
    /// Για νέο σχήμα, ουσιαστικά κάνει reset τα δεδομένα μας.
    /// </summary>
    protected void btnNewSchemaOKClick(object sender, EventArgs e)
    {
        lblSchemaName.Text = tbxNewSchemaName.Text.Trim();
        lblSchemaDescription.Text = tbxNewSchemaDescription.Text.Trim();

        attrList.Clear();
        fdList.Clear();
        msg = "";
        log.InnerText = msg;

        ResetGridViews();
    }

    /// <summary>
    /// Αδειάζει τα GridViews και εμφανίζει εικόνες στη θέση των 2 βασικών.
    /// </summary>
    private void ResetGridViews()
    {
        gridViewAttr.DataSource = null;
        gridViewAttr.DataBind();

        gridViewFD.DataSource = null;
        gridViewFD.DataBind();

        gridViewLeftFD.DataSource = null;
        gridViewLeftFD.DataBind();

        gridViewRightFD.DataSource = null;
        gridViewRightFD.DataBind();

        gridViewEditLeftFD.DataSource = null;
        gridViewEditLeftFD.DataBind();

        gridViewEditRightFD.DataSource = null;
        gridViewEditRightFD.DataBind();

        gridViewFindClosure.DataSource = null;
        gridViewFindClosure.DataBind();

        Image1.Visible = true;
        Image2.Visible = true;
    }


    #endregion

    #region Load

    /// <summary>
    /// Μέθοδος που διαβάζει και εμφανίζει τα διαθέσιμα παραδείγματα στην λίστα για επιλογή.
    /// </summary>
    protected void btnLoadSchema_Click(object sender, EventArgs e)
    {
        // Ανάγνωση ονομάτων αρχείων με αποθηκευμένα παραδείγματα.
        string s = Directory.GetCurrentDirectory() + "/Schemas";
        string[] txtFiles = GetFileNames(s, "*.txt", false); // Μέσω της μεθόδου αποθηκεύονται τα ονόματα των αρχείων χωρίς την επέκτασή τους.

        schemaLoadDropDownList.Items.Clear(); // σβήνονται τα προηγούμενα δεδομένα (για τυχόν ανανεώσεις).

        // Φόρτωση στην λίστα.
        foreach (string st in txtFiles)
        {
            schemaLoadDropDownList.Items.Add(st);
        }

        // Εμφάνιση modal με έτοιμα παραδείγματα.
        ClientScript.RegisterStartupScript(Page.GetType(), "loadSchemaModal", "$('#loadSchemaModal').modal();", true);

    }

    /// <summary>
    /// Ελέγχεται ποιό παράδειγμα επιλέχθηκε και φορτώνεται το αντίστοιχο αρχείο.
    /// </summary>
    protected void btnLoadSelectedSchemaClick(object sender, EventArgs e)
    {
        // Φορτώνεται το αρχείο το οποίο έχει επιλεχθεί.
        string selectedSchema = schemaLoadDropDownList.SelectedValue;
        LoadSelectedSchema(selectedSchema);
    }

    /// <summary>
    /// Μέθοδος που φορτώνει το παράδειγμα με το δοσμένο όνομα.
    /// </summary>
    /// <param name="selectedSchema">Το όνομα του παραδείγματος.</param>
    private void LoadSelectedSchema(string selectedSchema)
    {
        // Φορτώνεται το αρχείο το οποίο έχει επιλεχθεί.
        string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "/Schemas/" + selectedSchema);

        int i = 0; // τρέχων γραμμή.

        // Η 1η εγγραφή πρέπει να φέρει την ένδειξη NOR.
        if (lines[i++] != "NOR")
        {
            log.InnerText = "Μη έγκυρη μορφή αρχείου σχήματος.";
            return;
        }

        // Καθαρίζω τις λίστες για να προσθέσω τα καινούργια δεδομένα.
        attrList.Clear();
        fdList.Clear();

        i++; //προσπερνάω συμβατότητα.

        // Διαβάζεται η περιγραφή του σχήματος.
        string schemaDescription = lines[i++];

        // Διαβάζεται το πλήθος των γνωρισμάτων.
        int numberOfAttributes = Int32.Parse(lines[i++]);

        // Διαβάζονται ένα προς ένα τα γνωρίσματα (όνομα και τύπος) για κάθε ένα από αυτά, δημιουργείται ένα αντικείμενο τύπου Attr.
        string sName, sType;
        for (int j = 0; j < numberOfAttributes; j++)
        {
            sName = lines[i++];
            sType = lines[i++];
            AttrCreate(sName, sType);
        }

        // Διαβάζεται το πλήθος των συναρτησιακών εξαρτήσεων και για κάθε ένα από αυτά δημιουργείται ένα αντικείμενο FD.
        int numberofFDs = Int32.Parse(lines[i++]);

        for (int j = 0; j < numberofFDs; j++)
        {
            FD fd = new FD();

            // Διαβάζεται η ένδειξη αν η συναρτησιακή εξάρτηση είναι τετριμμένη.
            fd.IsTrivial = bool.Parse(lines[i++]);

            // Προσδιορίζεται το πλήθος των γνωρισμάτων που υπάρχουν στα δύο σκέλη της συναρτησιακής εξάρτησης.
            int left;
            int right;
            left = Int32.Parse(lines[i++]);
            right = Int32.Parse(lines[i++]);

            // Προσδιορίζεται το αντικείμενο attr που αντιστοιχεί στη μεταβλητή str και προστίθεται στο αριστερό ή το δεξί σκέλος της συναρτησιακής εξάρτησης.
            Attr attrRead;
            string str;
            for (int y = 0; y < left; y++)
            {
                str = lines[i++];
                attrRead = AttrGetObject(str);
                fd.AddLeft(attrRead);
            }
            for (int y = 0; y < right; y++)
            {
                str = lines[i++];
                attrRead = AttrGetObject(str);
                fd.AddRight(attrRead);
            }
            FDCreate(fd);

        }

        // Φόρτωση γνωρισμάτων στον πίνακα γνωρισμάτων και των συναρτησιακών εξαρτήσεων.
        populateAttrGridView(attrList);
        populateFdGridView(fdList);

        lblSchemaName.Text = selectedSchema;    
        lblSchemaDescription.Text = schemaDescription;
    }

    /// <summary>
    /// Μέθοδος που διαβάζει τα ονόματα όλων των αρχείων που περιέχουν το filter σε 
    /// έναν φάκελο και τα επιστρέφει χωρίς την επέκτασή τους.
    /// </summary>
    /// <param name="path">Ο φάκελος από τον οποίο διαβάζει όλα τα αρχεία.</param>
    /// <param name="filter">Φίλτρο αναζήτησης. Μπορεί να είναι και τύπος αρχείου.</param>
    /// <param name="extension">Λογική μεταβλητή true επιστροφή με επεκτάσεις, false χωρίς.
    /// <returns>Τα ονόματα των αρχείων χωρίς την επέκταση.</returns>
    private string[] GetFileNames(string path, string filter, bool extension)
    {
        string[] files = Directory.GetFiles(path, filter); // παίρνει όλα τα διαθέσιμα αρχεία με βάση το filter.

        if (extension)
            return files;

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileName(files[i]); // αποθηκεύει μόνο τα ονόματα χωρίς τις επεκτάσεις.
        }

        return files;
    }

    /// <summary>
    /// Επιστρέφει το αντικείμενο Attr στο οποίο αντιστοιχεί το όνομα name.
    /// </summary>
    private Attr AttrGetObject(string name)
    {
        // Διασχίζεται η λίστα με τα αντικείμενα ώσπου να βρεθεί αυτό με την ονομασία name, οπότε κι επιστρέφεται.
        for (int i = 0; i < attrList.Count; i++)
            if (attrList[i].Name == name) return attrList[i];
        return null;
    }

    #endregion

    #region Save

    /// <summary>
    /// Μέθοδος που αποθηκεύει και κατεβάζει στον χρήστη το τρέχων σχήμα.
    /// </summary>
    protected void btnSaveSchema_Click(object sender, EventArgs e)
    {
        // Εδώ προστίθεται το περιεχόμενο.

        string dataForFile = "NOR\n101\n"; // αναγνωριστικό, έκδοση.
        dataForFile += lblSchemaDescription.Text.Trim() + "\n"; // περιγραφή σχήματος.
        dataForFile += attrList.Count().ToString() + "\n"; // αριθμός γνωρισμάτων.

        foreach (Attr attr in attrList)
        {
            dataForFile += attr.Name + "\n\n"; // γνωρίσματα.
        }

        dataForFile += fdList.Count().ToString() + "\n"; // αριθμών συναρτησιακών εξαρτήσεων.

        foreach (FD fd in fdList)
        {
            dataForFile += fd.Excluded.ToString() + "\n"; // αν είναι excluded ή όχι.
            dataForFile += fd.GetLeft().Count().ToString() + "\n"; // αριθμός γνωρισμάτων στο αριστερό σκέλος.
            dataForFile += fd.GetRight().Count().ToString() + "\n"; // αριθμός γνωρισμάτων στο δεξί σκέλος.
            foreach (Attr attr in fd.GetLeft())
            {
                dataForFile += attr.Name + "\n"; // γνωρίσματα στο αριστερό σκέλος.
            }
            foreach (Attr attr in fd.GetRight())
            {
                dataForFile += attr.Name + "\n"; // γνωρίσματα στο δεξί σκέλος.
            }
        }

        // Εγγραφή των δεδομένων σε αρχείο.
        string filename = lblSchemaName.Text.Trim() + ".txt"; // όνομα αρχείου

        System.IO.StreamWriter file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + "/Schemas/Students/" + filename);
        file.WriteLine(dataForFile); // εγγραφή
        file.Close();

        // Διαδικασία για download αρχείου που αποθηκεύτηκε.
        Response.ContentType = "application/octet-stream";
        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
        Response.TransmitFile(Server.MapPath("~/Schemas/Students/" + filename));
        Response.End();

    }

    #endregion

    #endregion

    #region Populate GridViews (Attr, Fd, leftFd, rightFd, editLeftFd, editRightFd, Closure)

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τα γνωρίσματα.
    /// </summary>
    /// <param name="attrList">Τα γνωρίσματα που θα φορτώσει.</param>
    private void populateAttrGridView(List<Attr> attrList)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Description", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable.Rows.Add(attr.Name, attr.Type);
        }

        gridViewAttr.DataSource = dataTable;
        gridViewAttr.DataBind();

        // Σε περίπτωση που κάνω νέο σχήμα, καλό θα είναι να σιγουρευτώ ότι οι εικόνες θα φύγουν.
        Image1.Visible = false;
    }

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τις συναρτησιακές εξαρτήσεις.
    /// </summary>
    /// <param name="fdList">Οι συναρτησιακές εξαρτήσεις που θα φορτώσει.</param>
    private void populateFdGridView(List<FD> fdList)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Description", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Trivial", typeof(string)));

        foreach (FD fd in fdList)
        {
            string trivial = "";
            if (fd.IsTrivial)
                trivial = true.ToString();

            dataTable.Rows.Add(fd.ToString(), trivial);
        }

        gridViewFD.DataSource = dataTable;
        gridViewFD.DataBind();

        // Σε περίπτωση που κάνω νέο σχήμα, καλό θα είναι να σιγουρευτώ ότι οι εικόνες θα φύγουν.
        Image2.Visible = false;
    }

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τις συναρτησιακές εξαρτήσεις αριστερού και δεξιού σκέλους.
    /// </summary>
    private void populateLeftAndRightFDGridView()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Orizouses", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable.Rows.Add(attr.Name);
        }

        gridViewLeftFD.DataSource = dataTable;
        gridViewLeftFD.DataBind();

        DataTable dataTable2 = new DataTable();
        dataTable2.Columns.Add(new DataColumn("Eksartimenes", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable2.Rows.Add(attr.Name);
        }

        gridViewRightFD.DataSource = dataTable2;
        gridViewRightFD.DataBind();
    }

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τις συναρτησιακές εξαρτήσεις αριστερού και δεξιού σκέλους για επεξεργασία.
    /// </summary>
    /// <param name="fdList">Οι συναρτησιακές εξαρτήσεις που θα φορτώσει.</param>
    private void populateLeftAndRightEditFDGridView()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Orizouses", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable.Rows.Add(attr.Name);
        }

        gridViewEditLeftFD.DataSource = dataTable;
        gridViewEditLeftFD.DataBind();

        DataTable dataTable2 = new DataTable();
        dataTable2.Columns.Add(new DataColumn("Eksartimenes", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable2.Rows.Add(attr.Name);
        }

        gridViewEditRightFD.DataSource = dataTable2;
        gridViewEditRightFD.DataBind();
    }

    /// <summary>
    /// Μέθοδος που φορτώνει τον πίνακα με τα γνωρίσματα για εύρεση εγκλεισμού.
    /// </summary>
    private void populateFindClosureGridView()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("Name", typeof(string)));

        foreach (Attr attr in attrList)
        {
            dataTable.Rows.Add(attr.Name);
        }

        gridViewFindClosure.DataSource = dataTable;
        gridViewFindClosure.DataBind();
    }

    #endregion

    #region GridViews Management (Update selected rows: Attr & Fd)

    /// <summary>
    /// Προσθέτει λειτουργικότητα στις γραμμές του gridViewAttr μόλις προστεθεί περιεχόμενο.
    /// </summary>
    protected void OnRowDataBoundAttr(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridViewAttr, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    /// <summary>
    /// Ενημερώνει την επιλεγμένη γραμμή για το gridViewAttr.
    /// </summary>
    protected void OnSelectedIndexChangedAttr(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gridViewAttr.Rows)
        {
            if (row.RowIndex == gridViewAttr.SelectedIndex)
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

    #region FOR DELETE???
    // TODO: delete??? 

    /// <summary>
    /// Φορτώνονται τα γνωρίσματα στις Λίστες επιλογής για δημιουργία FD και και επιλογή για αναζήτηση Εγκλεισμού.
    /// </summary>
    protected void updateCheckBoxLists()
    {
        //  LeftFDCheckBoxListAttrSelection.Items.Clear();
        //  RightFDCheckBoxListAttrSelection.Items.Clear();
        // ClosureCheckBoxList.Items.Clear();

        foreach (Attr attr in attrList)
        {
            //    LeftFDCheckBoxListAttrSelection.Items.Add(attr.Name);
            //   RightFDCheckBoxListAttrSelection.Items.Add(attr.Name);
            //      ClosureCheckBoxList.Items.Add(attr.Name);
        }
    }

    /// <summary>
    /// Φορτώνονται τα αντικείμενα στις αντίστοιχες λίστες και εμφανίζονται στα panel.
    /// </summary>
    /// <param name="lbox">Η λίστα στην οποία θα προστεθούν αντικείμενα με βάση το i.</param>
    /// <param name="i">0 προσθέτει γνωρίσματα και 1 συναρτησιακές εξαρτήσεις.</param>
    protected void loadListBox(ListBox lbox, int i)
    {
        if (lbox != null)
        {
            lbox.Items.Clear();
            if (i == 0)
                foreach (Attr attr in attrList)
                {
                    lbox.Items.Add(attr.Name);
                }
            else if (i == 1)
                foreach (FD fd in fdList)
                {
                    lbox.Items.Add(fd.ToString());
                }
        }
    }


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

    // TODO: Βάλε επιλογή για ανέβασμα αρχείου από τον client για φόρτωση σχήματος.
    // TODO: Βάλε δυνατότητα login για την καθηγήτρια.
    // TODO: Βάλε σύνδεση με βάση και στατιστικά στοιχεία. (Μπήκαν analytics)
    // TODO: Φτιάξε το design.
    // TODO: Αποφάσισε την μορφή εμφάνισης αποτελεσμάτων (Κινούμενο εξτρά παράθυρο;).
    // TODO: Άλλαξε τον επιστρεφόμενο τύπο από τις μεθόδους στην Global (βάλε Json για αποτελέσματα και σχόλια).
    // TODO: Βάλε εμφάνιση λαθών και επιτυχιών σε Animated Alert (διόρθωση απόκρυψης μετά από 5 δεύτερα).
    // TODO: Βάλε έλεγχο εισαγωγής στα διάφορα inputs.
    // TODO: Αναίρεση ή ενσωμάτωση enter. 
    // TODO: Διαγραφή περιτών κομματιών (κλάσσεις, μεθόδους, μεταβλητές).
    // TODO: Περιγραφή προβλήματος;; (π.χ. Τριπλή επαγωγή).
    // TODO: Κατά την έξοδο από την StepsDecompose επιστροφή στο τελευταίο ενεργό πρόβλημα.
    // TODO: Πρόβλημα συντρέχοντος εκτέλεσης??


    protected void Button2_Click(object sender, EventArgs e)
    {

        Tuple<Attr, string> temp = Global.jsonGenerator();
        string msg = temp.Item1.Name;
        msg += " " + temp.Item1.Type;

        msg += " " + temp.Item2;

        log.InnerText = msg;
    }
}