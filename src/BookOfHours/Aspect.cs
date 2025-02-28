using System;
using System.Collections.Generic;
namespace BookOfHours
{
    /// <summary>
    /// Класс, описывающий сущность "Аспект" в системе "Book of Hours".
    /// Содержит различные поля, отражающие свойства аспекта, такие как
    /// идентификатор, название, описание, флаги видимости и т. д.
    /// Реализует интерфейс <see cref="IJSONObject"/> для работы с полями в JSON.
    /// </summary>
    public class Aspect : IJSONObject
    {
        /// <summary>
        /// Уникальный идентификатор аспекта.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название аспекта.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Описание аспекта.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Флаг, указывающий, является ли объект аспектом (специальная характеристика).
        /// </summary>
        public bool IsAspect { get; set; }

        /// <summary>
        /// Флаг, указывающий, скрыт ли аспект.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Флаг, указывающий, нужно ли подбирать специальное изображение для аспекта.
        /// Если <c>true</c>, изображение не требуется.
        /// </summary>
        public bool NoArtNeeded { get; set; }

        /// <summary>
        /// Путь к иконке аспекта.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Порядок сортировки аспекта.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Дополнительные комментарии к аспекту.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Список, отображающий возможные переходы или смежные аспекты (любые ассоциации).
        /// </summary>
        public List<string> Commute { get; set; }

        /// <summary>
        /// Словарь с дополнительными параметрами (ambits).
        /// Ключ — имя параметра, значение — числовой показатель.
        /// </summary>
        public Dictionary<string, int> Ambits { get; set; }

        /// <summary>
        /// Указывает, наследуется ли данный аспект от другого аспекта (ID родителя).
        /// </summary>
        public string Inherits { get; set; }

        /// <summary>
        /// Конструктор по умолчанию. Инициализирует коллекции и поля пустыми значениями.
        /// </summary>
        public Aspect()
        {
            // Если какое-то поле отсутствует – заполняем значением по умолчанию.
            Id = "";
            Label = "";
            Desc = "";
            Commute = new List<string>();
            Ambits = new Dictionary<string, int>();
        }

        /// <summary>
        /// Получает перечень названий всех полей объекта (реализация интерфейса <see cref="IJSONObject"/>).
        /// </summary>
        /// <returns>Список строк с именами свойств.</returns>
        public IEnumerable<string> GetAllFields()
        {
            return new List<string>
            {
                "Id",
                "Label",
                "Desc",
                "IsAspect",
                "IsHidden",
                "NoArtNeeded",
                "Icon",
                "Sort",
                "Comments",
                "Inherits"
            };
        }

        /// <summary>
        /// Возвращает значение поля объекта JSON по имени (реализация интерфейса <see cref="IJSONObject"/>).
        /// Если поле не найдено, возвращается <c>null</c>.
        /// </summary>
        /// <param name="fieldName">Имя поля, значение которого требуется получить.</param>
        /// <returns>Значение поля или <c>null</c>, если поле не существует.</returns>
        public string GetField(string fieldName)
        {
            switch (fieldName.ToLower())
            {
                case "id": return Id;
                case "label": return Label;
                case "desc": return Desc;
                case "isaspect": return IsAspect.ToString();
                case "ishidden": return IsHidden.ToString();
                case "noartneeded": return NoArtNeeded.ToString();
                case "icon": return Icon;
                case "sort": return Sort;
                case "comments": return Comments;
                case "inherits": return Inherits;
                default: return null;
            }
        }

        /// <summary>
        /// Устанавливает значение поля по имени <paramref name="fieldName"/> (реализация интерфейса <see cref="IJSONObject"/>).
        /// Если поле не найдено, генерируется исключение <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="fieldName">Имя поля, которое нужно установить.</param>
        /// <param name="value">Значение, которое нужно установить.</param>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если указано несуществующее поле <paramref name="fieldName"/> 
        /// или значение не может быть преобразовано в нужный тип.</exception>
        public void SetField(string fieldName, string value)
        {
            switch (fieldName.ToLower())
            {
                case "id":
                    Id = value;
                    break;
                case "label":
                    Label = value;
                    break;
                case "desc":
                    Desc = value;
                    break;
                case "isaspect":
                    if (bool.TryParse(value, out bool res1))
                        IsAspect = res1;
                    else
                        throw new KeyNotFoundException($"Невозможно установить значение для {fieldName}");
                    break;
                case "ishidden":
                    if (bool.TryParse(value, out bool res2))
                        IsHidden = res2;
                    else
                        throw new KeyNotFoundException($"Невозможно установить значение для {fieldName}");
                    break;
                case "noartneeded":
                    if (bool.TryParse(value, out bool res3))
                        NoArtNeeded = res3;
                    else
                        throw new KeyNotFoundException($"Невозможно установить значение для {fieldName}");
                    break;
                case "icon":
                    Icon = value;
                    break;
                case "sort":
                    Sort = value;
                    break;
                case "comments":
                    Comments = value;
                    break;
                case "inherits":
                    Inherits = value;
                    break;
                default:
                    throw new KeyNotFoundException($"Поле {fieldName} не найдено.");
            }
        }

        /// <summary>
        /// Переопределённый метод для удобного вывода объекта в текстовом виде.
        /// </summary>
        /// <returns>Строковое представление аспекта в формате "Id: Label – Desc".</returns>
        public override string ToString()
        {
            return $"{Id}: {Label} – {Desc}";
        }
    }
}