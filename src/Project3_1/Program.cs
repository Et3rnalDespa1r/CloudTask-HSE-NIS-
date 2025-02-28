/*
 * Загоруйко Даниил
 * Вариант 3
 * Проект3_1
 * БПИ 247-2
 */
using System;

namespace BookOfHoursApp
{
    /// <summary>
    /// Главный класс приложения, предоставляющий выбор типа пользовательского интерфейса:
    /// консольного (<see cref="BookOfHours.ConsoleUI"/>).
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Выберите интерфейс:");
            Console.WriteLine("1 – Классический консольный");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();
            if (choice.Trim() == "1")
            {
                BookOfHours.ConsoleUI ui = new BookOfHours.ConsoleUI();
                ui.Run();
            }
        }
    }
}