using Application.Abstractions.DataAccess;
using Application.CQRS.Queries.TagQueries.GetAllTags;
using Application.DTOs.Tags;
using Application.RequestFeatures.OffsetPagination;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

namespace ArticlesApp.Tests.UnitTests.Features.Tags
{
    public class GetAllTagsQueryHandlerTests
    {
        [Theory, AutoMoqData]
        internal async Task Handle_ShouldReturnSuccessfulPagedResult(
            [Frozen] Mock<ITagRepository> repositoryMock,
            GetAllTagsQuery query,
            OffsetPagedResult<TagShortInfoResponseDTO> pagedData,
            GetAllTagsQueryHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(x => x.GetOffsetPagedListProjectedAsync<TagShortInfoResponseDTO>(
                    It.IsAny<OffsetPaginationParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedData);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(pagedData);
        }

        //[Theory, AutoMoqData]
        //internal async Task Handle_ShouldCallRepositoryOnce(
        //    [Frozen] Mock<IBaseRepository<Tag>> repositoryMock,
        //    GetAllTagsQuery query,
        //    GetAllTagsQueryHandler handler)
        //{
        //    // Act
        //    await handler.Handle(query, default);

        //    // Assert
        //    repositoryMock.Verify(x => x.GetPagedListBySpecAsync<TagShortInfoResponseDTO>(
        //        It.IsAny<ISpecification<Tag>>(),
        //        It.IsAny<CancellationToken>()),
        //        Times.Once);
        //}
    }
}
