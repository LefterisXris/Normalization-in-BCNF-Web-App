using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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


        public static List<Key> findKeys(List<Attr> attrList, List<FD> fdList)
        {
            binLoader();
            return KeysGet(fdList, attrList, false);
        }

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


        private static List<Key> KeysGet(List<FD> FDList, List<Attr> newAttrList, bool showOut)
        {
            //δημιουργείται πίνακας με τα υποψήφια κλειδιά του σχήματος
            List<Key> keyList = new List<Key>();

            //δημιουργείται η frmout που αναφέρεται στην frmOut για να στέλνει πληροφορίες για τη διαδικασία της εύρεσης κλειδιών
            /*   frmOut frmout = new frmOut("Διαδικασία εύρεσης κλειδιών", true, null);
               frmout.AddOut("Αν ο εγκλεισμός ενός γνωρίσματος ή συνδυασμός αυτών περιλαμβάνει το σύνολο όλων των γνωρισμάτων του σχήματος, τότε το γνώρισμα αυτό, ή ο συνδυασμός των γνωρισμάτων, αποτελεί υποψήφιο κλειδί.\n\n");
               frmout.AddOut("Υποψήφια κλειδιά:\n\n");*/

            //ελέγχονται ένα προς ένα όλα τα γνωρίσματα για το κατά πόσον μπορεί να συμμετέχουν σε κλειδί
            //αν ένα γνώρισμα δεν βρίσκεται σε κανένα αριστερό σκέλος κάποιας FD τότε αποκλείεται από τον έλεγχο
            foreach (Attr attr in newAttrList)
            {
                attr.Exclude = true;
                foreach (FD fd in FDList)
                    if (fd.GetLeft().Contains(attr))
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
                    if (attrall.Contains(attr)) attr.Exclude = true;
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
                AttrBinarySelection(FDList, newAttrList, ref keyList, k, "", showOut);

            //αφού ολοκληρώθηκε η διαδικασία, ελέγχεται αν βρέθηκε έστω και ένα κλειδί, κι αν όχι, επιστρέφεται ως κλειδί του σχήματος ένα νέο κλειδί με όλα τα γνωρίσματα, αλλιώς μόνο τα κλειδιά που καταχωρήθηκαν στην keyList
            if (keyList.Count == 0)
            {
                Key key = new Key();
                for (int i = 0; i < newAttrList.Count; i++)
                    key.AddToKey(newAttrList[i]);
                keyList.Add(key);
                //  frmout.AddOut(key.ToString() + ".\n\nΕπιλέγεται ως υποψήφιο κλειδί το σύνολο των γνωρισμάτων του σχήματος, καθώς δεν εντοπίστηκαν ως κλειδιά μεμονωμένα γνωρίσματα ή συνδυασμοί αυτών.");
            }
            //αν έχει επιλεγεί να ανοίξει η φόρμα με τη διαδικασία εύρεσης των κλειδιών, τότε ανοίγει η frmOut
            //if (showOut)
            //    frmout.ShowDialog(null);
            // frmout.Dispose();
            return keyList;
        }

        private static void AttrBinarySelection(List<FD> FDList, List<Attr> newAttrList, ref List<Key> keyList, int k, string s, bool showOut)
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
                return;
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
                    if (newfd.GetAll().Intersect(newAttrList).Count() == newfd.GetAll().Count)
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

                        if (attrClosure(key.GetAttrs(), FDList, false).Intersect(newAttrList).Count() == newAttrList.Count)
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

            System.Diagnostics.Debug.WriteLine("i = " + i);
        }

        private static List<Attr> attrClosure(List<Attr> attrS, List<FD> fdList, bool showOut)
        {
            //ο πίνακας closure περιλαμβάνει τον εγκλεισμό των γνωρισμάτων attrS
            List<Attr> closure = new List<Attr>();
            closure.AddRange(attrS);


        //η RepeatLoop χρησιμοποιείται ως σημείο επανεκκίνησης του βρόχου σε περίπτωση που πρέπει να ελεγχθούν από την αρχή οι συναρτησιακές εξαρτήσεις
        RepeatLoop:
            //ελέγχω μία προς μία τις συναρτησιακές εξαρτήσεις
            foreach (FD fd in fdList)
            {
                //ελέγχεται με την τομή αν τα γνωρίσματα του αριστερού σκέλους της συναρτησιακής εξάρτησης περιλαμβάνονται στον ως τώρα εγκλεισμό
                if (fd.GetLeft().Intersect(closure).Count() >= fd.GetLeft().Count)
                {
                    //αν ναι, τότε προστίθενται τα γνωρίσματα του δεξιού σκέλους στην συναρτησιακή εξάρτηση, με την προϋπόθεση να μην έχουν ήδη προστεθεί, κι αν αυτό γίνει τότε η διαδικασία αρχίζει ξανά από την RepeatLoop
                    List<Attr> toAdd = new List<Attr>();
                    foreach (Attr attR in fd.GetRight())
                    {
                        if (!closure.Contains(attR))
                        {
                            anyFDUsed = true;
                            toAdd.Add(attR);
                        }
                    }
                    if (toAdd.Count > 0)
                    {
                        closure.AddRange(toAdd);

                        goto RepeatLoop;
                    }
                }
            }
            return closure;

        }

        private static int Factorial(ulong x)
        {
            ulong total = 1;
            for (ulong i = 1; i <= x; i++)
            {
                total = total * i;
            }
            return (int)total;
        }


    }
}