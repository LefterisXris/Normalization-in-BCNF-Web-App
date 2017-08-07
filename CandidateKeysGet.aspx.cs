using Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CandidateKeysGet : System.Web.UI.Page
{
    private List<Attr> attrList = new List<Attr>(); // Λίστα με αντικείμενα Attr, για τα γνωρίσματα.
    private List<FD> fdList = new List<FD>(); // Λίστα με αντικείμενα FD, για τις συναρτησιακές εξαρτήσεις.
    private List<Key> keyList; // τα κλειδιά του πίνακα.
    private List<Relation> relList = new List<Relation>(); // λίστα με τους πίνακες Relation.

    protected void Page_Load(object sender, EventArgs e)
    {

        attrList.Add(new Attr("A", ""));
        attrList.Add(new Attr("B", ""));
        attrList.Add(new Attr("C", ""));
        attrList.Add(new Attr("D", ""));
        attrList.Add(new Attr("E", ""));

        FD fd1 = new FD();
        FD fd2 = new FD();
        FD fd3 = new FD();

        fd1.AddLeft(attrList[0]);
        fd1.AddLeft(attrList[1]);
        fd1.AddRight(attrList[2]);

        fd2.AddLeft(attrList[2]);
        fd2.AddLeft(attrList[3]);
        fd2.AddRight(attrList[4]);

        fd3.AddLeft(attrList[3]);
        fd3.AddLeft(attrList[4]);
        fd3.AddRight(attrList[1]);


        fdList.Add(fd1);
        fdList.Add(fd2);
        fdList.Add(fd3);

       
        foreach (Attr attr in attrList)
        {
            Label2.Text += attr.ToString() + ", ";
        }

        foreach (FD fd in fdList)
        {
            Label3.Text += fd.ToString() + ", ";
        }
    
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        CandidateKeys cKeys = new CandidateKeys();
        keyList = cKeys.KeysGet(fdList, attrList, true);

        foreach (Key k in keyList)
        {
            Label1.Text += k.ToString() + ", ";
        }
        Console.WriteLine("");

    }
}