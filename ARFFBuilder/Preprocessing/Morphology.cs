using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ARFFBuilder.Entity;

namespace ARFFBuilder.ReportPreprocessing
{
    
    /// <summary>
    /// This class is useful for disambiguation of Czech words.
    /// It reads the lemmatas in CSTS used for the Prague Dependency Treebank project, as well as for the Czech National Corpus
    /// in very primitive way. After the class is construred the funcion GetLammata is ready to reaturn the lemmata of input token.
    /// </summary>
    public class Morphology : IMorphology
    {
        private Dictionary<string, string> lemmatas = new Dictionary<string, string>();

        public Morphology(Settings settings)
        {
            if (settings.Preprocessing.MorphologyInput != null && settings.MorfologyHasChanged) ReadLemmatas(settings.Preprocessing.MorphologyInput);
        }

        /// <summary>
        /// Reads lammata from morphology file.
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadLemmatas(string fileName)
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                string line = "";
                while((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("<f>")) continue;                
                    var m = Regex.Match(line, "<f[^>]*>([^<]+)<[^>]+>([^<]+)");
                    if (!m.Success) continue;
                    string token = m.Groups[1].Value;
                    string lemma = m.Groups[2].Value;
                    if (token == lemma) continue;
                    if (!lemmatas.ContainsKey(token)) lemmatas[token] = lemma;
                }
            }
        }

        /// <summary>
        /// GEt lemma to input token.
        /// </summary>
        /// <param name="token">Input token.</param>
        /// <returns>Lemma to input token.</returns>
        public string GetLemma(string token)
        {
            return lemmatas.ContainsKey(token) ? lemmatas[token] : token;
        }

    }
}
