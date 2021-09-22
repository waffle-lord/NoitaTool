using NoitaTool.Helpers;
using NoitaTool.Outputs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NoitaTool.Model
{
    public class Menu
    {
        public enum ConsistentResponses
        {
            Exit = ConsoleKey.E,
            MainMenu = ConsoleKey.Q,
            PlayGame = ConsoleKey.W
        }

        public int Width
        {
            get => Options.Keys.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + 7; //adding 7 for: spaces, lines, and numbers. Like:  12345      67
        }                                                                                                    //                                                "| 4] <text> |"
        public int Height
        {
            get => Options.Count + 2; //adding two to give space above and below for the box lines.
        }

        public string Title { get; set; } = String.Empty;
        public Dictionary<string, Action> Options = new Dictionary<string, Action>();

        public void ShowMenu()
        {
            OutputHelper.DrawMenu(this);
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);

            switch (pressedKey.Key)
            {
                case (ConsoleKey)Menu.ConsistentResponses.MainMenu:
                    {
                        if (!this.Equals(Menus.MainMenu()))
                        {
                            Menus.MainMenu().ShowMenu();
                        }

                        break;
                    }
                case (ConsoleKey)Menu.ConsistentResponses.PlayGame:
                    {
                        OutputHelper.DrawCenteredMessage("Starting Game. Have fun!");
                        Process.Start("cmd.exe", "/C start steam://rungameid/881100");
                        Thread.Sleep(2000);
                        break;
                    }
                case (ConsoleKey)Menu.ConsistentResponses.Exit:
                    {
                        OutputHelper.DrawCenteredMessage("Thanks for using!");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        int response;
                        if (int.TryParse(pressedKey.KeyChar.ToString(), out response)) 
                        {
                            if (Options.Count() >= response && response != 0)
                            {
                                Options.Values.ElementAt((response - 1)).Invoke();
                            }
                        }
                        break;
                    }
            }
        }
    }
}
