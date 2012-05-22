using System;
using NUnit.Framework;
using NHunspell;

[TestFixture]
public class HyphenTests
{
    [Test]
    public void CreationAndDestructionTest()
    {
        Hyphen hyphen = new Hyphen("hyph_en_us.dic");
        Assert.IsFalse(hyphen.IsDisposed);

        // Multiple dispose must be allowed
        hyphen.Dispose();
        Assert.IsTrue(hyphen.IsDisposed);
        hyphen.Dispose();
        Assert.IsTrue(hyphen.IsDisposed);
    }


    [Test]
    public void MemoryLeakTest()
    {
        // Libraries etc should be loaded two times to init the static backbone of the classes (first and repeat code path)
        // On the third run all code paths are established and all allocated memory should free  
        Hyphen hyphen = new Hyphen("hyph_en_us.dic");
        hyphen.Hyphenate("Recommendation");
        hyphen.Dispose();

        hyphen = new Hyphen("hyph_en_us.dic");
        hyphen.Hyphenate("Recommendation");
        hyphen.Dispose();


        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memoryOnBegin = GC.GetTotalMemory(true);

        hyphen = new Hyphen("hyph_en_us.dic");
        hyphen.Hyphenate("Recommendation");
        hyphen.Dispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memoryOnEnd = GC.GetTotalMemory(true);
        

        Assert.AreEqual(memoryOnBegin, memoryOnEnd);

    }

    [Test]
    public void GermanUmlautTest()
    {

        using (Hyphen hyphen = new Hyphen("hyph_de_de.dic"))
        {
            HyphenResult result;
            result = hyphen.Hyphenate("Änderung");
            Assert.IsTrue(result.HyphenatedWord == "Än=de=rung");

            result = hyphen.Hyphenate("Störung");
            Assert.IsTrue(result.HyphenatedWord == "Stö=rung");

            result = hyphen.Hyphenate("Begründung");
            Assert.IsTrue(result.HyphenatedWord == "Be=grün=dung");
        }
    }

    [Test]
    public void CyrillicLanguagesTest()
    {
        using (Hyphen hyphen = new Hyphen("hyph_ru.dic"))
        {
            HyphenResult result;
            result = hyphen.Hyphenate("Понедельник");
            Assert.IsTrue(result.HyphenatedWord == "По=не=дель=ник");
        }
    }



    [Test]
    public void UnicodeFilenameTest()
    {

        using (Hyphen hyphen = new Hyphen("hyph_de_de_ö.dic"))
        {
            HyphenResult result;
            result = hyphen.Hyphenate("Änderung");
            Assert.IsTrue(result.HyphenatedWord == "Än=de=rung");
        }
    }


    [Test]
    public void NemethTests()
    {
        using (Hyphen hyphen = new Hyphen("HyphenTests\\alt.pat"))
        {
            HyphenResult result;
            result = hyphen.Hyphenate("schiffahrt");
            Assert.IsTrue(result.HyphenatedWord == "schiff=fahrt");
        }
    }


}