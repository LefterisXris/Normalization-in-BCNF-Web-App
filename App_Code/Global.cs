using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Normalization
{
    /// <summary>
    /// Summary description for Global
    /// </summary>
    [Serializable]
    public static class Global
    {
        public static AttrComparer comparer = new AttrComparer();

        public static int MaxAttrNum = 20; //το μέγιστο πλήθος των επιτρεπόμενων γνωρισμάτων
        public static List<byte> bin = new List<byte>(); //βοηθητικός πίνακας για το δυαδικό σύστημα, μετρά το πλήθος του ψηφίου 1
        public static bool anyFDUsed; //προσδιορίζει αν χρησιμοποιήθηκε έστω και μια συναρτησιακή εξάρτηση κατά τη διαδικασία του εγκλεισμού
       
        #region Closure

        /// <summary>
        /// Καλεί την μέθοδο υπολογισμού εγκλεισμού.
        /// </summary>
        /// <param name="attrList">Λίστα με γνωρίσματα</param>
        /// <param name="fdList">Λίστα με συναρτησιακές εξαρτήσεις</param>
        /// <param name="showDetails">Λογική μεταβλητή για την εμφάνιση λεπτομερειών</param>
        /// <returns>Σύνθετο τύπο Tuple με Εγκλεισμό και string</returns>
        public static Tuple<List<Attr>, string> findClosure(List<Attr> attrList, List<FD> fdList, bool showDetails)
        {
            return attrClosure(attrList, fdList, showDetails);
        }


        /// <summary>
        /// Μέθοδος που υπολογίζει και επιστρέφει τον Εγλεισμό.
        /// </summary>
        /// <param name="attrS">Τα γνωρίσματα για τα οποία θα υπολογιστεί ο εγκλεισμός</param>
        /// <param name="fdList">Οι αντίστοιχες συναρτησιακές εξαρτήσεις</param>
        /// <param name="showOut">λογική μεταβλητή για εμφάνιση λεπτομερειών</param>
        /// <returns>Σύνθετο τύπο Tuple με Εγλεισμό και λεπτομέρειες</returns>
        private static Tuple<List<Attr>, string> attrClosure(List<Attr> attrS, List<FD> fdList, bool showOut)
        {
            string details = "";
            anyFDUsed = false; // Μηδενίζω την μεταβλητή γιατί είναι static και κρατάει την τιμή της από προηγούμενες ενέργειες.
            //ο πίνακας closure περιλαμβάνει τον εγκλεισμό των γνωρισμάτων attrS
            List<Attr> closure = new List<Attr>();
            closure.AddRange(attrS);

            details += "Διαδικασία εγκλεισμού\n";
            details += "Ο εγκλεισμός ενός συνόλου γνωρισμάτων X συμβολίζεται ως X\x207A.\n\n";
            Relation rel = new Relation(attrS);
            details += "Έστω ότι X = " + rel.ToString() + "\n\n";
            details += "Ισχύει ότι X\x207A = " + rel.ToString() + ".\n\n";
            details += "Εξετάζουμε τις συναρτησιακές εξαρτήσεις για να υπολογίσουμε τον συνολικό εγκλεισμό του X\x207A\n\n";
            details += "==============================\n\n";

        //η RepeatLoop χρησιμοποιείται ως σημείο επανεκκίνησης του βρόχου σε περίπτωση που πρέπει να ελεγχθούν από την αρχή οι συναρτησιακές εξαρτήσεις
        RepeatLoop:
            //ελέγχω μία προς μία τις συναρτησιακές εξαρτήσεις
            foreach (FD fd in fdList)
            {
                //ελέγχεται με την τομή αν τα γνωρίσματα του αριστερού σκέλους της συναρτησιακής εξάρτησης περιλαμβάνονται στον ως τώρα εγκλεισμό
                if (fd.GetLeft().Intersect(closure, Global.comparer).Count() >= fd.GetLeft().Count)
                {
                    //αν ναι, τότε προστίθενται τα γνωρίσματα του δεξιού σκέλους στην συναρτησιακή εξάρτηση, με την προϋπόθεση να μην έχουν ήδη προστεθεί, κι αν αυτό γίνει τότε η διαδικασία αρχίζει ξανά από την RepeatLoop
                    List<Attr> toAdd = new List<Attr>();
                    foreach (Attr attR in fd.GetRight())
                    {
                        if (!closure.Contains(attR, Global.comparer))
                        {
                            anyFDUsed = true;
                            toAdd.Add(attR);
                        }
                    }
                    if (toAdd.Count > 0)
                    {
                        closure.AddRange(toAdd);

                        if (toAdd.Count == 1)
                            details += "Ισχύει \"" + fd.ToString() + "\", οπότε μπαίνει στον εγκλεισμό το γνώρισμα {" + toAdd[0].Name + "}.\n\n";
                        else
                        {
                            Relation relToAdd = new Relation(toAdd);
                            details += "Ισχύει \"" + fd.ToString() + "\", οπότε μπαίνoυν στον εγκλεισμό τα γνωρίσματα " + relToAdd.ToString() + ".\n\n";
                        }

                        Relation rel2 = new Relation(closure);
                        details += "Έχουμε X\x207A = " + rel2.ToString() + "\n\n";
                        details += "==============================\n\n";

                        goto RepeatLoop;
                    }
                }
            }

            if (!showOut)
                details = "";
            var result = new Tuple<List<Attr>, string>(closure, details);
            return result;

        }

        #endregion

        #region Keys

        /// <summary>
        /// Καλεί την μέθοδο εύρεσης κλειδιών.
        /// </summary>
        /// <param name="attrList">Λίστα με γνωρίσματα</param>
        /// <param name="fdList">Λίστα με συναρτησιακές εξαρτήσεις</param>
        /// <param name="showDetails">Λογική μεταβλητή για την εμφάνιση λεπτομερειών</param>
        /// <returns>Σύνθετο τύπο Tuple με κλειδιά και string</returns>
        public static Tuple<List<Key>, string> findKeys(List<Attr> attrList, List<FD> fdList, bool showDetails)
        {
            binLoader();
            return KeysGet(fdList, attrList, showDetails);

        }

        /// <summary>
        /// Μέθοδος που υπολογίζει τα υποψήφια κλειδιά.
        /// </summary>
        /// <param name="FDList">Λίστα με συναρτησιακές εξαρτήσεις</param>
        /// <param name="newAttrList">Λίστα με γνωρίσματα</param>
        /// <param name="showOut">Λογική μεταβλητή για εμφάνιση λεπτομερειών</param>
        /// <returns>Σύνθετο τύπο Tuple με Λίστα από κλειδιά και string</returns>
        private static Tuple<List<Key>, string> KeysGet(List<FD> FDList, List<Attr> newAttrList, bool showOut)
        {
            //δημιουργείται πίνακας με τα υποψήφια κλειδιά του σχήματος
            List<Key> keyList = new List<Key>();
            string details = "";

            //δημιουργείται η frmout που αναφέρεται στην frmOut για να στέλνει πληροφορίες για τη διαδικασία της εύρεσης κλειδιών
            details += "Διαδικασία εύρεσης κλειδιών\n";
            details += "Αν ο εγκλεισμός ενός γνωρίσματος ή συνδυασμός αυτών περιλαμβάνει το σύνολο όλων των γνωρισμάτων του σχήματος, τότε το γνώρισμα αυτό, ή ο συνδυασμός των γνωρισμάτων, αποτελεί υποψήφιο κλειδί.\n\n";
            details += "Υποψήφια κλειδιά:\n\n";

            //ελέγχονται ένα προς ένα όλα τα γνωρίσματα για το κατά πόσον μπορεί να συμμετέχουν σε κλειδί
            //αν ένα γνώρισμα δεν βρίσκεται σε κανένα αριστερό σκέλος κάποιας FD τότε αποκλείεται από τον έλεγχο
            foreach (Attr attr in newAttrList)
            {
                attr.Exclude = true;
                foreach (FD fd in FDList)
                    if (fd.GetLeft().Contains(attr, Global.comparer))
                        attr.Exclude = false;

                //για να οριστικοποιηθεί ο αποκλεισμός του γνωρίσματος, πρέπει να μην συμμετέχει όμως και σε κανένα δεξί
                //κι αυτό γιατί τότε απλά προσδιορίζει τον εαυτό του, οπότε μπορεί να βρεθεί σε σύνθετο κλειδί
                //έτσι δημιουργείται μια τοπική λίστα με τα δεξιά γνωρίσματα όλων των FD
                if (attr.Exclude)
                {
                    List<Attr> attrall = new List<Attr>();
                    foreach (FD fd in FDList)
                        attrall.AddRange(fd.GetRight());

                    //αν το γνώρισμα attr υπάρχει στην τοπική λίστα με τα γνωρίσματα των δεξιών σκελών, ο αποκλεισμός του γνωρίσματος αίρεται
                    if (attrall.Contains(attr, Global.comparer)) attr.Exclude = true;
                    else attr.Exclude = false;
                    attrall = null;
                }
            }

            //ελέγχεται ο εγκλεισμός όλων των συνδυασμών των γνωρισμάτων του σχήματος κι αν αυτός περιλαμβάνει όλα τα γνωρίσματα, τότε έχουμε ένα υποψήφιο κλειδί
            //το πλήθος των γνωρισμάτων είναι n και τα εξετάζουμε σε συνδυασμούς ανά k, με τη βοήθεια της μεθόδου AttrBinarySelection
            int n, k;
            n = newAttrList.Count;
            k = 1;
            for (k = 1; k < n; k++)
                details += AttrBinarySelection(FDList, newAttrList, ref keyList, k, "", showOut);

            //αφού ολοκληρώθηκε η διαδικασία, ελέγχεται αν βρέθηκε έστω και ένα κλειδί, κι αν όχι, επιστρέφεται ως κλειδί του σχήματος ένα νέο κλειδί με όλα τα γνωρίσματα, αλλιώς μόνο τα κλειδιά που καταχωρήθηκαν στην keyList
            if (keyList.Count == 0)
            {
                Key key = new Key();
                for (int i = 0; i < newAttrList.Count; i++)
                    key.AddToKey(newAttrList[i]);
                keyList.Add(key);

                details += key.ToString() + ".\n\nΕπιλέγεται ως υποψήφιο κλειδί το σύνολο των γνωρισμάτων του σχήματος, καθώς δεν εντοπίστηκαν ως κλειδιά μεμονωμένα γνωρίσματα ή συνδυασμοί αυτών.";
            }

            if (!showOut) // Αν δεν χρειάζεται τις λεπτομέρειες τότε επιστρέφω μόνο τη λίστα με τα κλειδιά.
                details = "";

            var s = new Tuple<List<Key>, string>(keyList, details);

            return s;

        }

        /// <summary>
        /// Βοηθητική μέθοδος για τον υπολογισμό Εγκλεισμού αλλά και Υποψήφιων κλειδιών.
        /// Γενικά, για κάθε γνώρισμα ελέγχει τους πιθανούς συνδυασμούς του με άλλα.
        /// </summary>
        /// <param name="FDList">Λίστα με συναρτησιακές εξαρτήσεις</param>
        /// <param name="newAttrList">Λίστα με γνωρίσματα</param>
        /// <param name="keyList">αναφορά σε λίστα κλειδιών</param>
        /// <param name="k">παραγοντικό</param>
        /// <param name="s">μήνυμα</param>
        /// <param name="showOut">Λογική μεταβλητή για εμφάνιση λεπτομερειών</param>
        /// <returns>Το κλειδί</returns>
        private static string AttrBinarySelection(List<FD> FDList, List<Attr> newAttrList, ref List<Key> keyList, int k, string s, bool showOut)
        {
            string sBin = "";
            int i;
            int x;

            //δημιουργείται μια καθαρή λίστα γνωρισμάτων, χωρίς αυτά που είναι excluded
            List<Attr> cleanList = new List<Attr>();
            foreach (Attr attr in newAttrList)
            {
                if (!attr.Exclude)
                {
                    cleanList.Add(attr);
                }
            }

            //αν όλα τα γνωρίσματα είναι excluded ή το πλήθος των συνδυασμών k είναι μεγαλύτερο του μεγέθους της λίστας, επιστρέφεται κενή λίστα
            if (cleanList.Count == 0 | k > cleanList.Count)
            {
                return "";
            }

            //δημιουργείται μια καινούρια fdlist η οποία θα φιλοξενεί μόνο τις συναρτησιακές εξαρτήσεις που αφορούν τα γνωρίσματα της newAttrList
            //πριν γίνει αυτό, διασπώνται οι συναρτησιακές εξαρτήσεις που έχουν πολλαπλά γνωρίσματα στο δεξί σκέλος τους
            List<FD> tempFDList = new List<FD>();
            foreach (FD fd in FDList)
            {
                foreach (Attr attr in fd.GetRight())
                {
                    FD newfd = new FD();
                    newfd.AddLeft(fd.GetLeft());
                    newfd.AddRight(attr);
                    if (newfd.GetAll().Intersect(newAttrList, Global.comparer).Count() == newfd.GetAll().Count)
                    {
                        tempFDList.Add(newfd);
                    }
                }
            }

            //προσδιορίζεται το πλήθος των δυνητικών συνδυασμών.
            int maxComb;
            maxComb = Factorial((ulong)cleanList.Count) / (Factorial((ulong)k) * Factorial((ulong)(cleanList.Count - k)));

            //η αναζήτηση των συνδυασμών ξεκινά από τον πρώτο δυαδικό αριθμό που δίνει πλήρη συνδυασμό k γνωρισμάτων, κι αυτός δίνεται από τον τύπο (2^k)-1 
            i = (int)Math.Pow(2, k) - 1;
            x = 0;
            while (x < maxComb)
            {
                //ελέγχεται αν ο δυαδικός αριθμός έχει k ψηφία 1
                if (bin[i] == k)
                {
                    x++;

                    //δημιουργείται αντικείμενο τύπου Key
                    Key key = new Key();

                    //ο αριθμός i μετατρέπεται σε δυαδικό
                    sBin = Convert.ToString(i, 2);

                    //το sBin αντιστρέφεται
                    char[] ch = sBin.ToCharArray();
                    Array.Reverse(ch);
                    sBin = new string(ch);

                    //ελέγχεται η θέση των ψηφίων 1 στον δυαδικό αριθμό και προστίθενται στο κλειδί key τα αντίστοιχα γνωρίσματα
                    for (int j = 0; j < sBin.Length; j++)
                    {
                        if (sBin[j] == '1')
                        {
                            key.AddToKey(cleanList[j]);
                        }
                    }

                    //ελέγχεται αν το νέο κλειδί δεν περιλαμβάνεται ήδη στην λίστα με τα κλειδιά
                    if (key != null && !key.KeyExists(keyList))
                    {

                        //επίσης ελέγχεται αν ο εγκλεισμός του νέου κλειδιού περιλαμβάνει όλα τα γνωρίσματα του σχήματος, κι αν ναι, τότε προστίθεται στη λίστα των υποψήφιων κλειδιών
                        var result = attrClosure(key.GetAttrs(), FDList, false); // Item1 = closure.
                        if (result.Item1.Intersect(newAttrList, Global.comparer).Count() == newAttrList.Count)
                        // if (true)
                        {
                            keyList.Add(key);

                            if (showOut)
                            {
                                s += key.ToString() + "\n\n";
                            }
                        }
                    }
                }
                i++;
            }
            return s;
        }

        /// <summary>
        /// Μέθοδος που υπολογίζει το παραγοντικό ενός αριμού
        /// </summary>
        /// <param name="x">αριθμός</param>
        /// <returns>παραγοντικό</returns>
        private static int Factorial(ulong x)
        {
            ulong total = 1;
            for (ulong i = 1; i <= x; i++)
            {
                total = total * i;
            }
            return (int)total;
        }

        /// <summary>
        /// Βοηθητική μέθοδο για εύρεση κλειδιών.
        /// </summary>
        private static void binLoader()
        {
            #region γεμισμα bin
            //καταχωρούνται στον πίνακα bin το πλήθος των ψηφίων 1 στο δυαδικό σύστημα για κάθε αριθμό
            //η μεταβλητή str αναπαριστά δυαδικό αριθμό, ξεκινώντας από το 0 μέχρι το μέγιστο δυνητικό συνδυασμό, που είναι το 2 υψωμένο στο μέγιστο επιτρεπόμενο πλήθος των γνωρισμάτων
            string str;
            int kk = (int)Math.Pow(2, MaxAttrNum);
            for (int ii = 0; ii <= kk; ii++)
            {
                //ο αριθμός i μετατρέπεται σε δυαδικό
                str = Convert.ToString(ii, 2);

                //μετριέται το πλήθος των ψηφίων 1 στον δυαδικό αριθμό str και καταχωρείται στον πίνακα bin
                bin.Add((byte)str.Replace("0", "").Length);
            }
            #endregion
        }

        #endregion

        #region FOR DELETE???
        public static Tuple<Attr, string> jsonGenerator()
        {
            dynamic product = new JObject();
            product.ProductName = "Elbow Grease";
            product.Enabled = true;
            product.Price = 4.90m;
            product.StockCount = 9000;
            product.StockValue = 44100;
            product.Tags = new JArray("Real", "OnSale");
            List<string> s = new List<string>();
            s.Add("Yolo");
            s.Add("deytero");
            s.Add("Trito");

            JArray ja = new JArray();
            foreach (string ss in s)
            {
                ja.Add(ss);
            }

            product.Bla = ja;
            JArray ja2 = new JArray();
            ja2.Add("Eimai");
            ja2.Add("Eimai2");
            product.Bla2 = ja2;
            //  product.Tags2 = new JArray("Real2", "OnSale2");
            var pop = new Tuple<Attr, string>(new Attr("Yolo", "re"), product.ToString());
            return pop;


        }

        #endregion

    }
}