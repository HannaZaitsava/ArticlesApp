namespace Domain.Result
{
    /// <summary>
    /// Интерфейс для реализации паттерна Адаптер
    /// </summary>
    /// <remarks>
    /// Нужен для корректной работы кеширования.
    /// Т.к. в бизнес-логике реализован Result Pattern, то кешировать нужно не все значение Result<T>, а только его Value.
    /// </remarks>
    public interface IResultAdapter
    {
        bool IsSuccess { get; }
        object? RawValue { get; }

        /// <summary>
        /// Метод для пересоздания объекта ошибки
        /// </summary>
        /// <returns></returns>
        object ToFailureResult(); 
    }
}
