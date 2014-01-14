using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARFFBuilder.ReportPreprocessing
{
    /// <summary>
    /// Czech stemmer rewriteen from Java. 
    /// Free code are available at FIXME:
    /// </summary>
    public class CzechStemmer : IStemmer
    {
        /**
         * A buffer of the current word being stemmed
         */
        private StringBuilder sb = new StringBuilder();

        /**
         * Default constructor
         */
        

        public String Stem(String word)
        {
            
            word = word.ToLower();
            //reset string buffer
            sb.Remove(0, sb.Length);
            sb.Insert(0, word);
            // stemming...
            //removes case endings from nouns and adjectives
            RemoveCase(sb);
            //removes possesive endings from names -ov- and -in-
            removePossessives(sb);
            //removes comparative endings
            RemoveComparative(sb);
            //removes diminutive endings
            removeDiminutive(sb);
            //removes augmentatives endings
            removeAugmentative(sb);
            //removes derivational sufixes from nouns
            removeDerivational(sb);

            return sb.ToString();

        }
        private void removeDerivational(StringBuilder buffer)
        {
            int len = buffer.Length;

            string substring6 = len > 5 ? buffer.ToString().Substring(len - 6) : null;
            string substring5 = len > 4 ? buffer.ToString().Substring(len - 5) : null;
            string substring4 = len > 3 ? buffer.ToString().Substring(len - 4) : null;
            string substring3 = len > 2 ? buffer.ToString().Substring(len - 3) : null;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;
            string substring1 = len > 0 ? buffer.ToString().Substring(len - 1) : null;
            // 
            if ((len > 8) &&
                substring6 == "obinec")
            {

                buffer.Remove(len - 6, 6);
                return;
            }//len >8
            if (len > 7)
            {
                if (substring5 == "ion\u00e1\u0159")
                { // -ionář 

                    buffer.Remove(len - 4, 4);
                    Palatalise(buffer);
                    return;
                }
                if (substring5 == "ovisk" ||
                        substring5 == "ovstv" ||
                        substring5 == "ovi\u0161t" ||  //-ovišt
                        substring5 == "ovn\u00edk")
                { //-ovník

                    buffer.Remove(len - 5, 5);
                    return;
                }
            }//len>7
            if (len > 6)
            {
                if (substring4 == "\u00e1sek" || // -ásek 
                    substring4 == "loun" ||
                    substring4 == "nost" ||
                    substring4 == "teln" ||
                    substring4 == "ovec" ||
                    substring5 == "ov\u00edk" || //-ovík
                    substring4 == "ovtv" ||
                    substring4 == "ovin" ||
                    substring4 == "\u0161tin")
                { //-štin

                    buffer.Remove(len - 4, 4);
                    return;
                }
                if (substring4 == "enic" ||
                        substring4 == "inec" ||
                        substring4 == "itel")
                {

                    buffer.Remove(len - 3, 3);
                    Palatalise(buffer);
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (substring3 == "\u00e1rn")
                { //-árn

                    buffer.Remove(len - 3, 3);
                    return;
                }
                if (substring3 == "\u011bnk")
                { //-ěnk

                    buffer.Remove(len - 2, 2);
                    Palatalise(buffer);
                    return;
                }
                if (substring3 == "i\u00e1n" || //-ián
                        substring3 == "ist" ||
                        substring3 == "isk" ||
                        substring3 == "i\u0161t" || //-išt
                        substring3 == "itb" ||
                        substring3 == "\u00edrn")
                {  //-írn

                    buffer.Remove(len - 2, 2);
                    Palatalise(buffer);
                    return;
                }
                if (substring3 == "och" ||
                        substring3 == "ost" ||
                        substring3 == "ovn" ||
                        substring3 == "oun" ||
                        substring3 == "out" ||
                        substring3 == "ou\u0161")
                {  //-ouš

                    buffer.Remove(len - 3, 3);
                    return;
                }
                if (substring3 == "u\u0161k")
                { //-ušk

                    buffer.Remove(len - 3, 3);
                    return;
                }
                if (substring3 == "kyn" ||
                        substring3 == "\u010dan" ||    //-čan
                        substring3 == "k\u00e1\u0159" || //kář
                        substring3 == "n\u00e9\u0159" || //néř
                        substring3 == "n\u00edk" ||      //-ník
                        substring3 == "ctv" ||
                        substring3 == "stv")
                {

                    buffer.Remove(len - 3, 3);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (substring2 == "\u00e1\u010d" || // -áč
                    substring2 == "a\u010d" ||      //-ač
                    substring2 == "\u00e1n" ||      //-án
                        substring2 == "an" ||
                        substring2 == "\u00e1\u0159" || //-ář
                        substring2 == "as")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "ec" ||
                        substring2 == "en" ||
                        substring2 == "\u011bn" ||   //-ěn
                        substring2 == "\u00e9\u0159")
                {  //-éř

                    buffer.Remove(len - 1, 1);
                    Palatalise(buffer);
                    return;
                }
                if (substring2 == "\u00ed\u0159" || //-íř
                        substring2 == "ic" ||
                        substring2 == "in" ||
                        substring2 == "\u00edn" ||  //-ín
                        substring2 == "it" ||
                        substring2 == "iv")
                {

                    buffer.Remove(len - 1, 1);
                    Palatalise(buffer);
                    return;
                }

                if (substring2 == "ob" ||
                        substring2 == "ot" ||
                        substring2 == "ov" ||
                        substring2 == "o\u0148")
                { //-oň 

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "ul")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "yn")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "\u010dk" ||              //-čk
                        substring2 == "\u010dn" ||  //-čn
                        substring2 == "dl" ||
                        substring2 == "nk" ||
                        substring2 == "tv" ||
                        substring2 == "tk" ||
                        substring2 == "vk")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                string s = buffer.ToString();
                char lastChar = s[s.Length - 1];
                if (lastChar == 'c' ||
                   lastChar == '\u010d' || //-č
                   lastChar == 'k' ||
                   lastChar == 'l' ||
                   lastChar == 'n' ||
                   lastChar == 't')
                {

                    buffer.Remove(len - 1, 1);
                }
            }//len>3	

        }//removeDerivational

        private void removeAugmentative(StringBuilder buffer)
        {
            int len = buffer.Length;
            string substring5 = len > 4 ? buffer.ToString().Substring(len - 5) : null;
            string substring4 = len > 3 ? buffer.ToString().Substring(len - 4) : null;
            string substring3 = len > 2 ? buffer.ToString().Substring(len - 3) : null;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;
            string substring1 = len > 0 ? buffer.ToString().Substring(len - 1) : null;
            //
            if ((len > 6) &&
                 substring4 == "ajzn")
            {

                buffer.Remove(len - 4, 4);
                return;
            }
            if ((len > 5) &&
                (substring3 == "izn" ||
                 substring3 == "isk"))
            {

                buffer.Remove(len - 2, 2);
                Palatalise(buffer);
                return;
            }
            if ((len > 4) &&
                 substring2 == "\00e1k")
            { //-ák

                buffer.Remove(len - 2, 2);
                return;
            }

        }

        private void removeDiminutive(StringBuilder buffer)
        {
            int len = buffer.Length;

            string substring5 = len > 4 ? buffer.ToString().Substring(len - 5) : null;
            string substring4 = len > 3 ? buffer.ToString().Substring(len - 4) : null;
            string substring3 = len > 2 ? buffer.ToString().Substring(len - 3) : null;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;
            string substring1 = len > 0 ? buffer.ToString().Substring(len - 1) : null;
            // 
            if ((len > 7) &&
                 substring5 == "ou\u0161ek")
            {  //-oušek

                buffer.Remove(len - 5, 5);
                return;
            }
            if (len > 6)
            {
                if (substring4 == "e\u010dek" ||      //-eček
                   substring4 == "\u00e9\u010dek" ||    //-éček
                   substring4 == "i\u010dek" ||         //-iček
                   substring4 == "\u00ed\u010dek" ||    //íček
                   substring4 == "enek" ||
                   substring4 == "\u00e9nek" ||      //-ének
                   substring4 == "inek" ||
                   substring4 == "\u00ednek")
                {      //-ínek

                    buffer.Remove(len - 3, 3);
                    Palatalise(buffer);
                    return;
                }
                if (substring4 == "\u00e1\u010dek" || //áček
                     substring4 == "a\u010dek" ||   //aček
                     substring4 == "o\u010dek" ||   //oček
                     substring4 == "u\u010dek" ||   //uček
                     substring4 == "anek" ||
                     substring4 == "onek" ||
                     substring4 == "unek" ||
             substring4 == "\u00e1nek")
                {   //-ánek

                    buffer.Remove(len - 4, 4);
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (substring3 == "e\u010dk" ||   //-ečk
                   substring3 == "\u00e9\u010dk" ||  //-éčk 
                   substring3 == "i\u010dk" ||   //-ičk
                   substring3 == "\u00ed\u010dk" ||    //-íčk
                   substring3 == "enk" ||   //-enk
                   substring3 == "\u00e9nk" ||  //-énk 
                   substring3 == "ink" ||   //-ink
                   substring3 == "\u00ednk")
                {   //-ínk

                    buffer.Remove(len - 3, 3);
                    Palatalise(buffer);
                    return;
                }
                if (substring3 == "\u00e1\u010dk" ||  //-áčk
                    substring3 == "au010dk" || //-ačk
                    substring3 == "o\u010dk" ||  //-očk
                    substring3 == "u\u010dk" ||   //-učk 
                    substring3 == "ank" ||
                    substring3 == "onk" ||
                    substring3 == "unk")
                {

                    buffer.Remove(len - 3, 3);
                    return;

                }
                if (substring3 == "\u00e1tk" || //-átk
                   substring3 == "\u00e1nk" ||  //-ánk
           substring3 == "u\u0161k")
                {   //-ušk

                    buffer.Remove(len - 3, 3);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (substring2 == "ek" ||
                   substring2 == "\u00e9k" ||  //-ék
                   substring2 == "\u00edk" ||  //-ík
                   substring2 == "ik")
                {

                    buffer.Remove(len - 1, 1);
                    Palatalise(buffer);
                    return;
                }
                if (substring2 == "\u00e1k" ||  //-ák
                    substring2 == "ak" ||
                    substring2 == "ok" ||
                    substring2 == "uk")
                {

                    buffer.Remove(len - 1, 1);
                    return;
                }
            }
            if ((len > 3) &&
                 substring1 == "k")
            {

                buffer.Remove(len - 1, 1);
                return;
            }
        }//removeDiminutives

        private void RemoveComparative(StringBuilder buffer)
        {
            int len = buffer.Length;
            // 
            if (len > 5 &&
                (buffer.ToString().Substring(len - 3) == "ej\u0161" ||  //-ejš
                 buffer.ToString().Substring(len - 3) == "\u011bj\u0161"))
            {   //-ějš

                buffer.Remove(len - 2, 2);
                Palatalise(buffer);
                return;
            }

        }

        private void Palatalise(StringBuilder buffer)
        {
            int len = buffer.Length;

            string substring3 = len > 2 ? buffer.ToString().Substring(len - 3) : null;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;
            string substring1 = len > 0 ? buffer.ToString().Substring(len - 1) : null;

            if (substring2 == "ci" ||
                 substring2 == "ce" ||
                 substring2 == "\u010di" ||      //-či
                 substring2 == "\u010de")
            {   //-če

                JavaReplace(buffer, len - 2, len, "k");
                return;
            }
            if (substring2 == "zi" ||
                 substring2 == "ze" ||
                 substring2 == "\u017ei" ||    //-ži
                 substring2 == "\u017ee")
            {  //-že

                JavaReplace(buffer, len - 2, len, "h");
                return;
            }
            if (substring3 == "\u010dt\u011b" ||     //-čtě
                 substring3 == "\u010dti" ||   //-čti
                 substring3 == "\u010dt\u00ed")
            {   //-čtí

                JavaReplace(buffer, len - 3, len, "ck");
                return;
            }
            if (substring2 == "\u0161t\u011b" ||   //-ště
                substring2 == "\u0161ti" ||   //-šti
                 substring2 == "\u0161t\u00ed")
            {  //-ští


                JavaReplace(buffer, len - 2, len, "sk");
                return;
            }
            buffer.Remove(len - 1, 1);
            return;
        }//palatalise

        private void removePossessives(StringBuilder buffer)
        {
            int len = buffer.Length;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;

            if (len > 5)
            {
                if (substring2 == "ov")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "\u016fv")
                { //-ův

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "in")
                {

                    buffer.Remove(len - 1, 1);
                    Palatalise(buffer);
                    return;
                }
            }
        }//removePossessives

        private void RemoveCase(StringBuilder buffer)
        {
            int len = buffer.Length;

            string substring3 = len > 2 ? buffer.ToString().Substring(len - 3) : null;
            string substring2 = len > 1 ? buffer.ToString().Substring(len - 2) : null;
            string substring1 = len > 0 ? buffer.ToString().Substring(len - 1) : null;

            if ((len > 7) &&
                 buffer.ToString().Substring(len - 5) == "atech")
            {

                buffer.Remove(len - 5, 5);
                return;
            }//len>7
            if (len > 6)
            {
                if (buffer.ToString().Substring(len - 4) == "\u011btem")
                {   //-ětem

                    buffer.Remove(len - 3, 3);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString().Substring(len - 4) == "at\u016fm")
                {  //-atům
                    buffer.Remove(len - 4, 4);
                    return;
                }

            }
            if (len > 5)
            {
                if (substring3 == "ech" || substring3 == "ich" || substring3 == "\u00edch")
                { //-ích

                    buffer.Remove(len - 2, 2);
                    Palatalise(buffer);
                    return;
                }
                if (substring3 == ("\u00e9ho") || //-ého
                    substring3 == ("\u011bmi") ||  //-ěmu
                    substring3 == ("emi") ||
                    substring3 == ("\u00e9mu") ||  // -ému				                                                                buffer.substring( len-3,len).equals("ete")||
                    substring3 == ("eti") ||
                    substring3 == ("iho") ||
                    substring3 == ("\u00edho") ||  //-ího
                    substring3 == ("\u00edmi") ||  //-ími
                    substring3 == ("imu"))
                {

                    buffer.Remove(len - 2, 2);
                    Palatalise(buffer);
                    return;
                }
                if (substring3 == ("\u00e1ch") || //-ách
                    substring3 == ("ata") ||
                    substring3 == ("aty") ||
                    substring3 == ("\u00fdch") ||   //-ých
                    substring3 == ("ama") ||
                    substring3 == ("ami") ||
                    substring3 == ("ov\u00e9") ||   //-ové
                    substring3 == ("ovi") ||
                    substring3 == ("\u00fdmi"))
                {  //-ými

                    buffer.Remove(len - 3, 3);
                    return;
                }
            }
            if (len > 4)
            {
                if (substring2 == "em")
                {

                    buffer.Remove(len - 1, 1);
                    Palatalise(buffer);
                    return;

                }
                if (substring2 == "es" ||
                    substring2 == "\u00e9m" ||    //-ém
                    substring2 == "\u00edm")
                {   //-ím

                    buffer.Remove(len - 2, 2);
                    Palatalise(buffer);
                    return;
                }
                if (substring2 == "\u016fm")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
                if (substring2 == "at" ||
                    substring2 == "\u00e1m" ||    //-ám
                    substring2 == "os" ||
                    substring2 == "us" ||
                    substring2 == "\u00fdm" ||     //-ým
                    substring2 == "mi" ||
                    substring2 == "ou")
                {

                    buffer.Remove(len - 2, 2);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                if (substring1 == "e" || substring1 == "i")
                {

                    Palatalise(buffer);
                    return;
                }
                if (substring1 == "\u00ed" ||    //-é
                    substring1 == "\u011b")
                {   //-ě

                    Palatalise(buffer);
                    return;
                }
                if (substring1 == "u" ||
                    substring1 == "y" ||
                    substring1 == "\u016f")
                {   //-ů

                    buffer.Remove(len - 1, 1);
                    return;
                }
                if (substring1 == "a" ||
                    substring1 == "o" ||
                    substring1 == "\u00e1" ||  // -á
                    substring1 == "\u00e9" ||  //-é
                    substring1 == "\u00fd")
                {   //-ý

                    buffer.Remove(len - 1, 1);
                    return;
                }
            }//len>3
        }


        private void JavaReplace(StringBuilder sb, int start, int end, string str)
        {
            StringBuilder sb1 = new StringBuilder(sb.ToString().Substring(0, start));
            sb1.Append(str);
            sb1.Append(sb.ToString().Substring(end));
            sb = sb1;
        }


    }
}
