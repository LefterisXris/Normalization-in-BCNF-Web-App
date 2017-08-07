using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normalization
{
    [Serializable]
    public class Closure
    {
        private List<Attr> attrList; //πίνακας attrList όπου αποθηκεύονται τα γνωρίσματα - αντικείμενα τύπου Attr
        private List<FD> FDList; //πίνακας FDList όπου αποθηκεύονται οι συναρτησιακές εξαρτήσεις - αντικείμενα τύπου FD
        private bool anyFDUsed; //προσδιορίζει αν χρησιμοποιήθηκε έστω και μια συναρτησιακή εξάρτηση κατά τη διαδικασία του εγκλεισμού
        private string Msg = "";
        private string Msg2 = "";
        private List<byte> bin = new List<byte>(); //βοηθητικός πίνακας για το δυαδικό σύστημα, μετρά το πλήθος του ψηφίου 1
        public static int MaxAttrNum = 20; //το μέγιστο πλήθος των επιτρεπόμενων γνωρισμάτων
       



        /// <summary>
        /// Κατασκευαστής της Closure που υπολογίζει τον εγκλεισμό των γνωρισμάτων.
        /// </summary>
        /// <param name="attrList">Πίνακας των γνωρισμάτων</param>
        /// <param name="FDList">Πίνακας των συναρτησιακών εξαρτήσεων</param>
        public Closure(List<Attr> attrList, List<FD> FDList)
        {
            this.attrList = attrList;
            this.FDList = FDList;

            //καταχωρούνται στον πίνακα bin το πλήθος των ψηφίων 1 στο δυαδικό σύστημα για κάθε αριθμό
            //η μεταβλητή str αναπαριστά δυαδικό αριθμό, ξεκινώντας από το 0 μέχρι το μέγιστο δυνητικό συνδυασμό, που είναι το 2 υψωμένο στο μέγιστο επιτρεπόμενο πλήθος των γνωρισμάτων
            string str;
            int kk = (int)Math.Pow(2, MaxAttrNum);
            for (int i = 0; i <= kk; i++)
            {
                //ο αριθμός i μετατρέπεται σε δυαδικό
                str = Convert.ToString(i, 2);

                //μετριέται το πλήθος των ψηφίων 1 στον δυαδικό αριθμό str και καταχωρείται στον πίνακα bin
                bin.Add((byte)str.Replace("0", "").Length);
            }
        }


        public List<Attr> attrClosure(List<Attr> attrS, bool showOut)
        {
            //ο πίνακας closure περιλαμβάνει τον εγκλεισμό των γνωρισμάτων attrS
            List<Attr> closure = new List<Attr>();
            closure.AddRange(attrS);

            if (showOut)
            {
                Relation rel = new Relation(attrS);
                Msg += "Ο εγκλεισμός ενός συνόλου γνωρισμάτων X συμβολίζεται ως X\x207A.\n\n";
                Msg += "Έστω ότι X = " + rel.ToString() + "\n\n";
                Msg += "Ισχύει ότι X\x207A = " + rel.ToString() + ".\n\n";
                Msg += "Εξετάζουμε τις συναρτησιακές εξαρτήσεις για να υπολογίσουμε τον συνολικό εγκλεισμό του X\x207A\n\n";
                Msg += "==============================\n\n";
            }
            Stopwatch sw1, sw2, sw3, sw4, sw5;sw5 = Stopwatch.StartNew();
        //η RepeatLoop χρησιμοποιείται ως σημείο επανεκκίνησης του βρόχου σε περίπτωση που πρέπει να ελεγχθούν από την αρχή οι συναρτησιακές εξαρτήσεις
        RepeatLoop:
            sw4 = Stopwatch.StartNew();
            //ελέγχω μία προς μία τις συναρτησιακές εξαρτήσεις
            foreach (FD fd in FDList)
            {
                sw3 = Stopwatch.StartNew();
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
                    sw2 = Stopwatch.StartNew();
                    if (toAdd.Count > 0)
                    {
                        closure.AddRange(toAdd);
                        sw1 = Stopwatch.StartNew();
                        if (showOut)
                        {
                            if (toAdd.Count == 1)
                            {
                                Msg += "Ισχύει \"" + fd.ToString() + "\", οπότε μπαίνει στον εγκλεισμό το γνώρισμα {" + toAdd[0].Name + "}.\n\n";
                            }
                            else
                            {
                                Relation relToAdd = new Relation(toAdd);
                                Msg += "Ισχύει \"" + fd.ToString() + "\", οπότε μπαίνoυν στον εγκλεισμό τα γνωρίσματα " + relToAdd.ToString() + ".\n\n";
                            }

                            Relation rel = new Relation(closure);
                            Msg += "Έχουμε X\x207A = " + rel.ToString() + "\n\n";
                            Msg += "==============================\n\n";
                        }sw1.Stop();
                        goto RepeatLoop;
                    }sw2.Stop();
                }sw3.Stop();
            }sw4.Stop();sw5.Stop();
            return closure;

        }

        public string btnOK_Click(List<Attr> attrS)
        {
            string finalMessage = "";
            //δημιουργείται πίνακας με τα επιλεγμένα γνωρίσματα
            //List<Attr> attrS = new List<Attr>();

            List<Attr> attrS2 = new List<Attr>();
            anyFDUsed = false;

            //αν δεν επιλέχθηκε κανένα γνώρισμα, η διαδικασία ακυρώνεται
            if (attrS.Count == 0)
            {
                finalMessage += "Δεν έχετε επιλέξει κανένα γνώρισμα.";
            }
            //αλλιώς εκτελείται η διαδικασία του εγκλεισμού και εμφανίζονται τα αποτελέσματά του
            else
            {
                attrS = attrClosure(attrS, true);

                //δημιουργείται τοπικός πίνακας με τα ονόματα των γνωρισμάτων που περιλαμβάνονται στον εγκλεισμό
                List<string> names = new List<string>();
                foreach (Attr attr in attrS)
                    names.Add(attr.Name);
                names.Sort();
                string str = string.Join(", ", names);

                finalMessage = "";
                if (anyFDUsed)
                {
                    finalMessage = "Με βάση τις συναρτησιακές εξαρτήσεις, ο εγκλεισμός X\x207A είναι:\n\n{" + str + "}";
                }
                else
                {
                    Relation rel = new Relation(attrS2);
                    finalMessage = "Καμία συναρτησιακή εξάρτηση δεν έχει ως ορίζουσα το " + rel.ToString() + ".\n\nΟ εγκλεισμός X\x207A είναι:\n\n{" + str + "}";
                }

                //εξετάζεται αν το αποτέλεσμα του εγκλεισμού καθιστά τον επιλεγμένο συνδυασμό υποψήφιο κλειδί ή υπερκλειδί
                bool superkey = false;
                if (names.Count == attrList.Count)
                {
                    List<Key> tempList = new List<Key>();
                    tempList = KeysGet(FDList, attrList, false);

                    Key tempKey = new Key();

                    foreach (Key key in tempList)
                    {
                        if (attrS2.Count > key.GetAttrs().Count() && key.GetAttrs().Count == key.GetAttrs().Intersect(attrS2, Global.comparer).Count())
                        {
                            superkey = true;
                            tempKey = key;
                            break;
                        }
                    }
                    if (superkey)
                    {
                        finalMessage += "\n\nεπομένως το X αποτελεί υπερκλειδί αφού περιλαμβάνει όλα τα γνωρίσματα και είναι υπερσύνολο του υποψήφιου κλειδιού " + tempKey.ToString() + " του R.";
                    }
                    else
                    {
                        finalMessage += "\n\nεπομένως το X αποτελεί υποψήφιο κλειδί αφού περιλαμβάνει όλα τα γνωρίσματα του R.";
                    }
                }


            }
            return finalMessage;
        }

        /// <summary>
        /// Επιστρέφει τα υποψήφια κλειδιά του σχήματος.
        /// </summary>
        /// <param name="newAttrList">Η λίστα των γνωρισμάτων μέσα στα οποία θα αναζητηθούν τα κλειδιά</param>
        /// <param name="showOut">Προσδιορίζει αν τα αποτελέσματα θα φανούν στην οθόνη</param>
        /// <returns></returns>
        public List<Key> KeysGet(List<FD> FDList, List<Attr> newAttrList, bool showOut)
        {
            //δημιουργείται πίνακας με τα υποψήφια κλειδιά του σχήματος
            List<Key> keyList = new List<Key>();

            //δημιουργείται η frmout που αναφέρεται στην frmOut για να στέλνει πληροφορίες για τη διαδικασία της εύρεσης κλειδιών
            Msg2 += "Διαδικασία εύρεσης κλειδιών\n\n";
            Msg2 += "Αν ο εγκλεισμός ενός γνωρίσματος ή συνδυασμός αυτών περιλαμβάνει το σύνολο όλων των γνωρισμάτων του σχήματος, τότε το γνώρισμα αυτό, ή ο συνδυασμός των γνωρισμάτων, αποτελεί υποψήφιο κλειδί.\n\n";
            Msg2 += "Υποψήφια κλειδιά:\n\n";
            
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
                    List<Attr> attrAll = new List<Attr>();
                    foreach (FD fd in FDList)
                        attrAll.AddRange(fd.GetRight());

                    //αν το γνώρισμα attr υπάρχει στην τοπική λίστα με τα γνωρίσματα των δεξιών σκελών, ο αποκλεισμός του γνωρίσματος αίρεται
                    if (attrAll.Contains(attr, Global.comparer))
                        attr.Exclude = true;
                    else
                        attr.Exclude = false;
                    attrAll = null;
                }
            }
        
            int n, k;
            n = newAttrList.Count;
            k = 1;
            int ss = 0;
            
            for (k = 1; k < n; k++)
            {
                // point of disaster!!!! //TODO:change!
                AttrBinarySelection(FDList, newAttrList, ref keyList, k, Msg2, showOut); 
                ss++; // for debugging only
            }
            
            //αφού ολοκληρώθηκε η διαδικασία, ελέγχεται αν βρέθηκε έστω και ένα κλειδί, κι αν όχι, επιστρέφεται ως κλειδί του σχήματος ένα νέο κλειδί με όλα τα γνωρίσματα, αλλιώς μόνο τα κλειδιά που καταχωρήθηκαν στην keyList
            if (keyList.Count == 0)
            {
                Key key = new Key();
                for (int i = 0; i < newAttrList.Count; i++)
                {
                    key.AddToKey(newAttrList[i]);
                }
                keyList.Add(key);
                Msg2 += key.ToString() + ".\n\nΕπιλέγεται ως υποψήφιο κλειδί το σύνολο των γνωρισμάτων του σχήματος, καθώς δεν εντοπίστηκαν ως κλειδιά μεμονωμένα γνωρίσματα ή συνδυασμοί αυτών.";
            }

            return keyList;
        }

        /// <summary>
        /// Επιστρέφει πίνακα με όλους τους συνδυασμούς γνωρισμάτων ανά k
        /// </summary>
        /// <param name="k">Το επιθυμητό πλήθος των γνωρισμάτων ανά συνδυασμό</param>
        private void AttrBinarySelection(List<FD> FDList, List<Attr> newAttrList, ref List<Key> keyList, int k, string s, bool showOut)
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
                        Closure frm = new Closure(key.GetAttrs(), tempFDList);
                        if (frm.attrClosure(key.GetAttrs(), false).Intersect(newAttrList, Global.comparer).Count() == newAttrList.Count)
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

        /// <summary>
        /// Επιστρέφει την πραγοντική τιμή του x.
        /// </summary>
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
