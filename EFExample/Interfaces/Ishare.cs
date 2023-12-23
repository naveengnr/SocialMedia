using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Ishare
    {
        public Task<List<ShareGetDTO>> GetShares(int UserId , int PostId);
        public Task<string> DeleteShare(int ShareId);
        public Task<string> AddShare(ShareDTO share);
    }
}
