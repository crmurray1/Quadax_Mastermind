using Quadax_Mastermind.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Quadax_Mastermind
{
    public sealed class MasterMindGame
    {
        public List<Guess> Answer { get; set; }

        public void Run()
        {
            bool keepPlaying = false;

            try
            {
                do
                {
                    keepPlaying = false;

                    GreetPlayer();

                    GenerateAnswer();

                    bool winner = Play();

                    string response = "";

                    response = winner ? "Congratulations! You are a Master Mind!" : "Good Try!";                    
                    response += "   Would you like to play again? [Y]";

                    Console.WriteLine(response);

                    var input = Console.ReadLine();

                    if (input.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) keepPlaying = true;
                    else Environment.Exit(0);
                }
                while (keepPlaying);
            }
            catch (Exception e)
            {
                //Something went wrong. Log?
                Environment.Exit(0);
            }
            
        }

        public void GreetPlayer()
        {
            Console.WriteLine("We are going to play Mastermind. YOu will have 10 chances to guess all 4 numbers, varying from 1-6, in the correct order.");
            Console.WriteLine("If you do not guess the sequence correctly. Don't worry, we can play again!");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press any key when you are ready to play...");
            Console.ReadKey(true);
        }

        public void GenerateAnswer()
        {
            Answer = new List<Guess>();
            Random random = new Random();

            Answer.Add(new Guess(random.Next(1, 6)));
            Answer.Add(new Guess(random.Next(1, 6)));
            Answer.Add(new Guess(random.Next(1, 6)));
            Answer.Add(new Guess(random.Next(1, 6)));
        }

        public string ValidateInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Please submit a guess.";
            if (input.Length != 4) return "Input must be only 4 numbers (no spaces please).";
            if (!input.All(char.IsAsciiDigit)) return "Input must be numbers only please.";

            return string.Empty;
        }

        public bool Play()
        {
            for (int i = 0; i < 10; i++)
            {

                Console.WriteLine($"Guess #{i + 1}");
                string? input = Console.ReadLine();

                string validation = ValidateInput(input);

                bool isValid = string.IsNullOrEmpty(validation);

                if (isValid)
                {
                    List<Guess> playerGuess = new List<Guess>();

                    foreach (var g in input.ToCharArray())
                    {
                        playerGuess.Add(new Guess(int.Parse(g.ToString())));
                    }

                    for (int j = 0; j < Answer.Count; j++)
                    {
                        playerGuess[j].IsCorrect = Answer[j].Value == playerGuess[j].Value;

                        if (!playerGuess[j].IsCorrect) playerGuess[j].AlmostCorrect = Answer.Select(x => x.Value).ToList().Contains(playerGuess[j].Value);
                    }

                    //WINNER!!!
                    if (playerGuess.Select(x => x.IsCorrect).All(b => b)) return true;

                    string correct = string.Empty;
                    string almostCorrect = string.Empty;

                    foreach (var guess in playerGuess)
                    {
                        if (guess.IsCorrect) correct += "+";
                        if (guess.AlmostCorrect) almostCorrect += "-";
                    }

                    Console.WriteLine($"{correct} {almostCorrect}");
                }
                else
                {
                    Console.WriteLine(validation);
                    //replay this guess
                    i = i - 1;
                }
            }

            return false;
        }

    }
}
