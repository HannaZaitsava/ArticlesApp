using Domain.Enums;

namespace Domain.Errors
{
    public sealed record Error(string Name, string Message, ErrorType Type = ErrorType.Failure)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }
}
