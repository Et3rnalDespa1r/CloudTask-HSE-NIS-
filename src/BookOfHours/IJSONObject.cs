using System.Collections.Generic;

namespace BookOfHours
{
    /// <summary>
    /// Интерфейс, предоставляющий методы для работы с JSON-объектом.
    /// </summary>
    public interface IJSONObject
    {
        /// <summary>
        /// Возвращает список названий всех публичных полей (свойств) данного объекта.
        /// </summary>
        /// <returns>Перечисление названий свойств объекта.</returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// Возвращает значение поля с именем <paramref name="fieldName"/>.
        /// Если такого поля не существует, возвращает <c>null</c>.
        /// </summary>
        /// <param name="fieldName">Имя поля, значение которого требуется получить.</param>
        /// <returns>Значение поля в виде строки или <c>null</c>, если поле не найдено.</returns>
        string GetField(string fieldName);

        /// <summary>
        /// Устанавливает значение поля с именем <paramref name="fieldName"/>.
        /// Если такого поля не существует, генерируется исключение <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="fieldName">Имя поля, значение которого требуется установить.</param>
        /// <param name="value">Значение, которое необходимо установить.</param>
        /// <exception cref="KeyNotFoundException">Если поле <paramref name="fieldName"/> не найдено,
        /// либо значение не может быть преобразовано к нужному типу.</exception>
        void SetField(string fieldName, string value);
    }
}