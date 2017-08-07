using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Normalization
{
    /// <summary>
    /// Η κλάση Key δημιουργεί κλειδιά που περιλαμβάνουν γνωρίσματα (μονά ή και συνδυασμούς).
    /// </summary>
    [Serializable]
    public class Key
    {
        // ο πίνακας keyAttrs περιέχει τα γνωρίσματα που περιλαμβάνει το κλειδί.
        private List<Attr> keyAttrs = new List<Attr>();

        /// <summary>
        /// Προσθέτει ένα γνώρισμα τύπου Attr στο κλειδί.
        /// </summary>
        public void AddToKey(Attr attr)
        {
            keyAttrs.Add(attr);
        }

        /// <summary>
        /// Προσθέτει μια λίστα από γνωρίσματα στο κλειδί.
        /// </summary>
        public void AddToKey(List<Attr> list)
        {
            // ελέγχεται αν τα κλειδιά υπάρχουν ήδη στον πίνακα, κι αν όχι, προστίθενται.
            foreach (Attr attr in list)
                if (!keyAttrs.Contains(attr))
                    AddToKey(attr);
        }

        /// <summary>
        /// Επιστρέφει τον πίνακα με τα γνωρίσματα που περιλαμβάνονται στο κλειδί Key.
        /// </summary>
        public List<Attr> GetAttrs()
        {
            return keyAttrs;
        }

        /// <summary>
        /// Ελέγχει αν το κλειδί περιλαμβάνεται ήδη σαν υποσύνολο στον πίνακα KeyList
        /// </summary>
        public bool KeyExists(List<Key> keyList)
        {
            foreach (Key keysearch in keyList)
            {
                if (keysearch.GetAttrs().Intersect(keyAttrs).Count() >= keysearch.GetAttrs().Count)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Επιστρέφει τα γνωρίσματα του κλειδιού
        /// </summary>
        public override string ToString()
        {
            List<string> names = new List<string>();
            foreach (Attr attr in keyAttrs)
            {
                names.Add(attr.Name);
            }
            names.Sort();
            String str = string.Join(", ", names);
            str = "{" + str + "}";
            return str;
        }




    }
}