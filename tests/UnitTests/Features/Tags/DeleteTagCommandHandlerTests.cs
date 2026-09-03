using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.CQRS.Commands.TagCommands.DeleteTag;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Tests.UnitTests.Attributes;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;

namespace ArticlesApp.Tests.UnitTests.Features.Tags
{
    public class DeleteTagCommandHandlerTests
    {
        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagExists_ShouldDeleteAndPublishInvalidation(
            [Frozen] Mock<ITagRepository> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            DeleteTagCommand command,
            Tag tagEntity, 
            DeleteTagCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(x => x.GetTagWithFullInfoAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagEntity);
            
            // Собираем ожидаемые ключи для проверки события
            var expectedTagsToInvalidate = new HashSet<string> { CacheTags.Tags, CacheTags.Tag(command.Id) };
            foreach (var article in tagEntity.Articles)
                expectedTagsToInvalidate.Add(CacheTags.Article(article.Id));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Проверяем удаление и сохранение сущности
            repositoryMock.Verify(x => x.Remove(tagEntity), Times.Once);
            repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Проверяем публикацию события инвалидации кеша с правильным набором тегов
            mediatorMock.Verify(x => x.Publish(
                It.Is<CacheInvalidationEvent>(e => e.Tags.SetEquals(expectedTagsToInvalidate)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagNotFound_ShouldReturnFailure(
            [Frozen] Mock<ITagRepository> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            DeleteTagCommand command,
            DeleteTagCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(x => x.GetTagWithFullInfoAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle()
                .Which.Should().Be(TagErrors.TagNotFound(command.Id));

            // Проверяем, что сущность не удалялась и события обновления тегов кеша не было
            repositoryMock.Verify(x => x.Remove(It.IsAny<Tag>()), Times.Never);
            mediatorMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
