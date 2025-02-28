using System;
using System.Collections.Generic;

namespace BookOfHours
{
    /// <summary>
    /// Класс для управления коллекцией аспектов (CRUD-операции, загрузка/сохранение и т. д.).
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Список всех загруженных аспектов.
        /// </summary>
        public List<Aspect> Aspects { get; private set; } = new List<Aspect>();

        /// <summary>
        /// Возвращает аспект по его идентификатору <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Идентификатор искомого аспекта.</param>
        /// <returns>Найденный объект типа <see cref="Aspect"/> или <c>null</c>, если аспект не найден.</returns>
        public Aspect GetAspectById(string id)
        {
            return Aspects.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Загружает аспекты из JSON-файла и добавляет (или обновляет) их в коллекции.
        /// При <paramref name="isLocalized"/> = <c>false</c> объекты из файла полностью заменяют
        /// объекты в списке <see cref="Aspects"/> (если ID совпадает).
        /// При <paramref name="isLocalized"/> = <c>true</c> обновляются только отдельные поля,
        /// остальные свойства остаются прежними.
        /// </summary>
        /// <param name="filePath">Путь к JSON-файлу.</param>
        /// <param name="isLocalized">Флаг, определяющий режим загрузки:
        /// <c>false</c> – полноценная замена,
        /// <c>true</c> – локализованное обновление полей.</param>
        /// <exception cref="Exception">Выбрасывается при ошибках парсинга JSON.</exception>
        public void LoadFromFile(string filePath, bool isLocalized = false)
        {
            var loaded = JsonParser.ReadJson(filePath);
            foreach (var aspect in loaded)
            {
                var existing = Aspects.Find(a => a.Id == aspect.Id);
                if (existing != null)
                {
                    if (isLocalized)
                    {
                        // Обновляем только те поля, которые заданы (не пусты) в локализованном файле.
                        if (!string.IsNullOrEmpty(aspect.Label)) existing.Label = aspect.Label;
                        if (!string.IsNullOrEmpty(aspect.Desc)) existing.Desc = aspect.Desc;
                        if (!string.IsNullOrEmpty(aspect.Icon)) existing.Icon = aspect.Icon;
                        if (!string.IsNullOrEmpty(aspect.Sort)) existing.Sort = aspect.Sort;
                        if (!string.IsNullOrEmpty(aspect.Comments)) existing.Comments = aspect.Comments;
                        // Остальные поля остаются из core-файла.
                    }
                    else
                    {
                        // Полная замена объекта целиком.
                        int index = Aspects.IndexOf(existing);
                        Aspects[index] = aspect;
                    }
                }
                else
                {
                    Aspects.Add(aspect);
                }
            }
        }

        /// <summary>
        /// Сохраняет текущую коллекцию аспектов в указанный JSON-файл.
        /// </summary>
        /// <param name="filePath">Путь к JSON-файлу для сохранения.</param>
        /// <exception cref="IOException">Может возникнуть при ошибках записи в файл.</exception>
        public void SaveToFile(string filePath)
        {
            string json = JsonParser.WriteJson(Aspects);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Производит редактирование аспекта с указанным <paramref name="id"/>.
        /// Если аспект не найден, создаёт новый с данным идентификатором и добавляет в коллекцию.
        /// Затем вызывает переданный <paramref name="editAction"/> для изменения полей аспекта.
        /// </summary>
        /// <param name="id">Идентификатор аспекта.</param>
        /// <param name="editAction">Делегат, описывающий действия по редактированию полей аспекта.</param>
        public void EditAspect(string id, Action<Aspect> editAction)
        {
            var aspect = Aspects.Find(a => a.Id == id);
            if (aspect == null)
            {
                aspect = new Aspect { Id = id };
                Aspects.Add(aspect);
            }
            editAction(aspect);
        }

        /// <summary>
        /// Удаляет аспект с указанным <paramref name="id"/> из коллекции <see cref="Aspects"/>.
        /// </summary>
        /// <param name="id">Идентификатор аспекта.</param>
        /// <returns><c>true</c> если аспект был найден и удалён, иначе <c>false</c>.</returns>
        public bool DeleteAspect(string id)
        {
            var aspect = Aspects.Find(a => a.Id == id);
            if (aspect != null)
            {
                Aspects.Remove(aspect);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Сливает данные из двух JSON-файлов, решая конфликты путём выбора одной из двух версий пользователем.
        /// После слияния результат помещается в текущий список <see cref="Aspects"/>.
        /// </summary>
        /// <param name="file1">Путь к первому JSON-файлу.</param>
        /// <param name="file2">Путь ко второму JSON-файлу.</param>
        /// <exception cref="Exception">При ошибках чтения или парсинга JSON.</exception>
        public void MergeFiles(string file1, string file2)
        {
            var aspects1 = JsonParser.ReadJson(file1);
            var aspects2 = JsonParser.ReadJson(file2);
            Dictionary<string, Aspect> merged = new Dictionary<string, Aspect>();

            // Добавляем все аспекты из первого файла.
            foreach (var a in aspects1)
                merged[a.Id] = a;

            // Добавляем аспекты из второго файла, разрешая конфликты вручную.
            foreach (var a in aspects2)
            {
                if (merged.ContainsKey(a.Id))
                {
                    Console.WriteLine($"Конфликт для аспекта {a.Id}:");
                    Console.WriteLine("1. Версия из первого файла:");
                    Console.WriteLine(merged[a.Id].ToString());
                    Console.WriteLine("2. Версия из второго файла:");
                    Console.WriteLine(a.ToString());
                    Console.Write("Выберите версию (1 или 2): ");
                    string choice = Console.ReadLine();
                    if (choice.Trim() == "2")
                        merged[a.Id] = a;
                    // При выборе варианта 1 оставляем существующий вариант (не меняем merged[a.Id]).
                }
                else
                {
                    merged[a.Id] = a;
                }
            }
            Aspects = new List<Aspect>(merged.Values);
        }
    }
}