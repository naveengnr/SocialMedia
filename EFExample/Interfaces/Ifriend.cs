using EFExample.DTO;
namespace EFExample.Interfaces
{
    public interface Ifriend
    {
        public  Task<String> AddFriend(FriendDTO friendDTO);
        public Task<string> AcceptRequest(FriendDTO friendDTO);
    }
}
