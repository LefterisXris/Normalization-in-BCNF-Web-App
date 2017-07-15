using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Normalization
{
    /// <summary>
    /// Η κλάσση FD δημιουργεί συναρτησιακές εξαρτήσεις (functional dependency)
    /// </summary>
    [Serializable]
    public class FD : IEqualityComparer<FD>
    {
        // οι πίνακες Left και Right περιέχουν τα γνωρίσματα, που βρίσκονται στο αριστερό και το δεξί σκέλος αντίστοιχα, της συναρτησιακής εξάρτησης.
        // αντίστοιχα, οι BLeft και BRight είναι τα backup τους.
        private List<Attr> Left = new List<Attr>();
        private List<Attr> BLeft = new List<Attr>();
        private List<Attr> Right = new List<Attr>();
        private List<Attr> BRight = new List<Attr>();
        private int aa; // υποδηλώνει τον αύξων αριθμό της συναρτησιακής εξάρτησης.
        private bool isTrivial; // υποδηλώνει αν η συναρτησιακή εξάρτηση είναι τετριμμένη.
        private bool excluded; // υποδηλώνει αν η συναρτησιακή εξάρτηση αποκλείεται
        public static int count; // υποδηλώνει το πλήθος των συναρτησιακών εξαρτήσεων.

        /// <summary>
        /// Κατασκευαστής συναρτησιακής εξάρτησης.
        /// </summary>
        public FD()
        {
            count++;
            aa = count;
            Excluded = false;
        }

        /// <summary>
        /// Αντιγράφει τα περιεχόμενα της συναρτησιακής εξάρτησης fd.
        /// </summary>
        /// <param name="fd">Η συναρτησιακή εξάρτηση την οποία αντιγράφουμε.</param>
        public void Clone(FD fd)
        {
            IsTrivial = fd.isTrivial;
            Left.AddRange(fd.GetLeft());
            Right.AddRange(fd.GetRight());
        }

        /// <summary>
        /// Επιστρέφει τον αύξοντα αριθμό της συναρτησιακής εξάρτησης.
        /// </summary>
        /// <returns></returns>
        public int GetAA()
        {
            return aa;
        }

        /// <summary>
        /// Δημιουργεί backup της συναρτησιακής εξάρτησης.
        /// </summary>
        public void Backup()
        {
            BLeft.Clear();
            foreach (Attr attr in Left)
                BLeft.Add(attr);
            BRight.Clear();
            foreach (Attr attr in Right)
                BRight.Add(attr);
        }

        /// <summary>
        /// Επαναφέρει την συναρτησιακή εξάρτηση από το backup.
        /// </summary>
        public void Restore()
        {
            Reset();
            foreach (Attr attr in BLeft)
                Left.Add(attr);
            foreach (Attr attr in BRight)
                Right.Add(attr);
            Trivial();
        }

        /// <summary>
        /// Επιστρέφει true αν το γνώρισμα περιλαμβάνεται ήδη στην συναρτησιακή εξάρτηση FD
        /// </summary>
        /// <param name="attrcheck">Το αντικείμενο - γνώρισμα που ελέγχουμε.</param>
        /// <returns>Αν περιλαμβάνεται ή όχι.</returns>
        public bool CheckAttr(Attr attrcheck)
        {
            foreach (Attr attr in Left)
                if (attrcheck == attr) return true;
            foreach (Attr attr in Right)
                if (attrcheck == attr) return true;
            return false;
        }

        /// <summary>
        /// Επαναφέρει το αντικείμενο στην αρχική του μορφή.
        /// </summary>
        public void Reset()
        {
            Left.Clear();
            Right.Clear();
            isTrivial = false;
        }

        /// <summary>
        /// Προσθέτει ένα γνώρισμα τύπου Attr στο αριστερό σκέλος της FD.
        /// </summary>
        public void AddLeft(Attr attr)
        {
            // ελέγχεται αν το γνώρισμα υπάρχει ήδη στα αριστερά, κι αν όχι, τότε προστίθεται.
            if (!GetLeft().Contains(attr, Global.comparer))
            {
                Left.Add(attr);
                Trivial();
            }
        }

        /// <summary>
        /// Προσθέτει μια λίστα τύπου Attr στο αριστερό σκέλος της FD.
        /// </summary>
        public void AddLeft(List<Attr> attrList)
        {
            // προστίθενται ένα ένα τα γνωρίσματα της attrList στα αριστερά.
            foreach (Attr attr in attrList)
                AddLeft(attr);
        }

        /// <summary>
        /// Προσθέτει ένα γνώρισμα τύπου Attr στο δεξί σκέλος της FD.
        /// </summary>
        public void AddRight(Attr attr)
        {
            // ελέγχεται αν το γνώρισμα υπάρχει ήδη στα δεξιά, κι αν όχι, τότε προστίθεται.
            if (!GetRight().Contains(attr, Global.comparer))
            {
                Right.Add(attr);
                Trivial();
            }
        }

        /// <summary>
        /// Προσθέτει μια λίστα τύπου Attr στο δεξί σκέλος της FD.
        /// </summary>
        public void AddRight(List<Attr> attrList)
        {
            // προστίθενται ένα ένα τα γνωρίσματα της attrList στα δεξιά.
            foreach (Attr attr in attrList)
                AddRight(attr);
        }

        /// <summary>
        /// Αφαιρείται η λίστα τύπου Attr από το δεξί σκέλος της συναρτησιακής εξάρτησης.
        /// </summary>
        public void RemoveRight(List<Attr> attrList)
        {
            Right = Right.Except(attrList, Global.comparer).ToList();
        }

        /// <summary>
        /// Επιστρέφει την τελική μορφή της συναρτξσιακής εξάρτησης.
        /// </summary>
        public override string ToString()
        {
            // δημιουργούνται οι πίνακες LeftT και RightT.
            List<string> LeftT = new List<string>();
            List<string> RightT = new List<string>();

            // σε αυτούς τους πίνακες προστίθενται οι ονομασίες των γνωρισμάτων των πινάκων Left και Right.
            foreach (Attr attr in Left)
                LeftT.Add(attr.Name);
            foreach (Attr attr in Right)
                RightT.Add(attr.Name);

            // οι πίνακες LeftT και RightT ταξινομούνται αλφαβητικά.
            LeftT.Sort();
            RightT.Sort();

            // διαβάζονται τα περιεχόμενα των πινάκων LeftT και RightT και δημιουργείται η τελική μορφή της συναρτησιακής εξάρτησης η οποία και επιστρέφεται ως string.
            string str = "";
            string left = "";
            string right = "";

            // πρώτα του πίνακα LeftT.
            for (int i = 0; i < LeftT.Count; i++)
            {
                if (left != "") left = left + ", ";
                left = left + LeftT[i];
            }

            // μετά του πίνακα RightT
            for (int i = 0; i < RightT.Count; i++)
            {
                if (right != "") right = right + ", ";
                right = right + RightT[i];
            }

            // σχηματίζεται η τελική μορφή της συναρτησιακής εξάρτησης.
            // το "\u2192" χρησιμοποιείται για την απεικόνιση του δεξιού βέλους.
            str = left + " " + "\u2192" + " " + right;

            return str;
        }


        /// <summary>
        /// Επιστρέφει τη λίστα με τα αντικείμενα - γνωρίσματα στο αριστερό σκέλος της συναρτησιακής εξάρτησης.
        /// </summary>
        /// <returns>Το αριστερό σκέλος της FD.</returns>
        public List<Attr> GetLeft()
        {
            return Left;
        }

        /// <summary>
        /// Επιστρέφει τη λίστα με τα αντικείμενα - γνωρίσματα στο δεξί σκέλος της συναρτησιακής εξάρτησης.
        /// </summary>
        /// <returns>Το δεξι σκέλος της FD.</returns>
        public List<Attr> GetRight()
        {
            return Right;
        }

        /// <summary>
        /// Επιστρέφει τη λίστα με όλα τα γνωρίσματα της συναρτησιακής εξάρτησης.
        /// </summary>
        /// <returns></returns>
        public List<Attr> GetAll()
        {
            return Left.Concat(Right).Except(Right.Intersect(Left, Global.comparer), Global.comparer).ToList();
        }

        /// <summary>
        /// Ελέγχει αν η συναρτησιακή εξάρτηση είναι τετριμμένη ή όχι και ενημερώνει την σχετική μεταβλητή.
        /// </summary>
        public void Trivial()
        {
            //Πρέπει και τα δύο σκέλη να είναι μη κενά.
            if (Right.Count * Left.Count == 0)
            {
                IsTrivial = false;
                return;
            }
        }

        public bool Equals(FD x, FD y)
        {
            return x.GetLeft() == y.GetLeft();
            throw new NotImplementedException();
        }

        public int GetHashCode(FD obj)
        {
            return this.GetHashCode();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Προσδιορίζει αν η συναρτησιακή εξάρτηση είναι τετριμμένη.
        /// </summary>
        public bool IsTrivial
        {
            get { return isTrivial; }
            set { isTrivial = value; }
        }

        /// <summary>
        /// Προσδιορίζει αν η συναρτησιακή εξάρτηση αποκλείεται.
        /// </summary>
        public bool Excluded
        {
            get { return excluded; }
            set { excluded = value; }
        }

    }
}