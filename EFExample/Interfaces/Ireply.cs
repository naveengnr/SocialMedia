using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Ireply
    {
        public Task<string> AddReply(ReplyDTO reply);
        public Task<List<ReplyGetDTO>> GetReplies(int ReplyId);
        public Task<String> UpdateReplies(ReplyUpdateDTO updateDTO);
    }
}
