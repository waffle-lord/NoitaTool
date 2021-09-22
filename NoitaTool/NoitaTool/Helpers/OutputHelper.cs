using NoitaTool.Model;
using System;
using System.Collections.Generic;
using System.Text;

using CConsole = Colorful.Console;
using CColor = System.Drawing.Color;

namespace NoitaTool.Helpers
{
    /// <summary>
    /// Helps to manage the console buffer.
    /// </summary>
    public static class OutputHelper
    {
        //these aren't really useful anymore since I need to redraw the entire screen for this to work in exe form. So dumb...
        private static CursorPosition BufferStart;
        private static CursorPosition BufferStop;
        public enum BoxLinePosition
        {
            Top,
            Middle,
            Bottom
        }

        public static void ClearBuffer()
        {

            string emptyString = String.Empty;
            for (int i = 0; i < Console.BufferWidth; i++)
            {
                emptyString += " ";
            }

            for (int y = 0; y < BufferStop.Y - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.WriteLine(emptyString);
            }

            Console.SetCursorPosition(0,0);

            Init();
        }

        public static void DrawConsistentOptions()
        {
            Console.SetCursorPosition(BufferStart.X, BufferStop.Y - 3);
            DrawFullWidthSeperator('═');

            var conOptions = Enum.GetValues(typeof(Menu.ConsistentResponses));

            string optionsOutput = String.Empty;
            foreach(var value in conOptions)
            {
                string conKey = Enum.GetName(typeof(ConsoleKey), (int)Enum.Parse(typeof(Menu.ConsistentResponses), value.ToString()));

                //ConsoleKey numbers start with 'D'. So I'm just removing it so it looks more normal
                if(conKey.Length > 1 && conKey.StartsWith('D'))
                {
                    conKey = conKey.Remove(0, 1);
                }

                optionsOutput += $"{conKey}: {value}   ";
            }

            int Xoffset = (Console.BufferWidth / 2) - (optionsOutput.Length / 2);

            Console.SetCursorPosition(Xoffset, Console.CursorTop);
            Console.WriteLine(optionsOutput);
            Console.SetCursorPosition(BufferStart.X, BufferStart.Y);
        }

        public static void DrawMenu(Menu menu)
        {   
            ClearBuffer();

            int menuWidth = menu.Width > menu.Title.Length + 2 ? menu.Width : menu.Title.Length + 2;

            int Xoffset = (Console.BufferWidth / 2) - (menuWidth / 2);
            int Yoffset = (Console.BufferHeight / 4 * 3) - (menu.Height / 2);

            List<string> options = new List<string>(menu.Options.Keys);

            Console.SetCursorPosition(Xoffset, Yoffset);

            DrawBoxLine(menuWidth, BoxLinePosition.Top);
            Yoffset++;


            if(!String.IsNullOrEmpty(menu.Title))
            {
                int titleOffset = ((menuWidth-2) / 2) - (menu.Title.Length / 2);
                string titleLine = "║";

                for(int x = 0; x < titleOffset; x++)
                {
                    titleLine += " ";
                }

                titleLine += menu.Title;

                for (;titleLine.Length < menuWidth-1;)
                {
                    titleLine += " ";
                }

                titleLine += "║";

                Console.SetCursorPosition(Xoffset, Yoffset);
                Console.WriteLine(titleLine);
                Yoffset++;

                Console.SetCursorPosition(Xoffset, Yoffset);
                DrawBoxLine(menuWidth, BoxLinePosition.Middle);
                Yoffset++;

            }



            for(int x = 0; x < menu.Options.Count; x++)
            {
                Console.SetCursorPosition(Xoffset, Yoffset);
                string menuLine = $"║ {x+1}] {options[x]}";
                int lineOffset = menuWidth - menuLine.Length - 1;

                for(int i = 0; i < lineOffset; i++)
                {
                    menuLine += " ";
                }

                menuLine += "║";

                Console.WriteLine(menuLine);
                Yoffset++;
            }

            Console.SetCursorPosition(Xoffset, Yoffset);
            DrawBoxLine(menuWidth, BoxLinePosition.Bottom);
            DrawConsistentOptions();
        }

        public static void DrawBoxLine(int MenuWidth, BoxLinePosition position)
        {
            StringBuilder sb = new StringBuilder();

            for(; MenuWidth != 0; MenuWidth--)
            {
                sb.Append('═');
            }

            switch (position)
            {
                case BoxLinePosition.Top:
                    {
                        sb.Remove(0, 1);
                        sb.Insert(0, '╔');
                        sb.Remove(sb.Length-1, 1);
                        sb.Insert(sb.Length, '╗');
                        break;
                    }
                case BoxLinePosition.Middle:
                    {
                        sb.Remove(0, 1);
                        sb.Insert(0, '╠');
                        sb.Remove(sb.Length-1, 1);
                        sb.Insert(sb.Length, '╣');
                        break;
                    }
                case BoxLinePosition.Bottom:
                    {
                        sb.Remove(0, 1);
                        sb.Insert(0, '╚');
                        sb.Remove(sb.Length-1, 1);
                        sb.Insert(sb.Length, '╝');
                        break;
                    }
            }

            Console.WriteLine(sb.ToString());
        }

        public static void DrawFullWidthSeperator(char character)
        {
            string seperator = String.Empty;

            Console.SetCursorPosition(1, Console.CursorTop);

            for(int i = 0; i < Console.BufferWidth-1; i++)
            {
                seperator += character;
            }

            Console.WriteLine(seperator);
        }

        public static bool GetCenteredConfirmation(string Message)
        {
            OutputHelper.DrawCenteredMessage($"{Message} (y/[n])");
            char key = Console.ReadKey(true).KeyChar;

            return key == 'y';
        }

        public static void DrawCenteredMessage(string message)
        {
            ClearBuffer();
            int Xoffset = (Console.BufferWidth / 2) - (message.Length / 2);
            int Yoffset = Console.BufferHeight / 4 * 3;
            Console.SetCursorPosition(Xoffset, Yoffset);

            Console.WriteLine(message);
        }

        private static void PrintLogo()
        {
            CConsole.WriteLine();
            CConsole.WriteLine();
            CConsole.WriteLine();
            string[] logoLines = new string[] {
            "      ░▓     ▓▓░                                                               ",
            "     ▓▓▓▓▓▓  ▓▓▓▓▓▓                                                            ",
            "   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                         ▓▓▓▓       ▓▓▓                   ",
            "   ▓▓▓▓▓▓▓▓▓▓  ▓▓▓▓▓▓▓▓                      ▓▓▓▓▓     ▓▓▓▓▓                   ",
            "   ▓▓▓▓▓▓▓▓      ▓▓▓▓▓▓▓▓                      ▓▓▓▓   ▓▓▓▓▓▓                   ",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓▓                    ▓     ▓▓▓▓▓▓                   ",
            "   ▓▓▓▓▓▓▓▓         ▓▓▓▓▓▓▓░                    ░ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒            ",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓          ▓         ▓  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓       ▓      ",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓      ▓▓▓▓▓▓      ▓▓▓▓     ▓▓▓▓▓▓         ▓▓▓▓▓▓▓   ",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓   ▓▓▓▓▓▓       ▓▓▓▓▓▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓  ▓▓▓▓▓▓   ▓▓▓▓▓▓     ▓▓▓▓  ▓▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓▓   ▓▓▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓    ▓▓▓▓▓▓ ▓▓▓▓▓▓   ▓▓▓▓▓▓   ▓▓▓▓     ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓    ▓▓▓▓▓▓ ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓     ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓    ▓▓▓▓▓▓ ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓     ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓     ▓▓▓▓▓ ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓▓    ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓     ▓▓▓▓  ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓  ▓▓▓▓▓  ▓▓▓▓▓▓   ▓▓▓▓▓▓   ▓▓▓▓▓▓  ▓▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓ ░▓▓▓▓▓▓▓▓▓▓▓▒    ▓▓▓▓▓▓   ▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓        ▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓     ▓▓▓▓▓▓▓  ▓▓▓▓▓▓▓      ▓▓▓▓▓ ▓▓▓▓▓▓▓",
            "   ▓▓▓▓▓▓▓▓▓▓      ▓▓▓▓▓▓▓▓        ▓▓       ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓      ▓     ▓▓▓▓▓▓",
            "                 ▓▓▓▓▓▓                                                        " };
            

            int r = 255;
            int g = 200;
            int b = 0;
            int wait = 0;
            foreach(string s in logoLines)
            {
                CConsole.WriteLine(s, CColor.FromArgb(r,g,b));

                wait++;
                if(wait >= 10)
                {
                    g -= 12;
                }
            }

            
            CConsole.WriteLine("  ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
        }

        public static void Init()
        {
            PrintLogo();
            BufferStart = new CursorPosition(Console.CursorLeft, Console.CursorTop);
            BufferStop = new CursorPosition(Console.BufferWidth, Console.BufferHeight);
        }
    }
}
