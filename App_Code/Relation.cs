using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Normalization
{
    /// <summary>
    /// Η κλάση Relation δημιουργεί πίνακες γνωρισμάτων, που είτε είναι μορφής BCNF, είτε όχι.
    /// </summary>
    [Serializable]
    public class Relation
    {

        public static bool isAA; // προσδιορίζει αν η αρίθμηση των πινάκων θα γίνεται με α/α ή με το σύστημα 1-2.
        public static int aa; // προσδιορίζει τον αύξοντα αριθμό του πίνακα.
        private bool BCNF; // προσδιορίζει αν ο πίνακας είναι BCNF.
        private bool excluded; // προσδιορίζει αν ο πίνακας αποκλείεται από διασπάσεις.
        private string name; // το όνομα του πίνακα.
        private Key key = new Key(); // το κλειδί του πίνακα.
        private List<Attr> attrList = new List<Attr>(); // η λίστα με τα γνωρίσματα που περιέχει ο πίνακας.

        /// <summary>
        /// Κατασκευαστής του πίνακα χωρίς παραμέτρους.
        /// </summary>
        public Relation()
        {

        }

        /// <summary>
        /// Ορίζει το κλειδί του πίνακα.
        /// </summary>
        public void SetKey(Key key)
        {
            this.key = key;
        }

        /// <summary>
        /// Επιστρέφει το κλειδί του πίνακα.
        /// </summary>
        public Key GetKey()
        {
            return key;
        }

        /// <summary>
        /// Κατασκευαστής του πίνακα με αρχικοποίηση των γνωρισμάτων.
        /// </summary>
        /// <param name="attrList"> Λίστα με όλα τα γνωρίσματα.</param>
        public Relation(List<Attr> attrList)
        {
            // Προστίθεται το περιεχόμενο της παραμέτρου στη λίστα attrList του πίνακα BCNF.
            foreach (Attr attr in attrList) AttrAdd(attr);
        }

        /// <summary>
        /// Προστίθεται ένα γνώρισμα στη λίστα attrList του πίνακα.
        /// </summary>
        /// <param name="attr"> Το γνώρισμα που εισάγεται.</param>
        public void AttrAdd(Attr attr)
        {
            // ελέγχεται αν το γνώρισμα υπάρχει ήδη στη λίστα, κι αν όχι, τότε καταχωρείται.
            if (!attrList.Contains(attr)) attrList.Add(attr);
        }

        /// <summary>
        /// Ορίζει αν ο πίνακας είναι BCNF.
        /// </summary>
        public bool IsBCNF
        {
            get { return BCNF; }
            set { BCNF = value; }
        }

        /// <summary>
        /// Ορίζει αν ο πίνακας αποκλείεται από διασπάσεις.
        /// </summary>
        public bool Excluded
        {
            get { return excluded; }
            set { excluded = value; }
        }

        /// <summary>
        /// Το όνομα του πίνακα.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Επιστρέφει τη λίστα με τα γνωρίσματα του πίνακα.
        /// </summary>
        public List<Attr> GetList()
        {
            return attrList;
        }

        public override string ToString()
        {
            // στον τοπικό πίνακα names προστίθενται το όνομα του πίνακα, τα ονόματα των γνωρισμάτων που μετέχουν στον πίνακα,
            // τα κλειδιά τους, ταξινομούνται αλφαβητικά και επιστρέφονται ως ενιαίο, μορφοποιημένο string.
            // το "\u2660" χρησιμοποιείται για την απεικόνιση του κλειδιού (μπαστουνιού)
            List<string> names = new List<string>();
            foreach (Attr attr in attrList)
            {
                if (key.GetAttrs().Contains(attr))
                    names.Add("\u2660" + attr.Name);
                //names.Add("#" + attr.Name);
                else
                    names.Add(attr.Name);
            }

            names.Sort();
            string str = string.Join(", ", names);
            str = name + "{" + str + "}";
            return str;
        }
    }
}