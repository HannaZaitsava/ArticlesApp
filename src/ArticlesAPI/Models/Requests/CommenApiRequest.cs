namespace ArticlesAPI.Models.Requests
{
    public sealed record CommenApiRequest(Guid? ParentId, string Text);    
}
