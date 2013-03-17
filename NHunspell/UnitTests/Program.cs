using System;
using System.Collections.Generic;
using NHunspell;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             var test = new SpellEngineTests();
             test.CreationAndDestructionTest();
             test.FunctionsTest();
             return;
            */
            
            
            // var test = new HyphenTests();
            // test.CreationAndDestructionTest();
            // test.MemoryLeakTest();
            // test.UnicodeFilenameTest();
            // test.GermanUmlautTest();
            // test.CyrillicLanguagesTest();
            // test.NemethTests();
            
            var test = new HunspellTests();
            // test.AllDictionariesTest();
            // test.SpellComplexWordsTest();
            test.AddWordTest();
            // test.GermanUmlautTest();
            // test.UnicodeFilenameTest();
            // test.MemoryLeakTest();

            /*
            var test = new InteropTests();
            test.Init();
            test.ArrayInteropTests();
            test.StringInteropTests();
          

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            return;
            */
            Console.WriteLine("NHunspell functions and classes demo");

            /*
            Console.WriteLine("Thesaurus with Thes");
            Thes thes = new Thes();
            thes.LoadOpenOffice("th_en_us_new.dat");
            */

            
            Console.WriteLine("");
            Console.WriteLine("Thesaurus with Thes");
            MyThes thes = new MyThes("th_en_us_new.idx", "th_en_us_new.dat");
            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {

                ThesResult result = thes.Lookup("cars", hunspell);
                foreach (ThesMeaning meaning in result.Meanings)
                {
                    Console.WriteLine("  Meaning:" + meaning.Description);
                    foreach (string synonym in meaning.Synonyms)
                    {
                        Console.WriteLine("    Synonym:" + synonym);
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Spell Check with with Hunspell");

            // Important: Due to the fact Hunspell will use unmanaged memory you have to serve the IDisposable pattern
            // In this block of code this is be done by a using block. But you can also call hunspell.Dispose()
            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {
   
                Console.WriteLine("Check if the word 'Recommendation' is spelled correct"); 
                bool correct = hunspell.Spell("Recommendation");
                Console.WriteLine("Recommendation is spelled " + (correct ? "correct" : "not correct"));

                Console.WriteLine("");
                Console.WriteLine("Make suggestions for the word 'Recommendatio'");
                List<string> suggestions = hunspell.Suggest("Recommendatio");
                Console.WriteLine("There are " + suggestions.Count.ToString() + " suggestions" );
                foreach (string suggestion in suggestions)
                {
                    Console.WriteLine("Suggestion is: " + suggestion );
                }

                Console.WriteLine("");
                Console.WriteLine("Analyze the word 'decompressed'");
                List<string> morphs = hunspell.Analyze("decompressed");
                foreach (string morph in morphs)
                {
                    Console.WriteLine("Morph is: " + morph);
                }

                Console.WriteLine("");
                Console.WriteLine("Stem the word 'decompressed'");
                List<string> stems = hunspell.Stem("decompressed");
                foreach (string stem in stems)
                {
                    Console.WriteLine("Stem is: " + stem);
                }
                /*
                for (; ; )
                {
                    Console.WriteLine("");
                    Console.WriteLine("Word1:");
                    string word = Console.ReadLine();
                    Console.WriteLine("Word2:");
                    string word2 = Console.ReadLine();

                    List<string> generated = hunspell.Generate(word, word2); // Generate("Girl","Boys");
                    foreach (string stem in generated)
                    {
                        Console.WriteLine("Generated is: " + stem);
                    }
                }
                 */
            }
           
            Console.WriteLine("");
            Console.WriteLine("Hyphenation with Hyph");

            // Important: Due to the fact Hyphen will use unmanaged memory you have to serve the IDisposable pattern
            // In this block of code this is be done by a using block. But you can also call hyphen.Dispose()
            using (Hyphen hyphen = new Hyphen("hyph_en_us.dic"))
            {
                Console.WriteLine("Get the hyphenation of the word 'Recommendation'"); 
                HyphenResult hyphenated = hyphen.Hyphenate("Recommendation");
                Console.WriteLine("'Recommendation' is hyphenated as: " + hyphenated.HyphenatedWord );

                hyphenated = hyphen.Hyphenate("eighteen");
                hyphenated = hyphen.Hyphenate("eighteen");

            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
     }
}


