namespace ArticlesAPI.Models.Requests
{
    public sealed record TagApiRequest(string? Color, string Label = default!);   
}
