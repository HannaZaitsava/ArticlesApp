namespace ArticlesAPI.Models.Requests
{
    public sealed record ArticleApiRequest
    {
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;

        //public IFormFile? CoverImage { get; set; }

        public IReadOnlyCollection<Guid>?  Categories { get; init; }
        public IReadOnlyCollection<Guid>? Tags { get; init; }
    }
}
