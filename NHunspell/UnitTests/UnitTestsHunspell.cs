using System;
using NUnit.Framework;
using NHunspell;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;



[TestFixture]
public class HunspellTests
{
    [Test]
    public void CreationAndDestructionTest()
    {
        Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic");
        Assert.IsFalse(hunspell.IsDisposed);

        // Multiple dispose must be allowed
        hunspell.Dispose();
        Assert.IsTrue(hunspell.IsDisposed);
        hunspell.Dispose();
        Assert.IsTrue(hunspell.IsDisposed);
    }

    [Test]
    public void GenerationTest()
    {
        using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
        {
            var generated = hunspell.Generate("boy", "girls");
            foreach (var gen in generated)
            {
                
            }
        }
    }


    [Test]
    public void SpellComplexWordsTest()
    {
        using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
        {
            Assert.IsTrue(hunspell.Spell("houses")); // plural
            Assert.IsTrue(hunspell.Spell("stockbroker")); // compound
        }
    }


    [Test]
    public void AddWordTest()
    {
        using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
        {
            Assert.IsFalse(hunspell.Spell("phantasievord"));
            Assert.IsTrue( hunspell.Add("phantasievord"));
            Assert.IsTrue(hunspell.Spell("phantasievord"));
            Assert.IsTrue(hunspell.Remove("phantasievord"));
            Assert.IsFalse(hunspell.Spell("phantasievord"));

            Assert.IsFalse(hunspell.Spell("phantasos"));
            Assert.IsFalse(hunspell.Spell("phantasoses"));
            Assert.IsTrue( hunspell.AddWithAffix("phantasos", "fish") );
            Assert.IsTrue(hunspell.Spell("phantasos"));
            Assert.IsTrue(hunspell.Spell("phantasoses"));
            Assert.IsTrue(hunspell.Remove("phantasos"));
            Assert.IsFalse(hunspell.Spell("phantasos"));
            Assert.IsFalse(hunspell.Spell("phantasoses"));
        }
    }


    // [Ignore("Dictionary Package (FullPackage.zip) isn't included in source code due t its size")]   
    // [Test]
    public void AllDictionariesTest()
    {
        using (ZipFile fullPackageZip = new ZipFile("..\\..\\..\\..\\Dictionaries\\FullPackage.zip"))
        {
            foreach (ZipEntry fullPackageEntry in fullPackageZip)
            {
                if (!fullPackageEntry.Name.StartsWith("hyph") && !fullPackageEntry.Name.StartsWith("thes"))
                {
                    var packageStream = fullPackageZip.GetInputStream(fullPackageEntry);
                    var packageZip = new ZipFile(packageStream);
                    foreach (ZipEntry fileEntry in packageZip)
                    {
                        try
                        {
                            if (fileEntry.Name.EndsWith(".aff") || fileEntry.Name.EndsWith(".dic"))
                            {
                                var inputStream = packageZip.GetInputStream(fileEntry);
                                var outputStream = new FileStream(fileEntry.Name, FileMode.Create);
                                byte[] buffer = new byte[4096];
                                StreamUtils.Copy(inputStream, outputStream, buffer);
                                outputStream.Flush();

                            }
                        }
                        catch (Exception e) { }

                    }

                    string filePrefix = fullPackageEntry.Name.Replace(".zip","");

                    Console.WriteLine("Dictionary: " + filePrefix);

                    using (Hunspell hunspell = new Hunspell(filePrefix + ".aff", filePrefix + ".dic"))
                    {
                        var suggest = hunspell.Suggest("interne");

                        foreach (string suggestion in suggest)
                        {
                            hunspell.Spell(suggestion);
                            
                        }
                    }
                }
            }
        }

    }


    [Test]
    public void MemoryLeakTest()
    {
        // Libraries etc should be loaded two times to init the static backbone of the classes (first and repeat code path)
        // On the third run all code paths are established and all allocated memory should free  
        Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic");
        hunspell.Spell("Recommendation");
        hunspell.Suggest("Recommendatio");
        hunspell.Dispose();

        hunspell = new Hunspell("en_us.aff", "en_us.dic");
        hunspell.Spell("Recommendation");
        hunspell.Suggest("Recommendatio"); 
        hunspell.Dispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memoryOnBegin = GC.GetTotalMemory(true);

        hunspell = new Hunspell("en_us.aff", "en_us.dic");
        hunspell.Spell("Recommendation");
        hunspell.Suggest("Recommendatio");
        hunspell.Dispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memoryOnEnd = GC.GetTotalMemory(true);

        Assert.AreEqual(memoryOnBegin, memoryOnEnd); 
    }

    [Test]
    public void GermanUmlautTest()
    {
        Hunspell hunspell = new Hunspell("de_de_frami.aff", "de_de_frami.dic");

        var correct = hunspell.Spell("Änderung");
        var suggest = hunspell.Suggest("Änderun");

        hunspell.Dispose();
    }

    [Test]
    public void UnicodeFilenameTest()
    {
        Hunspell hunspell = new Hunspell("de_de_ö_frami.aff", "de_de_ö_frami.dic");

        var correct = hunspell.Spell("Änderung");
        var suggest = hunspell.Suggest("Änderun");

        hunspell.Dispose();
    }

}