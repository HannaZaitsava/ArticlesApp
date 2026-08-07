using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class InvalidSortPropertyException : BaseDomainException
    {
        public InvalidSortPropertyException(string entityName, string propertyName)
            : base($"Entity '{entityName}' does not contain a sortable field '{propertyName}'")
        { }
    }
}
