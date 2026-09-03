using ArticlesApp.Application.DTOs.Comments;
using ArticlesApp.Domain.Entities;
using Mapster;

namespace ArticlesApp.Application.MappingProfiles
{
    public class CommentMappingConfig : IRegister
    {      
        public void Register(TypeAdapterConfig config)
        {           
            config.NewConfig<Comment, CommentResponseDTO>()
                // Игнорируем рекурсивное свойство для проекций в БД,
                // т.к. сборка дерева ответов реализована в отдельном алгоритме (CommentResponseDTOExtensions)
                .Ignore(dest => dest.Replies); 
        }
    }
}
