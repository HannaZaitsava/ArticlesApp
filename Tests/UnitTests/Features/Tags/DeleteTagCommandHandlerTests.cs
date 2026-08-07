using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Application.CQRS.Commands.TagCommands.DeleteTag;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Errors;
using FluentAssertions;
using MediatR;
using Moq;

namespace ArticlesApp.Tests.UnitTests.Features.Tags
{
    public class DeleteTagCommandHandlerTests
    {
        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagExists_ShouldDeleteAndPublishInvalidation(
            [Frozen] Mock<IBaseRepository<Tag>> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            DeleteTagCommand command,
            Tag tagEntity, 
            DeleteTagCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagEntity);
            
            // Собираем ожидаемые ключи для проверки события
            var expectedKeys = new HashSet<string> { CacheTags.Tags, CacheTags.Tag(command.Id) };
            foreach (var article in tagEntity.Articles)
                expectedKeys.Add(CacheTags.Article(article.Id));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Проверяем удаление и сохранение сущности
            repositoryMock.Verify(x => x.Remove(tagEntity), Times.Once);
            repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Проверяем публикацию события инвалидации кеша с правильным набором тегов
            mediatorMock.Verify(x => x.Publish(
                It.Is<CacheInvalidationEvent>(e => e.Tags.SetEquals(expectedKeys)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory, AutoMoqData]
        internal async Task Handle_WhenTagNotFound_ShouldReturnFailure(
            [Frozen] Mock<IBaseRepository<Tag>> repositoryMock,
            [Frozen] Mock<IMediator> mediatorMock,
            DeleteTagCommand command,
            DeleteTagCommandHandler handler)
        {
            // Arrange
            repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
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
