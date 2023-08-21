using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Icomments
    {
        public String AddComment(CommentDTO comment);
        public List<CommentGetDTO> GetComments(int CommentId);
        public string UpdateComment(CommentUpdateDTO updateDTO);
    }
}
