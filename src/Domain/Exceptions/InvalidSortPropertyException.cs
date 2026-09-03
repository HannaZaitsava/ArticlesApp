using ArticlesApp.Domain.Exceptions.Base;

namespace ArticlesApp.Domain.Exceptions
{
    public class InvalidSortPropertyException : BaseDomainException
    {
        public InvalidSortPropertyException(string entityName, string propertyName)
            : base($"Entity '{entityName}' does not contain a sortable field '{propertyName}'")
        { }
    }
}
