using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Random
{
    class Program
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static byte[] buffer = new byte[100];       

        private static Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue) return minValue;
            Int64 diff = maxValue - minValue;
            while (true)
            {
                rng.GetBytes(buffer);
                UInt32 rand = BitConverter.ToUInt32(buffer, 0);

                Int64 max = (1 + (Int64)UInt32.MaxValue);
                Int64 remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (Int32)(minValue + (rand % diff));
                }
            }
        }

        private static void MergeFilesLinesRandomly(string file1, string file2, string outputFile)
        {
            string[] lines1 = File.ReadAllLines(file1);
            string[] lines2 = File.ReadAllLines(file2);

            int i1 = 0;
            int i2 = 0;

            if (File.Exists(outputFile)) File.Delete(outputFile);
            using (StreamWriter writer = File.CreateText(outputFile))
            {
                while (i1 < lines1.Length && i2 < lines2.Length)
                {
                    int coin = Next(0, 99);
                    if (coin < 50) writer.WriteLine(lines1[i1++]); else writer.WriteLine(lines2[i2++]);                    
                }

                while (i1 < i2) writer.WriteLine(lines1[i1++]);
                while (i2 < i1) writer.WriteLine(lines2[i2++]);
            }
        }

        private void GetRandomLines(string fileName, int count, string outputFile)
        {
            string[] lines;
            HashSet<int> set = new HashSet<int>();           
            
            lines = File.ReadAllLines(fileName);
            string[] outputLines = new string[count];

            int outputIndex = 0;
            while (outputIndex != count)
            {
                int index = Next(0, lines.Length - 1);
                if (set.Contains(index)) continue;
                set.Add(index);
                outputLines[outputIndex] = lines[index];
                outputIndex++;
            }
            
            File.WriteAllLines(outputFile, outputLines);
        }

        static void Main(string[] args)
        {
            //string outputFile = string.Format("{0}\\{1}", Path.GetDirectoryName(fileName), "RandomLines.txt");            
            
        }
    }

}
