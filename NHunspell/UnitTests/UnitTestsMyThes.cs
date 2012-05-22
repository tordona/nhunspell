using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NHunspell;

[TestFixture]
public class MyThesTests
{
    [Test]
    public void ThesContainAllWordsTest()
    {
        string[] thesFiles = new string[] { "th_de_DE_v2","th_en_US_new","th_it_IT_v2" };
        foreach (string file in thesFiles)
        {
            string[] wordsInIndex = LoadIndex(file+".idx");
            Assert.Greater(wordsInIndex.Length, 1000);
            var thes = new MyThes(file+".dat");
            foreach (string word in wordsInIndex)
            {
                var result = thes.Lookup(word);
                Assert.IsNotNull(result, "Failed Word: " + word);
            }
        }
    }

    public string[] LoadIndex(string datFile)
    {
        datFile = Path.GetFullPath(datFile);
        if (!File.Exists(datFile))
        {
            throw new FileNotFoundException("Index File not found: " + datFile);
        }

        byte[] indexData;
        using (FileStream stream = File.OpenRead(datFile))
        {
            using (var reader = new BinaryReader(stream))
            {
                indexData = reader.ReadBytes((int)stream.Length);
            }
        }

        int currentPos = 0;
        int currentLength = this.GetLineLength(indexData, currentPos);

        string fileEncoding = Encoding.ASCII.GetString(indexData, currentPos, currentLength);
        Encoding enc = MyThes.GetEncoding(fileEncoding);
        currentPos += currentLength;
        List<string> result = new List<string>();
        while (currentPos < indexData.Length)
        {
            currentPos += this.GetCrLfLength(indexData, currentPos);
            currentLength = this.GetLineLength(indexData, currentPos);
            string lineText = enc.GetString(indexData, currentPos, currentLength).Trim();

            if (lineText != null && lineText.Length > 0)
            {
                string[] tokens = lineText.Split('|');
                if (tokens.Length == 2 && tokens[0].Length > 0 )
                {
                    result.Add(tokens[0]);
                }
            }

            currentPos += currentLength;
        }
        return result.ToArray();
    }

    private int GetCrLfLength(byte[] buffer, int pos)
    {
        if (buffer[pos] == 10)
        {
            if (buffer.Length > pos + 1 && buffer[pos] == 13)
            {
                return 2;
            }

            return 1;
        }

        if (buffer[pos] == 13)
        {
            if (buffer.Length > pos + 1 && buffer[pos] == 10)
            {
                return 2;
            }

            return 1;
        }

        throw new ArgumentException("buffer[pos] dosen't point to CR or LF");
    }

    private int GetLineLength(byte[] buffer, int start)
    {
        for (int i = start; i < buffer.Length; ++i)
        {
            if (buffer[i] == 10 || buffer[i] == 13)
            {
                return i - start;
            }
        }

        return buffer.Length - start;
    }

}