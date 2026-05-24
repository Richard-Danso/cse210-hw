
using System;
using System.Collections.Generic;
using System.Linq; // Crucial for .Where() and .All()
using System.Collections.Generic; // Crucial for List<>
namespace ScriptureMemorizer
{
    public class Word
    {
        private string _text;
        private bool _isHidden;

        public Word(string text)
        {
            _text = text;
            _isHidden = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public void Show()
        {
            _isHidden = false;
        }

        public bool IsHidden()
        {
            return _isHidden;
        }

        public string GetDisplayText()
        {
            if (_isHidden)
            {
                return new string('_', _text.Length);
            }
            return _text;
        }
    }
}


namespace ScriptureMemorizer
{
    public class Reference
    {
        private string _book;
        private int _chapter;
        private int _verse;
        private int _endVerse;

        public Reference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _verse = verse;
            _endVerse = verse;
        }

        public Reference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _verse = startVerse;
            _endVerse = endVerse;
        }

        public string GetDisplayText()
        {
            if (_verse == _endVerse)
            {
                return $"{_book} {_chapter}:{_verse}";
            }
            return $"{_book} {_chapter}:{_verse}-{_endVerse}";
        }
    }
}



namespace ScriptureMemorizer
{
    public class Scripture
    {
        private Reference _reference;
        private List<Word> _words;

        public Scripture(Reference reference, string text)
        {
            _reference = reference;
            _words = new List<Word>();

            string[] splitWords = text.Split(' ');
            foreach (string wordText in splitWords)
            {
                _words.Add(new Word(wordText));
            }
        }

        public void HideRandomWords(int numberToHide)
        {
            Random random = new Random();
            List<Word> visibleWords = _words.Where(w => !w.IsHidden()).ToList();
            int actualToHide = Math.Min(numberToHide, visibleWords.Count);

            for (int i = 0; i < actualToHide; i++)
            {
                int index = random.Next(visibleWords.Count);
                visibleWords[index].Hide();
                visibleWords.RemoveAt(index);
            }
        }

        public string GetDisplayText()
        {
            List<string> displayedWords = new List<string>();
            foreach (Word word in _words)
            {
                displayedWords.Add(word.GetDisplayText());
            }

            string combinedText = string.Join(" ", displayedWords);
            return $"{_reference.GetDisplayText()} - {combinedText}";
        }

        public bool IsCompletelyHidden()
        {
            return _words.All(w => w.IsHidden());
        }
    }
}



namespace ScriptureMemorizer
{
    class Program
    {
        /*
         * CREATIVITY AND EXCEEDING REQUIREMENTS:
         * 1. Implemented a Scripture Library: Instead of hardcoding a single verse, the program initialized a 
         * list of multiple scriptures (handling both single-verse and multi-verse scenarios) and selects 
         * one at random each time the program runs.
         * 2. Enhanced Random Selection (Stretch Goal): The program actively filters out already-hidden words, 
         * ensuring that it only targets remaining visible text until completely blank.
         */
        static void Main(string[] args)
        {
            List<Scripture> scriptureLibrary = new List<Scripture>
            {
                new Scripture(new Reference("John", 3, 16), "For God so loved the world that he gave his only Son"),
                new Scripture(new Reference("Proverbs", 3, 5, 6), "Trust in the Lord with all your heart and lean not on your own understanding in all your ways acknowledge him and he will make your paths straight"),
                new Scripture(new Reference("Philippians", 4, 13), "I can do all things through Christ who strengthens me"),
                new Scripture(new Reference("Joshua", 1, 9), "Be strong and courageous Do not be afraid do not be discouraged for the Lord your God will be with you wherever you go")
            };

            Random random = new Random();
            Scripture currentScripture = scriptureLibrary[random.Next(scriptureLibrary.Count)];

            while (true)
            {
       
                Console.WriteLine(currentScripture.GetDisplayText());
                Console.WriteLine();

                if (currentScripture.IsCompletelyHidden())
                {
                    Console.WriteLine("Great job! You've hidden the whole passage.");
                    break;
                }

                Console.WriteLine("Press Enter to hide more words, or type 'quit' to exit:");
                string input = Console.ReadLine();

                if (input?.ToLower() == "quit")
                {
                    break;
                }

                currentScripture.HideRandomWords(3);
            }
        }
    }
}
