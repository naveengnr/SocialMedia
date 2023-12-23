using EFExample.DTO;
using EFExample.Models;

namespace EFExample.Interfaces
{
    public interface Ipost
    {
        public List<PostAllDetailsDTO> GetAllPostsContent();
        public Task<string> AddPost(PostDTO post , int Id);
        public Task<List<PostGetDTO>> GetPosts(int PostId , int PostUserId);
        public Task<string> UpdatePost(PostUpdateDTO updateDTO,int Id);
        public Task<IQueryable<PostContentDTO>> Posts();
    }
}
