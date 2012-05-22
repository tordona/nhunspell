using System;
using NUnit.Framework;
using NHunspell;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Diagnostics;



[TestFixture]
public class SpellEngineTests
{
    [Test]
    public void CreationAndDestructionTest()
    {
        SpellEngine engine = new SpellEngine();
        Assert.IsFalse(engine.IsDisposed);

        // Multiple dispose must be allowed
        engine.Dispose();
        Assert.IsTrue(engine.IsDisposed);
        engine.Dispose();
        Assert.IsTrue(engine.IsDisposed);
    }

    [Test]
    public void FunctionsTest()
    {
        using (SpellEngine engine = new SpellEngine())
        {
            LanguageConfig enConfig = new LanguageConfig();
            enConfig.LanguageCode = "en";
            enConfig.HunspellAffFile = "en_us.aff";
            enConfig.HunspellDictFile = "en_us.dic";
            enConfig.HunspellKey = "";
            enConfig.HyphenDictFile = "hyph_en_us.dic";
            enConfig.MyThesIdxFile = "th_en_us_new.idx";
            enConfig.MyThesDatFile = "th_en_us_new.dat";
            engine.AddLanguage(enConfig);

            var sp = engine["en"];
            sp.Spell("Hello");
            sp.Suggest("Halo");
            sp.Stem("boys");
            sp.Analyze("boys");
            sp.LookupSynonyms("eingine", true);
            sp.Generate("girl", "boys");

            sp.Spell("Hello");
            sp.Suggest("Halo");
            sp.Stem("boys");
            sp.Analyze("boys");
            sp.LookupSynonyms("eingine", true);
            sp.Generate("girl", "boys");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100; ++i)
            {
                sp.Spell("Hello");
                sp.Suggest("girles");
                sp.Stem("boys");
                sp.Analyze("boys");
                sp.LookupSynonyms("eingine", true);
                sp.Generate("girl", "boys");
            }
            stopwatch.Stop();
            Console.WriteLine("Time (ms): " + stopwatch.ElapsedMilliseconds.ToString());

        }

    }

}