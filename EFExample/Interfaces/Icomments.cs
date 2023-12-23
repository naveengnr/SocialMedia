using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Icomments
    {
        public Task<String> AddComment(CommentDTO comment);
        public Task< List<CommentGetDTO> >GetComments(int CommentId);
        public Task<string> UpdateComment(CommentUpdateDTO updateDTO);
    }
}
