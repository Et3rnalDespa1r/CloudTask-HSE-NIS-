using System;

namespace BookOfHours
{
    /// <summary>
    /// Классический консольный пользовательский интерфейс для взаимодействия с пользователем.
    /// Предоставляет меню с действиями: загрузка, редактирование, удаление, слияние файлов, сохранение и т. д.
    /// </summary>
    public class ConsoleUI
    {
        private readonly DataManager dataManager = new DataManager();

        /// <summary>
        /// Запускает цикл обработки пользовательских команд в консоли.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nBook of Hours – Меню");
                Console.WriteLine("1. Загрузить данные из файла");
                Console.WriteLine("2. Редактировать аспект");
                Console.WriteLine("3. Удалить аспект");
                Console.WriteLine("4. Слить данные из двух файлов");
                Console.WriteLine("5. Сохранить данные в файл");
                Console.WriteLine("6. Вывести список аспектов");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1":
                            Console.Write("Введите путь к файлу для загрузки: ");
                            string loadPath = Console.ReadLine();
                            Console.Write("Файл локализованный? (y/n): ");
                            bool isLocalized = Console.ReadLine().Trim().ToLower() == "y";
                            dataManager.LoadFromFile(loadPath, isLocalized);
                            Console.WriteLine("Данные успешно загружены.");
                            break;

                        case "2":
                            Console.Write("Введите id аспекта для редактирования: ");
                            string editId = Console.ReadLine();
                            dataManager.EditAspect(editId, aspect =>
                            {
                                Console.Write("Введите новое название: ");
                                aspect.Label = Console.ReadLine();
                                Console.Write("Введите новое описание: ");
                                aspect.Desc = Console.ReadLine();
                            });
                            Console.WriteLine("Аспект изменён.");
                            break;

                        case "3":
                            Console.Write("Введите id аспекта для удаления: ");
                            string delId = Console.ReadLine();
                            if (dataManager.DeleteAspect(delId))
                                Console.WriteLine("Аспект удалён.");
                            else
                                Console.WriteLine("Аспект не найден.");
                            break;

                        case "4":
                            Console.Write("Введите путь к первому файлу: ");
                            string file1 = Console.ReadLine();
                            Console.Write("Введите путь ко второму файлу: ");
                            string file2 = Console.ReadLine();
                            dataManager.MergeFiles(file1, file2);
                            Console.WriteLine("Файлы слиты.");
                            break;

                        case "5":
                            Console.Write("Введите путь для сохранения данных: ");
                            string savePath = Console.ReadLine();
                            dataManager.SaveToFile(savePath);
                            Console.WriteLine("Данные сохранены.");
                            break;

                        case "6":
                            Console.WriteLine("Список аспектов:");
                            foreach (var aspect in dataManager.Aspects)
                                Console.WriteLine(aspect.ToString());
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
