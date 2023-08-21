using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Ireply
    {
        public string AddReply(ReplyDTO reply);
        public List<ReplyGetDTO> GetReplies(int ReplyId);
        public String UpdateReplies(ReplyUpdateDTO updateDTO);
    }
}
