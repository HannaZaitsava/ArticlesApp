using System.Linq.Expressions;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.CreateArticleCategory;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Tests.UnitTests.Attributes;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;

namespace ArticlesApp.Tests.UnitTests.Features.ArticleCategories
{
    public class CreateArticleCategoryCommandHandlerTests
    {
        [Theory, AutoMoqData]
        internal async Task Handle_WhenArticleCategoryDoesNotExist_ShouldCreateArticleCategoryAndReturnSuccess(
            [Frozen] Mock<IBaseRepository<ArticleCategory>> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            CreateArticleCategoryCommand command,
            CreateArticleCategoryCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(r => r.IsExistingAsync(It.IsAny<Expression<Func<ArticleCategory, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<ArticleCategory>(), It.IsAny<CancellationToken>()))
                .Callback<ArticleCategory, CancellationToken>((category, _) => category.Id = Guid.NewGuid())
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.Id.Should().NotBeEmpty();
            result.Value.Name.Should().Be(command.Name);

            repositoryMock.Verify(r => r.AddAsync(It.IsAny<ArticleCategory>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var expectedCacheTagsToInvalidate = new HashSet<string> { CacheTags.ArticleCategories };
            mediatorMock.Verify(m => m.Publish(
                It.Is<CacheInvalidationEvent>(e => e.Tags.SetEquals(expectedCacheTagsToInvalidate)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory, AutoMoqData]
        internal async Task Handle_WhenArticleCategoryAlreadyExists_ShouldReturnFailureWithProperError(
            [Frozen] Mock<IBaseRepository<ArticleCategory>> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            CreateArticleCategoryCommand command,
            CreateArticleCategoryCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(r => r.IsExistingAsync(It.IsAny<Expression<Func<ArticleCategory, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle()
                .Which.Should().Be(ArticleCategoryErrors.ArticleCategoryAlreadyExists(command.Name));

            repositoryMock.Verify(r => r.AddAsync(It.IsAny<ArticleCategory>(), It.IsAny<CancellationToken>()), Times.Never);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            mediatorMock.Verify(m => m.Publish(
                It.IsAny<CacheInvalidationEvent>(),
                It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
