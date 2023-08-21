using EFExample.DTO;

namespace EFExample.Interfaces
{
    public interface Ishare
    {
        public List<ShareGetDTO> GetShares(int UserId , int PostId);
        public string DeleteShare(int ShareId);
        public string AddShare(ShareDTO share);
    }
}
