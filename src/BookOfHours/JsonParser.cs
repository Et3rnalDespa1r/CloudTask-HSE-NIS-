using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BookOfHours
{
    /// <summary>
    /// Статический класс для чтения и записи списка аспектов из/в JSON‑формат.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Считывает файл в JSON-формате, валидирует структуру и парсит объекты аспекта.
        /// </summary>
        /// <param name="filePath">Путь к JSON-файлу.</param>
        /// <returns>Список аспектов, считанных из JSON.</returns>
        /// <exception cref="Exception">Выбрасывается, если файл не прошёл упрощённую валидацию
        /// или имеет некорректную структуру.</exception>
        public static List<Aspect> ReadJson(string filePath)
        {
            // Определяем кодировку, если необходимо – можно использовать StreamReader с автоопределением BOM.
            string content;
            using (var reader = new StreamReader(filePath, detectEncodingFromByteOrderMarks: true))
            {
                content = reader.ReadToEnd();
            }
            content = content.Trim();

            if (!ValidateJson(content))
                throw new Exception("Файл не соответствует стандарту RFC 8259 (упрощённая проверка).");

            int startIndex = content.IndexOf("\"elements\":");
            if (startIndex < 0)
                throw new Exception("Отсутствует ключ \"elements\".");
            startIndex = content.IndexOf("[", startIndex);
            int endIndex = content.LastIndexOf("]");
            if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex)
                throw new Exception("Некорректная структура JSON: не найден корректный массив элементов.");

            string arrayContent = content.Substring(startIndex + 1, endIndex - startIndex - 1);

            List<string> objectStrings = SplitJsonObjects(arrayContent);
            List<Aspect> aspects = new List<Aspect>();
            foreach (var obj in objectStrings)
            {
                Aspect a = ParseAspect(obj);
                if (a != null)
                    aspects.Add(a);
            }
            return aspects;
        }

        /// <summary>
        /// Простая проверка структуры JSON, необходимой для дальнейшего парсинга.
        /// </summary>
        /// <param name="content">Содержимое JSON-файла.</param>
        /// <returns><c>true</c>, если файл содержит ключ "elements" и начинается/заканчивается фигурной скобкой.</returns>
        private static bool ValidateJson(string content)
        {
            if (!content.StartsWith("{") || !content.EndsWith("}"))
                return false;
            if (!content.Contains("\"elements\""))
                return false;
            return true;
        }

        /// <summary>
        /// Разделяет строку, содержащую несколько объектов JSON, на отдельные объекты.
        /// Учитывает вложенные фигурные скобки.
        /// </summary>
        /// <param name="arrayContent">Строка вида {"key":"value"}, {"key2":"value2"}, ...</param>
        /// <returns>Список подстрок, каждая из которых является отдельным JSON-объектом.</returns>
        private static List<string> SplitJsonObjects(string arrayContent)
        {
            List<string> objects = new List<string>();
            int braceLevel = 0;
            int startIndex = -1;
            for (int i = 0; i < arrayContent.Length; i++)
            {
                char c = arrayContent[i];
                if (c == '{')
                {
                    if (braceLevel == 0)
                        startIndex = i;
                    braceLevel++;
                }
                else if (c == '}')
                {
                    braceLevel--;
                    if (braceLevel == 0 && startIndex != -1)
                    {
                        int length = i - startIndex + 1;
                        objects.Add(arrayContent.Substring(startIndex, length));
                        startIndex = -1;
                    }
                }
            }
            return objects;
        }

        /// <summary>
        /// Парсит строку, содержащую один JSON-объект аспекта, и создает <see cref="Aspect"/>.
        /// Извлекает основные свойства, включая булевые поля, а также массивы и вложенные объекты.
        /// </summary>
        /// <param name="jsonObj">Строка JSON в фигурных скобках.</param>
        /// <returns>Сконструированный объект <see cref="Aspect"/> со считанными свойствами.</returns>
        private static Aspect ParseAspect(string jsonObj)
        {
            Aspect a = new Aspect();

            // Регулярное выражение, позволяющее найти пары "ключ": значение
            Regex regex = new Regex("\"(\\w+)\"\\s*:\\s*(\"(.*?)\"|(true|false)|(-?\\d+)|(\\[.*?\\])|(\\{.*?\\}))", RegexOptions.Compiled);
            var matches = regex.Matches(jsonObj);
            foreach (Match m in matches)
            {
                string key = m.Groups[1].Value;
                // strVal – строковое значение в кавычках
                string strVal = m.Groups[3].Success ? m.Groups[3].Value : null;
                // boolVal – булево значение (true/false)
                string boolVal = m.Groups[4].Success ? m.Groups[4].Value : null;
                // numVal – целое число
                string numVal = m.Groups[5].Success ? m.Groups[5].Value : null;
                // arrayVal – массив в квадратных скобках
                string arrayVal = m.Groups[6].Success ? m.Groups[6].Value : null;
                // objVal – вложенный объект в фигурных скобках
                string objVal = m.Groups[7].Success ? m.Groups[7].Value : null;

                switch (key.ToLower())
                {
                    case "id":
                        a.Id = strVal;
                        break;
                    case "label":
                        a.Label = strVal;
                        break;
                    case "desc":
                    case "description":
                        a.Desc = strVal;
                        break;
                    case "isaspect":
                        a.IsAspect = boolVal != null && bool.Parse(boolVal);
                        break;
                    case "ishidden":
                        a.IsHidden = boolVal != null && bool.Parse(boolVal);
                        break;
                    case "noartneeded":
                        a.NoArtNeeded = boolVal != null && bool.Parse(boolVal);
                        break;
                    case "icon":
                        a.Icon = strVal;
                        break;
                    case "sort":
                        a.Sort = strVal;
                        break;
                    case "comments":
                        a.Comments = strVal;
                        break;
                    case "commute":
                        if (arrayVal != null)
                        {
                            // Убираем квадратные скобки
                            string arr = arrayVal.Trim('[', ']');
                            var items = arr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in items)
                            {
                                string trimmed = item.Trim().Trim('"');
                                if (!string.IsNullOrEmpty(trimmed))
                                    a.Commute.Add(trimmed);
                            }
                        }
                        break;
                    case "ambits":
                        if (objVal != null)
                        {
                            // Убираем фигурные скобки
                            string inner = objVal.Trim('{', '}');
                            var pairs = inner.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var pair in pairs)
                            {
                                var parts = pair.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int ambVal))
                                    a.Ambits[parts[0].Trim().Trim('"')] = ambVal;
                            }
                        }
                        break;
                    case "inherits":
                        a.Inherits = strVal;
                        break;
                }
            }
            return a;
        }

        /// <summary>
        /// Формирует JSON-строку из списка аспектов.
        /// </summary>
        /// <param name="aspects">Коллекция аспектов <see cref="Aspect"/>.</param>
        /// <returns>Строка, представляющая собой JSON с массивом объектов (elements).</returns>
        public static string WriteJson(List<Aspect> aspects)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"elements\": [");
            for (int i = 0; i < aspects.Count; i++)
            {
                var a = aspects[i];
                sb.AppendLine("    {");
                sb.AppendLine($"      \"id\": \"{a.Id}\",");
                sb.AppendLine($"      \"label\": \"{a.Label}\",");
                sb.AppendLine($"      \"desc\": \"{a.Desc}\",");
                sb.AppendLine($"      \"isAspect\": {a.IsAspect.ToString().ToLower()},");
                sb.AppendLine($"      \"isHidden\": {a.IsHidden.ToString().ToLower()},");
                sb.AppendLine($"      \"noArtNeeded\": {a.NoArtNeeded.ToString().ToLower()},");
                if (!string.IsNullOrEmpty(a.Icon))
                    sb.AppendLine($"      \"icon\": \"{a.Icon}\",");
                if (!string.IsNullOrEmpty(a.Sort))
                    sb.AppendLine($"      \"sort\": \"{a.Sort}\",");
                if (!string.IsNullOrEmpty(a.Comments))
                    sb.AppendLine($"      \"comments\": \"{a.Comments}\",");
                // Если имеются массивы (commute) и словари (ambits), их можно добавить аналогичным образом.

                sb.Append("    }");
                if (i < aspects.Count - 1)
                    sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
