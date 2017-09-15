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
using System.Configuration;
using MySql.Data.MySqlClient;

/// <summary>
/// Default: Περιλαμβάνει όλες τις λειτουργίες της Εφαρμογής.
/// </summary>
public partial class _Default : System.Web.UI.Page
{
    private List<Attr> attrList = new List<Attr>(); // Λίστα με αντικείμενα Attr, για τα γνωρίσματα.
    private List<FD> fdList = new List<FD>(); // Λίστα με αντικείμενα FD, για τις συναρτησιακές εξαρτήσεις.
    private string msg = ""; // Μεταβλητή που τυπώνει στην Logging Console. (βοηθητική) // TODO: άλλος σχεδιασμός.
    DBConnect dbConnect = new DBConnect();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Έλεγχος αν προσπαθεί να μπει ο διαχειριστής
        if (Request.QueryString["role"] == "admin")
        {
            //  Response.Redirect("http://ilust.uom.gr:9000/MemberPages/Admin.aspx");
            Response.Redirect("MemberPages/Admin.aspx");
        }

        // Αρχικοποίηση της Logging Console.
        msg += "Εδώ θα εμφανίζονται τα αποτελέσματα";
        log.InnerText = msg;

        // Φόρτωση του default σχήματος.
        if (IsPostBack == false)
        {
            if (Session["schemaName"] == null)
                LoadSelectedSchema(dbConnect.getDefaultSchemaName());
            else
            {
                string sch = (string)Session["schemaName"]; // Φορτώνω το σχήμα που είχα στην Steps Decompose.
                if (!LoadSelectedSchema(sch))
                    LoadSelectedSchema(dbConnect.getDefaultSchemaName());
            }
        }

        #region ViewStates Load
        if (ViewState["attrListVS"] != null)
            attrList = (List<Attr>)ViewState["attrListVS"];

        if (ViewState["fdListVS"] != null)
            fdList = (List<FD>)ViewState["fdListVS"];

        if (ViewState["logVS"] != null)
            msg = (string)ViewState["logVS"];

        #endregion


        // Εάν έχει συνδεθεί ο Admin τότε εμφανίζονται οι εξτρά δυνατότητες.
        setVisibleItemsForAdmin(HttpContext.Current.User.Identity.IsAuthenticated);

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

    /// <summary>
    /// Ανάλογα με το αν είναι συνδεδεμένος ένας χρήστης, εμφανίζει ή αποκρύπτει τα στοιχεία που πρέπει.
    /// </summary>
    /// <param name="visible"></param>
    private void setVisibleItemsForAdmin(bool visible)
    {
        nav.Visible = visible;
        navUsers.Visible = !visible;
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
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Το γνώρισμα δημιουργήθηκε με επιτυχία!'); $('#alertBoxSuccess').show(); ", true);
            }
            else
            {
                msg += "\nCannot create attribute: Attribute already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", "$('#alertBoxFailText').html('<strong>Σφάλμα!</strong> Αποτυχία δημιουργίας γνωρίσματος. Το γνώρισμα υπάρχει ήδη..'); $('#alertBoxFail').show();", true);
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
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Η συναρτησιακή εξάρτηση δημιουργήθηκε με επιτυχία!'); $('#alertBoxSuccess').show();", true);
        }
        else
        {
            log.InnerText = "Cannot insert FD: FD already exists..";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Σφάλμα!</strong> Αποτυχία δημιουργίας συναρτησιακής εξάρτησης. Η συναρτησιακή εξάρτηση υπάρχει ήδη..'); $('#alertBoxFail').show();", true);
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
        int index = Int32.Parse(gridViewAttrHiddenField.Value);
        if (index >= 0)
        {
            tbxEditAttrName.Text = attrList[index].Name;
            tbxEditAttrType.Text = attrList[index].Type;

            ClientScript.RegisterStartupScript(Page.GetType(), "modalEditAttribute", "$('#modalEditAttribute').modal();", true);
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε ένα γνώρισμα.'); $('#alertBoxWarning').show();", true);
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
        
        int index = Int32.Parse(gridViewAttrHiddenField.Value);
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
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Το γνώρισμα επεξεργάστηκε επιτυχώς!'); $('#alertBoxSuccess').show();", true);
            }
            else
            {
                msg += "\nCannot create attribute: Attribute already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Σφάλμα!</strong> Αδυναμία επεξεργασίας γνωρίσματος. Το γνώρισμα υπάρχει ήδη..'); $('#alertBoxFail').show();", true);
                attrList[index].Name = prevName;
            }
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε ένα γνώρισμα.'); $('#alertBoxWarning').show();", true);
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
        
        int index = Int32.Parse(gridViewFDHiddenField.Value);
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
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε μια συναρτησιακή εξάρτηση.'); $('#alertBoxWarning').show();", true);
            return;
        }
    }

    /// <summary>
    /// Η επεξεργασμένη πλέον συναρτησιακή εξάρτηση προστίθεται στην λίστα fdList.
    /// </summary>
    protected void btnEditFDΟΚClick(object sender, EventArgs e)
    {
        int index = Int32.Parse(gridViewFDHiddenField.Value);
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
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Η συναρτησιακή εξάρτηση επεξεργάστηκε επιτυχώς!'); $('#alertBoxSuccess').show();", true);
            }
            else
            {
                log.InnerText = "Cannot insert FD: FD already exists..";
                ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Σφάλμα!</strong> Αδυναμία επεξεργασίας συναρτησιακής εξάρτησης. Η συναρτησιακή εξάρτηση υπάρχει ήδη..'); $('#alertBoxFail').show();", true);
            }
        }
        else
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε μια συναρτησιακή εξάρτηση.'); $('#alertBoxWarning').show();", true);
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
        int index = Int32.Parse(gridViewAttrHiddenField.Value);
        if (index >= 0 && index <= attrList.Count && !IsAttrInFD(attrList[index]))
        {
            attrList.RemoveAt(index);
            populateAttrGridView(attrList);
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Το γνώρισμα διαγράφηκε επιτυχώς.'); $('#alertBoxSuccess').show();", true);
        }
        else if(index < 0)
        {
            log.InnerText = "You must select an attribute first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε ένα γνώρισμα.'); $('#alertBoxWarning').show();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxFail", " $('#alertBoxFailText').html('<strong>Σφάλμα!</strong> Το γνώρισμα συμμετέχει σε τουλάχιστον μια συναρτησιακή εξάρτηση και δεν μπορεί να διαγραφεί.'); $('#alertBoxFail').show();", true);
        }

    }

    /// <summary>
    /// Επιστρέφει true αν το γνώρισμα attr συμμετέχει σε κάποια από τις συναρτησιακές εξαρτήσεις
    /// </summary>
    /// <param name="attr">Το γνώρισμα που ελέγχουμε</param>
    public bool IsAttrInFD(Attr attr)
    {
        foreach (FD fd in fdList)
        {
            if (fd.GetLeft().Contains(attr, Global.comparer)) return true;
            if (fd.GetRight().Contains(attr, Global.comparer)) return true;
        }
        return false;
    }

    #endregion

    #region FD

    /// <summary>
    /// Διαγράφει την επιλεγμένη συναρτησιακή εξάρτηση από τον πίνακα.
    /// </summary>
    protected void btnDeleteFDClick(object sender, EventArgs e)
    {
        int index = Int32.Parse(gridViewFDHiddenField.Value);
        if (index >= 0)
        {
            fdList.RemoveAt(index);
            populateFdGridView(fdList);
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Η συναρησιακή εξάρτηση διαγράφηκε επιτυχώς.'); $('#alertBoxSuccess').show();", true);
        }
        else
        {
            log.InnerText = "You must select an FD first.";
            ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxWarning", " $('#alertBoxWarningText').html('<strong>Προσοχή!</strong> Πρέπει να επιλέξετε μια συναρτησιακή εξάρτηση.'); $('#alertBoxWarning').show();", true);
        }
    }

    #endregion

    #endregion

    #region ACTIONS (Closure, Keys, Decompose, StepsDecompose)

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
        dbConnect.TrackStat(lblSchemaName.Text, lblSchemaId.Text, "nClosure");

        List<Attr> attrListSelected = new List<Attr>();

        // Τα επιλεγμένα γνωρίσματα εισάγωνται στην λίστα attrListSelected.
        foreach (GridViewRow item in gridViewFindClosure.Rows)
        {
            if ((item.Cells[0].FindControl("checkBoxFindClosure") as CheckBox).Checked)
            {
                attrListSelected.Add(attrList[item.RowIndex]);
            }
        }


        var result = Global.findClosure(attrListSelected, fdList, true);
        List<Attr> attrS = result.Item1;
        msg = result.Item2;

        //δημιουργείται τοπικός πίνακας με τα ονόματα των γνωρισμάτων που περιλαμβάνονται στον εγκλεισμό
        List<string> names = new List<string>();
        foreach (Attr attr in attrS)
            names.Add(attr.Name);
        names.Sort();
        string str = string.Join(", ", names);

        if (Global.anyFDUsed)
            msg += "Με βάση τις συναρτησιακές εξαρτήσεις, ο εγκλεισμός X\x207A είναι:\n\n{" + str + "}";
        else
        {
            Relation rel = new Relation(attrListSelected);
            msg += "Καμία συναρτησιακή εξάρτηση δεν έχει ως ορίζουσα το " + rel.ToString() + ".\n\nΟ εγκλεισμός X\x207A είναι:\n\n{" + str + "}";
        }

        //εξετάζεται αν το αποτέλεσμα του εγκλεισμού καθιστά τον επιλεγμένο συνδυασμό υποψήφιο κλειδί ή υπερκλειδί
        bool superKey = false;
        if (names.Count == attrList.Count)
        {
            List<Key> tempoList = new List<Key>();
            var result2 = Global.findKeys(attrList, fdList, false);
            tempoList = result2.Item1;
            Key tempoKey = new Key();

            foreach (Key key in tempoList)
            {
                if (attrListSelected.Count > key.GetAttrs().Count() && key.GetAttrs().Count == key.GetAttrs().Intersect(attrList, Global.comparer).Count())
                {
                    superKey = true;
                    tempoKey = key;
                    break;
                }
            }
            if (superKey)
                msg += "\n\nεπομένως το X αποτελεί υπερκλειδί αφού περιλαμβάνει όλα τα γνωρίσματα και είναι υπερσύνολο του υποψήφιου κλειδιού " + tempoKey.ToString() + " του R.";
            else
                msg += "\n\nεπομένως το X αποτελεί υποψήφιο κλειδί αφού περιλαμβάνει όλα τα γνωρίσματα του R.";

        }

        log.InnerText = msg;
        resultTitle.Text = "Διαδικασία Εγλεισμού";
        OpenResultsModal(false);
    }

    #endregion

    #region CalculateKeys

    /// <summary>
    /// Υπολογίζει τα υποψήφια κλειδιά του σχήματος.
    /// </summary>
    protected void btnCalculateKeysClick(object sender, EventArgs e)
    {
        dbConnect.TrackStat(lblSchemaName.Text, lblSchemaId.Text, "nFindKeys");

        List<Key> keyList = new List<Key>();
        var result = Global.findKeys(attrList, fdList, true);

        keyList = result.Item1; // Είναι η λίστα με τα κλειδιά.

        msg = result.Item2.ToString(); // Είναι οι λεπτομέρειες.

        log.InnerText = msg;
        resultTitle.Text = "Διαδικασία Εύρεσης Κλειδιών";
        OpenResultsModal(false);
    }

    #endregion

    #region Decompose

    /// <summary>
    /// Μέθοδος διάσπασης σε πίνακες BCNF
    /// </summary>
    protected void btnDecomposeClick(object sender, EventArgs e)
    {
        dbConnect.TrackStat(lblSchemaName.Text, lblSchemaId.Text, "nDecompose");
        
        btnDecomposeAlternative.Visible = false;
        bool isAlternative = false;
        Button btn = (Button)sender;

        // Εάν έχει πατηθεί το πλήκτρο για εναλλακτική διάσπαση.
        if (btn.ID.Equals("btnDecomposeAlternative"))
        {
            isAlternative = true;

            var resultAlter = Global.Decompose(attrList, fdList, isAlternative);
            logAlter.InnerText = resultAlter.Item1;

           // resultModalSize.Attributes["class"] = "modal-dialog modal-lg";
        }
        else
        {
            var result = Global.Decompose(attrList, fdList, isAlternative);
            msg = result.Item1;
            

            if (result.Item2)
                btnDecomposeAlternative.Visible = true;
        }

        log.InnerText = msg;
        resultTitle.Text = "Διαδικασία Διάσπασης σε BCNF";
        OpenResultsModal(isAlternative);
    }

    #endregion

    #region StepsDecompose

    /// <summary>
    /// Ανοίγει νέα σελίδα όπου γίνεται η υλοποίηση της σταδιακής διάσπασης.
    /// </summary>
    protected void btnStepsDecomposeClick(object sender, EventArgs e)
    {
        dbConnect.TrackStat(lblSchemaName.Text, lblSchemaId.Text, "nStepsDecompose");

        // Αν έχει προηγηθεί διάσπαση προηγουμένως, τότε πρέπει να αναιρεθούν τα ενδιάμεσα αποτελέσματα.Σ
        foreach (FD fd in fdList)
        {
            fd.Excluded = false;
        }

        Session["attrListSE"] = attrList;
        Session["fdListSE"] = fdList;
        Session["schemaName"] = lblSchemaName.Text;
        Session["schemaDescription"] = lblSchemaDescription.Text;
        //  Response.Redirect("http://ilust.uom.gr:9000/StepsDecompose.aspx");
        Response.Redirect("StepsDecompose.aspx");

    }

    #endregion


    #region Helping Methods for Actions

    /// <summary>
    /// Ανοίγει το modal με τα αποτελέσματα.
    /// </summary>
    private void OpenResultsModal(bool isAlternative)
    {
        logAlter.Visible = isAlternative;
        lblAlter.Visible = isAlternative;
        
        ClientScript.RegisterStartupScript(Page.GetType(), "modalResults", "$('#modalResults').modal();", true);
    }

    #endregion

    #endregion

    #region Schemas Management (New, Load Schema)

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

        dbConnect.saveNewSchemaOnDB(lblSchemaName.Text); // Αποθήκευση ονόματος στην ΒΔ.
        lblSchemaId.Text = dbConnect.getSchemaId(lblSchemaName.Text).ToString();

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
        List<string> availableFiles = dbConnect.getSchemaNames();

        schemaLoadDropDownList.Items.Clear(); // σβήνονται τα προηγούμενα δεδομένα (για τυχόν ανανεώσεις).

        // Φόρτωση στην λίστα.
        foreach (string st in availableFiles)
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

        if (!LoadSelectedSchema(selectedSchema))
            if (!LoadSelectedSchema(selectedSchema+".txt"))
                LoadSelectedSchema(dbConnect.getDefaultSchemaName());
        
    }

    /// <summary>
    /// Μέθοδος που φορτώνει το παράδειγμα με το δοσμένο όνομα.
    /// </summary>
    /// <param name="selectedSchema">Το όνομα του παραδείγματος.</param>
    private bool LoadSelectedSchema(string selectedSchema)
    {
        lblSchemaId.Text = dbConnect.getSchemaId(selectedSchema).ToString();
        dbConnect.TrackStat(selectedSchema, lblSchemaId.Text, "nLoad");

        string[] lines;
        try
        {
            // Φορτώνεται το αρχείο το οποίο έχει επιλεχθεί.
            lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "/Schemas/" + selectedSchema);
        }
        catch (Exception e)
        {
            return false;
        }


        int i = 0; // τρέχων γραμμή.

        // Η 1η εγγραφή πρέπει να φέρει την ένδειξη NOR.
        if (lines[i++] != "NOR")
        {
            log.InnerText = "Μη έγκυρη μορφή αρχείου σχήματος.";
            return false;
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
        
        return true;
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

    #region Admin controls (Save, SetDefault)

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
            dataForFile += attr.Name + "\n"; // γνωρίσματα.
            dataForFile += attr.Type + "\n"; // τύπος 
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


        dbConnect.updateSchemaByAdmin(lblSchemaId.Text);

        // Εγγραφή των δεδομένων σε αρχείο.
        string filename = lblSchemaName.Text.Trim() + ".txt"; // όνομα αρχείου

        System.IO.StreamWriter file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + "/Schemas/" + filename);
        file.WriteLine(dataForFile); // εγγραφή
        file.Close();

        // Διαδικασία για download αρχείου που αποθηκεύτηκε.
        Response.ContentType = "application/octet-stream";
        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
        Response.TransmitFile(Server.MapPath("~/Schemas/" + filename));
        Response.End();


    }

    #endregion

    #region SetDefault

    /// <summary>
    /// Μέθοδος που εμφανίζει το Modal για επιλογή προεπιλεγμένου σχήματος.
    /// </summary>
    protected void btnSetDefaultSchemaSelect(object sender, EventArgs e)
    {
        List<string> availableFiles = dbConnect.getSchemaNames();

        SetSchemaDefaultDropDownList.Items.Clear(); // σβήνονται τα προηγούμενα δεδομένα (για τυχόν ανανεώσεις).

        // Φόρτωση στην λίστα.
        foreach (string st in availableFiles)
        {
            SetSchemaDefaultDropDownList.Items.Add(st);
        }

        // Εμφάνιση modal με διαθέσιμα σχήματα.
        ClientScript.RegisterStartupScript(Page.GetType(), "SetDefaultSchemaModal", "$('#SetDefaultSchemaModal').modal();", true);
    }

    /// <summary>
    /// Θέτει το αρχείο ως προεπιλεγμένο στην ΒΔ.
    /// </summary>
    protected void btnSetDefaultSchemaClick(object sender, EventArgs e)
    {
        // Θέτω το αρχείο το οποίο έχει επιλεγεί ως προεπιλεγμένο.
        string selectedSchema = SetSchemaDefaultDropDownList.SelectedValue;

        dbConnect.setDefaultSchema(selectedSchema);

        ClientScript.RegisterStartupScript(Page.GetType(), "alertBoxSuccess", " $('#alertBoxSuccessText').html('<strong>Επιτυχία!</strong> Το προεπιλεγμένο σχήμα άλλαξε!'); $('#alertBoxSuccess').show(); ", true);
    }

    #endregion

    #endregion



    // TODO: Διαγραφή περιτών κομματιών (κλάσεις, μεθόδους, μεταβλητές).
    // TODO: Πρόβλημα συντρέχοντος εκτέλεσης??
    // TODO: Change DBqueries with prepare.
    

    protected void btnGetSchemasClick(object sender, EventArgs e)
    {
        
        

        List<string> list = null;
       // list = dbConnect.Query("SELECT * FROM `Schemas`");

        if (list != null)
            log.InnerText = "Yeah";    
        else
            log.InnerText = "dfhfd";

        string mmm = "";
        /* foreach (List<string> ll in list)
         {
             foreach (string s in ll)
                 mmm += s + ",  ";
             mmm += "\n\n";
         }*/
            
        foreach (string s in list)
            mmm += s + ",  ";

        log.InnerText = mmm;

        OpenResultsModal(false);
    }

}