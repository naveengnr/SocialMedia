using EFExample.DTO;
using EFExample.Models;

namespace EFExample.Interfaces
{
    public interface Ipost
    {
        public List<PostAllDetailsDTO> GetAllPostsContent();
        public string AddPost(PostDTO post);
        public List<PostGetDTO> GetPosts(int PostId , int PostUserId);
        public string UpdatePost(PostUpdateDTO updateDTO);
    }
}
