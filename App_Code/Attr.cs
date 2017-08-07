using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Ctrl + M + O collaps all.  
// Ctrl + M + L expands all.
//auto format Ctrl + K , Ctrl + D   Για αυτόματη αντιστοίχηση.

namespace Normalization
{
    /// <summary>
    /// Η κλάση Attr δημιουργεί γνωρίσματα του σχήματος
    /// </summary>
    [Serializable]
    public class Attr
    {
        // μεταβλητές με τα χαρακτηριστικά του γνωρίσματος
        string name; // όνομα
        string type; // τύπος
        bool exclude; // αποκλείει τη συμμετοχή σε κλειδί

        /// <summary>
        /// Κατασκευαστής του γνωρίσματος Attr
        /// </summary>
        /// <param name="name">Ονομασία γνωρίσματος</param>
        /// <param name="type">Τύπος γνωρίσματος</param>
        public Attr(string name, string type)
        {
            // κατά τη δημιουργία του γνωρίσματος περνιούνται στις αντίστοιχες μεταβλητές η ονομασία και ο τύπος του.
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Ονομασία του γνωρίσματος
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Τύπος του γνωρίσματος
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Αποκλείει τη συμμετοχή σε κλειδί
        /// </summary>
        public bool Exclude
        {
            get { return exclude; }
            set { exclude = value; }
        }


    }

    /// <summary>
    /// Custom συγκριτής για τα αντικείμενα της κλάσσης Attr. 
    /// Χρησιμοποιείται για την Intersect.
    /// </summary>
    [Serializable]
    public class AttrComparer : IEqualityComparer<Attr>
    {

        // Δύο αντικείμενα της κλάσης Attr είναι ίσα αν έχουν το ίδιο όνομα.
        public bool Equals(Attr attrX, Attr attrY)
        {
            // Έλεγχος αν δείχνουν στα ίδια δεδομένα (αν πρόκειται δηλάδή για το ίδιο αντικείμενο).
            if (Object.ReferenceEquals(attrX, attrY))
            {
                return true;
            }

            // Έλεγχος για το αν κάποιο αντικείμενο είναι null.
            if (Object.ReferenceEquals(attrX, null) || Object.ReferenceEquals(attrY, null))
            {
                return false;
            }

            // Έλεγχος αν τα ονόματα τους είναι ίδια.
            return attrX.Name == attrY.Name;
        }

        // Εάν η μέθοδος Equals() επιστρέψει true για δύο αντικείμενα 
        // τότε η GetHashCode() θα πρέπει να επιστρέψει την ίδια τιμή για αυτά τα δύο αντικείμενα.
        public int GetHashCode(Attr attr)
        {
            /*  // Έλεγχος αν το αντικείμενο είναι null.
              if (Object.ReferenceEquals(attr, null))
              {
                  return 0;
              }

              // Υπολογισμός hash code για το όνομα του αντικειμένου, αν αυτό ΔΕΝ είναι null.
              int hashAttrName = attr.Name == null ? 0 : attr.Name.GetHashCode();

              return hashAttrName; <------------------ Comment here ------------>*/
            return 0;
        }
    }
}