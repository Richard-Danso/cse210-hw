using System;
using System.Collections.Generic; // Added for List<T>
using System.IO; // Added for file handling

namespace JournalProgram
{
    public class Entry
    {
        public string _date { get; set; }
        public string _promptText { get; set; }
        public string _entryText { get; set; }
        public string _mood { get; set; } // Creativity Feature: Holds the user's mood rating

        // Updated constructor to include mood tracking
        public Entry(string date, string promptText, string entryText, string mood)
        {
            _date = date;
            _promptText = promptText;
            _entryText = entryText;
            _mood = mood;
        }

        public void Display()
        {
            Console.WriteLine($"Date: {_date} - Prompt: {_promptText}");
            Console.WriteLine($"Mood Rating: {_mood}/5");
            Console.WriteLine($"Response: {_entryText}");
            Console.WriteLine();
        }

        public string ExportToLine()
        {
            // Includes the mood metric in the saved data line
            return $"{_date}~|~{_promptText}~|~{_entryText}~|~{_mood}";
        }
    }
}

namespace JournalProgram
{
    public class PromptGenerator
    {
        public List<string> _prompts;

        public PromptGenerator()
        {
            _prompts = new List<string>
            {
                "Who was the most interesting person I interacted with today?",
                "What was the best part of my day?",
                "How did I see the hand of the Lord in my life today?",
                "What was the strongest emotion I felt today?",
                "If I had one thing I could do over today, what would it be?",
                "What was a small success you had today?",
                "What did you do today that brought you closer to your goals?"
            };
        }

        public string GetRandomPrompt()
        {
            Random random = new Random();
            int index = random.Next(_prompts.Count);
            return _prompts[index];
        }
    }
}

namespace JournalProgram
{
    public class Journal
    {
        public List<Entry> _entries;

        public Journal()
        {
            _entries = new List<Entry>();
        }

        public void AddEntry(Entry newEntry)
        {
            _entries.Add(newEntry);
        }

        public void DisplayAll()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("The journal is currently empty.");
                return;
            }

            foreach (Entry entry in _entries)
            {
                entry.Display();
            }
        }

        public void SaveToFile(string file)
        {
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                foreach (Entry entry in _entries)
                {
                    outputFile.WriteLine(entry.ExportToLine());
                }
            }
            Console.WriteLine("Journal saved successfully!");
        }

        public void LoadFromFile(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("Error: That file does not exist.");
                return;
            }

            _entries.Clear();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                string[] parts = line.Split(new string[] { "~|~" }, StringSplitOptions.None);
                
                // Expecting 4 parts now including mood
                if (parts.Length == 4)
                {
                    string date = parts[0];
                    string prompt = parts[1];
                    string entryText = parts[2];
                    string mood = parts[3];

                    Entry loadedEntry = new Entry(date, prompt, entryText, mood);
                    _entries.Add(loadedEntry);
                }
            }
            Console.WriteLine("Journal loaded successfully!");
        }
    }
}


namespace JournalProgram
{
    class Program
    {
        /* 
           CREATIVITY AND EXCEEDING REQUIREMENTS:
           - Added a custom Mood Tracker subsystem.
           - When writing an entry, the user is prompted to rate their daily mood on a scale from 1 to 5.
           - The mood value is fully encapsulated into the Entry object and saved/loaded via the file system.
           - Displaying entries prints out the visual mood score alongside the text.
        */

        static void Main(string[] args)
        {
            Journal myJournal = new Journal();
            PromptGenerator promptGenerator = new PromptGenerator();
            string userChoice = "";

            Console.WriteLine("Welcome to the Journal Program!");

            while (userChoice != "5")
            {
                Console.WriteLine("Please select one of the following choices:");
                Console.WriteLine("1. Write");
                Console.WriteLine("2. Display");
                Console.WriteLine("3. Load");
                Console.WriteLine("4. Save");
                Console.WriteLine("5. Quit");
                Console.Write("What would you like to do? ");
                
                userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    string randomPrompt = promptGenerator.GetRandomPrompt();
                    Console.WriteLine($"\nPrompt: {randomPrompt}");
                    Console.Write("> ");
                    string response = Console.ReadLine();

                    // Prompt user for the exceeding requirement metric
                    Console.Write("Rate your overall mood today (1-5): ");
                    string moodRating = Console.ReadLine();

                    string currentDate = DateTime.Now.ToShortDateString();
                    
                    Entry newEntry = new Entry(currentDate, randomPrompt, response, moodRating);
                    myJournal.AddEntry(newEntry);
                    Console.WriteLine();
                }
                else if (userChoice == "2")
                {
                    Console.WriteLine("\n--- Journal Entries ---");
                    myJournal.DisplayAll();
                }
                else if (userChoice == "3")
                {
                    Console.Write("What is the filename? ");
                    string loadFilename = Console.ReadLine();
                    myJournal.LoadFromFile(loadFilename);
                    Console.WriteLine();
                }
                else if (userChoice == "4")
                {
                    Console.Write("What is the filename? ");
                    string saveFilename = Console.ReadLine();
                    myJournal.SaveToFile(saveFilename);
                    Console.WriteLine();
                }
                else if (userChoice == "5")
                {
                    Console.WriteLine("Thank you for using the Journal Program. Goodbye!");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.\n");
                }
            }
        }
    }
}
  