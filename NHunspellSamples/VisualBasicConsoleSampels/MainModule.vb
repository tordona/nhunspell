Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NHunspell

Module MainModule
    Sub Main()
        ' Important: Due to the fact Hunspell will use unmanaged memory you have to serve the IDisposable pattern
        ' In this block of code this is be done by a using block. But you can also call hunspell.Dispose()

        Console.WriteLine("NHunspell functions demonstration")
        Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")
        Console.WriteLine()

        Using hunspell As New Hunspell("en_us.aff", "en_us.dic")
            Console.WriteLine("Hunspell - Spell Checking Functions")
            Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")

            Console.WriteLine("Check if the word 'Recommendation' is spelled correct")
            Dim correct As Boolean = hunspell.Spell("Recommendation")
            Console.WriteLine("Recommendation is spelled " & (If(correct, "correct", "not correct")))

            Console.WriteLine("")
            Console.WriteLine("Make suggestions for the word 'Recommendatio'")
            Dim suggestions As List(Of String) = hunspell.Suggest("Recommendatio")
            Console.WriteLine("There are " & suggestions.Count.ToString() & " suggestions")
            For Each suggestion As String In suggestions
                Console.WriteLine("Suggestion is: " & suggestion)
            Next

            Console.WriteLine("")
            Console.WriteLine("Analyze the word 'decompressed'")
            Dim morphs As List(Of String) = hunspell.Analyze("decompressed")
            For Each morph As String In morphs
                Console.WriteLine("Morph is: " & morph)
            Next

            Console.WriteLine("")
            Console.WriteLine("Find the word stem of the word 'decompressed'")
            Dim stems As List(Of String) = hunspell.Stem("decompressed")
            For Each stem As String In stems
                Console.WriteLine("Word Stem is: " & stem)
            Next

            Console.WriteLine("")
            Console.WriteLine("Generate the plural of 'girl' by providing sample 'boys'")
            Dim generated As List(Of String) = hunspell.Generate("girl", "boys")
            For Each stem As String In generated
                Console.WriteLine("Generated word is: " & stem)

            Next
        End Using

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("Hyph - Hyphenation Functions")
        Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")

        ' Important: Due to the fact Hyphen will use unmanaged memory you have to serve the IDisposable pattern
        ' In this block of code this is be done by a using block. But you can also call hyphen.Dispose()
        Using hyphen As New Hyphen("hyph_en_us.dic")
            Console.WriteLine("Get the hyphenation of the word 'Recommendation'")
            Dim hyphenated As HyphenResult = hyphen.Hyphenate("Recommendation")
            Console.WriteLine("'Recommendation' is hyphenated as: " & hyphenated.HyphenatedWord)
        End Using

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("MyThes - Thesaurus/Synonym Functions")
        Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")

        Dim thes As New MyThes("th_en_us_new.dat")

        Using hunspell As New Hunspell("en_us.aff", "en_us.dic")
            Console.WriteLine("Get the synonyms of the plural word 'cars'")
            Console.WriteLine("hunspell must be used to get the word stem 'car' via Stem().")
            Console.WriteLine("hunspell generates the plural forms of the synonyms via Generate()")
            Dim tr As ThesResult = thes.Lookup("cars", hunspell)

            If tr.IsGenerated Then
                Console.WriteLine("Generated over stem (The original word form wasn't in the thesaurus)")
            End If
            For Each meaning As ThesMeaning In tr.Meanings
                Console.WriteLine()
                Console.WriteLine("  Meaning: " & meaning.Description)

                For Each synonym As String In meaning.Synonyms

                    Console.WriteLine("    Synonym: " & synonym)
                Next
            Next
        End Using

        ' Using the spell engine for server applications
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("SpellEngine - Spell Check/Hyphenation/Thesaurus Engine")
        Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")
        Console.WriteLine("High performance spell checking for servers and web servers")
        Console.WriteLine("All functions are tread safe. Implementaion uses multi core/multi processor")
        Console.WriteLine("Multiple Languages can be added via AddLanguage()")
        Using engine As New SpellEngine()

            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Adding a Language with all dictionaries for Hunspell, Hypen and MyThes")
            Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯")
            Dim enConfig As New LanguageConfig()
            enConfig.LanguageCode = "en"
            enConfig.HunspellAffFile = "en_us.aff"
            enConfig.HunspellDictFile = "en_us.dic"
            enConfig.HunspellKey = ""
            enConfig.HyphenDictFile = "hyph_en_us.dic"
            enConfig.MyThesIdxFile = "th_en_us_new.idx"
            enConfig.MyThesDatFile = "th_en_us_new.dat"
            Console.WriteLine("Configuration will use " & engine.Processors.ToString() & " processors to serve concurrent requests")
            engine.AddLanguage(enConfig)

            Console.WriteLine()
            Console.WriteLine("Check if the word 'Recommendation' is spelled correct")
            Dim correct As Boolean = engine("en").Spell("Recommendation")
            Console.WriteLine("Recommendation is spelled " & (If(correct, "correct", "not correct")))


            Console.WriteLine()
            Console.WriteLine("Make suggestions for the word 'Recommendatio'")
            Dim suggestions As List(Of String) = engine("en").Suggest("Recommendatio")
            Console.WriteLine("There are " & suggestions.Count.ToString() & " suggestions")
            For Each suggestion As String In suggestions
                Console.WriteLine("Suggestion is: " & suggestion)
            Next

            Console.WriteLine("")
            Console.WriteLine("Analyze the word 'decompressed'")
            Dim morphs As List(Of String) = engine("en").Analyze("decompressed")
            For Each morph As String In morphs
                Console.WriteLine("Morph is: " & morph)
            Next

            Console.WriteLine("")
            Console.WriteLine("Find the word stem of the word 'decompressed'")
            Dim stems As List(Of String) = engine("en").Stem("decompressed")
            For Each stem As String In stems
                Console.WriteLine("Word Stem is: " & stem)
            Next

            Console.WriteLine()
            Console.WriteLine("Generate the plural of 'girl' by providing sample 'boys'")
            Dim generated As List(Of String) = engine("en").Generate("girl", "boys")
            For Each stem As String In generated
                Console.WriteLine("Generated word is: " & stem)
            Next

            Console.WriteLine()
            Console.WriteLine("Get the hyphenation of the word 'Recommendation'")
            Dim hyphenated As HyphenResult = engine("en").Hyphenate("Recommendation")
            Console.WriteLine("'Recommendation' is hyphenated as: " & hyphenated.HyphenatedWord)


            Console.WriteLine("Get the synonyms of the plural word 'cars'")
            Console.WriteLine("hunspell must be used to get the word stem 'car' via Stem().")
            Console.WriteLine("hunspell generates the plural forms of the synonyms via Generate()")
            Dim tr As ThesResult = engine("en").LookupSynonyms("cars", True)
            If tr.IsGenerated Then
                Console.WriteLine("Generated over stem (The original word form wasn't in the thesaurus)")
            End If
            For Each meaning As ThesMeaning In tr.Meanings
                Console.WriteLine()
                Console.WriteLine("  Meaning: " & meaning.Description)

                For Each synonym As String In meaning.Synonyms

                    Console.WriteLine("    Synonym: " & synonym)
                Next

            Next
        End Using

        Console.WriteLine("")
        Console.WriteLine("Press any key to continue...")


        Console.ReadKey()

    End Sub

End Module
