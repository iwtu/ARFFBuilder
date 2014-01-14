using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Utils
{
    /// <summary>
    /// String utilizations.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Removes unnecessary punctuation.
        /// </summary>
        /// <param name="word">Word to punctuate.</param>
        /// <returns></returns>
        public static string RemovePunctuation(string word)
        {            
            int lastIndex = word.Length - 1;
            char lastChar = word[lastIndex];
            while (lastChar == ',' || lastChar == '.' || lastChar == '!' || lastChar == '?') lastChar = lastIndex-- > 0 ? word[lastIndex] : 'x';
            return word.Substring(0, lastIndex + 1);
        }

        /// <summary>
        /// Removes quotes from start and end of the word.
        /// </summary>
        /// <param name="word">Words to remove quotes.</param>
        /// <returns></returns>
        public static string TrimQuotes(string word)
        {
            int lastIndex = word.Length - 1;
            if (word[0] != '"' && word[lastIndex] != '"') return word;
            if (word[0] == '"' && word[lastIndex] == '"') return word.Substring(1, --lastIndex);
            if (word[0] == '"') return word.Substring(1, lastIndex);
            if (word[lastIndex] == '"') return word.Substring(0, lastIndex);
            return word;
        }
    }
}
