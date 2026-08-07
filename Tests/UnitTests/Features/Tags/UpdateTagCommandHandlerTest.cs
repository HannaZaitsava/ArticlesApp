using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Application.CQRS.Commands.TagCommands.UpdateTag;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Errors;
using FluentAssertions;
using MediatR;
using Moq;

namespace ArticlesApp.Tests.UnitTests.Features.Tags
{
    public class UpdateTagCommandHandlerTest
    {
        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagExists_ShouldUpdateDetailsAndReturnSuccessAndInvalidateCache(
            [Frozen] Mock<ITagRepository> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            UpdateTagCommand command,
            Tag tagEntity,
            UpdateTagCommandHandler handler)
        {
            // Arrange           
            //tagEntity.Id = command.Id; // не обязательно, так как мы не проверяем конкретные данные, а только что они были обновлены

            repositoryMock
                .Setup(r => r.GetTagWithFullInfoAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagEntity);

            // ожидаемые ключи кэша
            var expectedCacheKeys = new HashSet<string>
            {
                CacheTags.Tags,
                CacheTags.Tag(command.Id)
            };
            foreach (var article in tagEntity.Articles)
                expectedCacheKeys.Add(CacheTags.Article(article.Id));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Проверка маппинга (Sociable Test): данные из команды должны быть в сущности
            tagEntity.Label.Should().Be(command.Label);
            tagEntity.Color.Should().Be(command.Color);

            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
                       
            mediatorMock.Verify(m => m.Publish(
                It.Is<CacheInvalidationEvent>(e => e.Tags.SetEquals(expectedCacheKeys)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagDoesNotExist_ShouldReturnFailureAndNotInvalidateCache(
            [Frozen] Mock<ITagRepository> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            UpdateTagCommand command,
            UpdateTagCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(r => r.GetTagWithFullInfoAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
                        
            result.Errors.Should().ContainSingle()
                .Which.Should().Be(TagErrors.TagNotFound(command.Id));

            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            mediatorMock.Verify(m => m.Publish(
                It.IsAny<CacheInvalidationEvent>(),
                It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagHasNoRelatedArticles_ShouldInvalidateOnlyTagCacheKeys(
            [Frozen] Mock<ITagRepository> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            UpdateTagCommand command,
            Tag tagEntity,
            UpdateTagCommandHandler handler)
        {
            // Arrange
            tagEntity.Articles = [];

            repositoryMock
                .Setup(r => r.GetTagWithFullInfoAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagEntity);

            var expectedCacheKeys = new HashSet<string>
            {
                CacheTags.Tags,
                CacheTags.Tag(command.Id)
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            mediatorMock.Verify(
                m => m.Publish(
                    It.Is<CacheInvalidationEvent>(e =>
                        e.Tags.Count == 2 && e.Tags.SetEquals(expectedCacheKeys)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}

