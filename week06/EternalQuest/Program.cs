using System;
using System.Collections.Generic;
using System.IO;
/*
===========================================================================================
EXCEEDING REQUIREMENTS DESCRIPTION:
1. Level-Up System: Added a dynamic leveling engine in GoalManager. Users gain levels 
   for every 1000 points accrued, triggering a custom reward message and rank title.
2. Negative Goals (Bad Habits): Created a `NegativeGoal` subclass that allows users 
   to track bad habits. When recorded, it deducts points from their score, penalizing 
   setbacks while preserving full polymorphic behavior with the base class.
===========================================================================================
*/

class Program
{
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();
        manager.Start();
    }
}

public abstract class Goal
{
    // Encapsulation: protected variables allow derived classes access while hiding them from Main
    protected string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    public string GetShortName() => _shortName;

    // Polymorphism: Abstract methods that every unique goal type MUST implement differently
    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public abstract string GetDetailsString();
    public abstract string GetStringRepresentation();
}
public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points) : base(name, description, points)
    {
        _isComplete = false;
    }

    // Overloaded constructor specifically used when loading from files
    public SimpleGoal(string name, string description, int points, bool isComplete) : base(name, description, points)
    {
        _isComplete = isComplete;
    }

    public override int RecordEvent()
    {
        _isComplete = true;
        return _points;
    }

    public override bool IsComplete() => _isComplete;

    public override string GetDetailsString()
    {
        string checkbox = _isComplete ? "[X]" : "[ ]";
        return $"{checkbox} {_shortName} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal:{_shortName},{_description},{_points},{_isComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points)
    {
    }

    public override int RecordEvent()
    {
        // Eternal goals are never finished, they just award points indefinitely
        return _points;
    }

    public override bool IsComplete() => false;

    public override string GetDetailsString()
    {
        return $"[ ] {_shortName} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal:{_shortName},{_description},{_points}";
    }
}

public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int target, int bonus) : base(name, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    public ChecklistGoal(string name, string description, int points, int amountCompleted, int target, int bonus) : base(name, description, points)
    {
        _amountCompleted = amountCompleted;
        _target = target;
        _bonus = bonus;
    }

    public override int RecordEvent()
    {
        _amountCompleted++;
        if (_amountCompleted == _target)
        {
            return _points + _bonus; // Give base points + completion bonus
        }
        return _points;
    }

    public override bool IsComplete() => _amountCompleted >= _target;

    public override string GetDetailsString()
    {
        string checkbox = IsComplete() ? "[X]" : "[ ]";
        return $"{checkbox} {_shortName} ({_description}) -- Currently completed: {_amountCompleted}/{_target}";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal:{_shortName},{_description},{_points},{_amountCompleted},{_target},{_bonus}";
    }
}

public class NegativeGoal : Goal
{
    // Used to track breaking bad habits. Subtracts points instead of adding.
    public NegativeGoal(string name, string description, int points) : base(name, description, points)
    {
    }

    public override int RecordEvent()
    {
        // Notice the negative return value
        return -_points;
    }

    public override bool IsComplete() => false;

    public override string GetDetailsString()
    {
        return $"[!] {_shortName} ({_description}) [Bad Habit]";
    }

    public override string GetStringRepresentation()
    {
        return $"NegativeGoal:{_shortName},{_description},{_points}";
    }
}

public class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score;
    private int _level;

    public GoalManager()
    {
        _score = 0;
        _level = 1;
    }

    public void Start()
    {
        bool running = true;
        while (running)
        {
            UpdateLevel();
            Console.WriteLine();
            Console.WriteLine($"--- Current Score: {_score} | Level: {_level} ({GetRankTitle()}) ---");
            Console.WriteLine();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Create New Goal");
            Console.WriteLine("  2. List Goals");
            Console.WriteLine("  3. Save Goals");
            Console.WriteLine("  4. Load Goals");
            Console.WriteLine("  5. Record Event");
            Console.WriteLine("  6. Quit");
            Console.Write("Select a choice from the menu: ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CreateGoal(); break;
                case "2": ListGoalDetails(); break;
                case "3": SaveGoals(); break;
                case "4": LoadGoals(); break;
                case "5": RecordEvent(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid entry. Please choose 1-6."); break;
            }
        }
    }

    private void UpdateLevel()
    {
        // Level up algorithm: 1 level per 1000 points accrued. Minimum Level 1.
        int calculatedLevel = (_score / 1000) + 1;
        if (calculatedLevel < 1) calculatedLevel = 1; 

        if (calculatedLevel > _level)
        {
            _level = calculatedLevel;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🌟🌟🌟 LEVEL UP! You are now Level {_level} ({GetRankTitle()})! 🌟🌟🌟\n");
            Console.ResetColor();
        }
        else
        {
            _level = calculatedLevel;
        }
    }

    private string GetRankTitle()
    {
        if (_level == 1) return "Novice";
        if (_level == 2) return "Apprentice Scout";
        if (_level == 3) return "Questing Knight";
        return "Ninja Unicorn";
    }

    public void ListGoalDetails()
    {
        Console.WriteLine("\nThe goals are:");
        if (_goals.Count == 0) Console.WriteLine("(No goals currently loaded or created)");
        
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("\nThe types of Goals are:");
        Console.WriteLine("  1. Simple Goal");
        Console.WriteLine("  2. Eternal Goal");
        Console.WriteLine("  3. Checklist Goal");
        Console.WriteLine("  4. Negative Goal (Bad Habit)");
        Console.Write("Which type of goal would you like to create? ");
        string type = Console.ReadLine();

        Console.Write("What is the name of your goal? ");
        string name = Console.ReadLine();
        Console.Write("What is a short description of it? ");
        string description = Console.ReadLine();
        Console.Write("What is the amount of points associated with this goal? ");
        int points = int.Parse(Console.ReadLine());

        switch (type)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, description, points));
                break;
            case "2":
                _goals.Add(new EternalGoal(name, description, points));
                break;
            case "3":
                Console.Write("How many times does this goal need to be accomplished for a bonus? ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("What is the bonus for accomplishing it that many times? ");
                int bonus = int.Parse(Console.ReadLine());
                _goals.Add(new ChecklistGoal(name, description, points, target, bonus));
                break;
            case "4":
                _goals.Add(new NegativeGoal(name, description, points));
                break;
            default:
                Console.WriteLine("Invalid choice. Goal creation aborted.");
                break;
        }
    }

    public void RecordEvent()
    {
        ListGoalDetails();
        if (_goals.Count == 0) return;

        Console.Write("\nWhich goal did you accomplish? ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < _goals.Count)
        {
            Goal selectedGoal = _goals[index];
            
            if (selectedGoal.IsComplete())
            {
                Console.WriteLine("This goal is already marked complete!");
                return;
            }

            int pointsEarned = selectedGoal.RecordEvent();
            _score += pointsEarned;

            if (pointsEarned > 0)
            {
                Console.WriteLine($"Congratulations! You have earned {pointsEarned} points!");
            }
            else if (pointsEarned < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ouch! Point penalty! You lost {Math.Abs(pointsEarned)} points for slipping up.");
                Console.ResetColor();
            }
            Console.WriteLine($"You now have {_score} points.");
        }
        else
        {
            Console.WriteLine("Invalid goal selection index.");
        }
    }

    public void SaveGoals()
    {
        Console.Write("What is the filename for the goal file? ");
        string filename = Console.ReadLine();

        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            // First entry line is always the saved score/state
            outputFile.WriteLine(_score);
            foreach (Goal goal in _goals)
            {
                outputFile.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine("Goals successfully saved.");
    }

    public void LoadGoals()
    {
        Console.Write("What is the filename for the goal file? ");
        string filename = Console.ReadLine();

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        _goals.Clear(); // Empty existing entries before loading new file
        string[] lines = File.ReadAllLines(filename);
        
        // Grab score from the first row
        _score = int.Parse(lines[0]);

        // Implement the Factory Pattern layout to construct subclasses poly-morphically
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] mainParts = line.Split(":");
            
            string type = mainParts[0];
            string[] dataFields = mainParts[1].Split(",");

            if (type == "SimpleGoal")
            {
                _goals.Add(new SimpleGoal(dataFields[0], dataFields[1], int.Parse(dataFields[2]), bool.Parse(dataFields[3])));
            }
            else if (type == "EternalGoal")
            {
                _goals.Add(new EternalGoal(dataFields[0], dataFields[1], int.Parse(dataFields[2])));
            }
            else if (type == "ChecklistGoal")
            {
                _goals.Add(new ChecklistGoal(dataFields[0], dataFields[1], int.Parse(dataFields[2]), int.Parse(dataFields[3]), int.Parse(dataFields[4]), int.Parse(dataFields[5])));
            }
            else if (type == "NegativeGoal")
            {
                _goals.Add(new NegativeGoal(dataFields[0], dataFields[1], int.Parse(dataFields[2])));
            }
        }
        Console.WriteLine("Goals successfully loaded.");
    }
}