using EFExample.DTO;
namespace EFExample.Interfaces
{
    public interface Ilikes
    {
        public  Task<string> AddLikes(LikesDTO likesDto);
    }
}
