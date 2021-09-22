using NoitaTool.Helpers;
using NoitaTool.Model;
using NoitaTool.Outputs;
using System;

namespace NoitaTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 50);
            Console.SetBufferSize(80, 50);
            Console.Title = "Noita Tool";

            OutputHelper.Init();

            do
            {
                Menus.MainMenu().ShowMenu();
            }
            while (true);
        }
    }
}
