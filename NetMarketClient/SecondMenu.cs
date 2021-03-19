using System;
using System.Collections.Generic;

namespace NetMarketClient
{
    public static class SecondMenu
    {
        private static int _iterator = 0;
        private static bool _exit = false;
        private static List<string> _list;

        public static void Show(int firstIndex)
        {
            _list = NetWork.PostFirstMenu(firstIndex);

            Print(_list);

            do
            {
                if (Console.KeyAvailable)
                {
                    Control(_list.Count, firstIndex);

                    Print(_list);
                }
            } while (!_exit);

            _exit = false;
        }

        private static void Control(int limit, int firstIndex)
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (_iterator > 0) _iterator--;
                    break;

                case ConsoleKey.DownArrow:
                    if (_iterator < limit - 1) _iterator++;
                    break;

                case ConsoleKey.Add:
                    _list = NetWork.PUTSecondMenu(firstIndex);
                    break;

                case ConsoleKey.Spacebar:
                    _list = NetWork.EditSecondMenu(firstIndex, _iterator);
                    break;

                case ConsoleKey.Delete:
                    _list = NetWork.DelSecondMenu(firstIndex, _iterator);                    
                    _iterator = 0;
                    break;

                case ConsoleKey.Escape:
                    _exit = true;
                    break;

                default:
                    break;
            }
        }

        private static void Print(List<string> list)
        {
            Console.Clear();

            Console.WriteLine("Id\t|Name\t\t\t|Volume\t\t|Price\t\t|");

            Console.WriteLine("------------------------------------------------------------------------");

            int count = 0;

            foreach (var item in list)
            {
                Console.ForegroundColor = count == _iterator ? ConsoleColor.Red : ConsoleColor.White;

                string[] words = item.Split(new char[] { ' ' });

                int j = 0;

                foreach (var i in words)
                {
                    Console.CursorLeft = j * ((j == 1) ? 8 : 16);

                    Console.Write(i);

                    j++;
                }

                Console.WriteLine();

                count++;
            }

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, 25);

            Console.WriteLine("#################################" +
                "##################################################" +
                "#####################################");

            Console.WriteLine("Escape - выход, " +
                "    Enter - вход,      ↑↓ - навигация, " +
                "    Delete - удалить,    + - добавить,  " +
                "  Space - редактировать");

            Console.CursorVisible = false;
        }
    }
}