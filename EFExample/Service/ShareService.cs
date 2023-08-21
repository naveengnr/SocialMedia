using EFExample.Models;
using EFExample.DTO;
using EFExample.Interfaces;
using System.Threading.Tasks;
using System;

namespace EFExample.Service
{
    public class ShareService : Ishare
    {
        public readonly SocialMediaContext _context;

        public ShareService(SocialMediaContext context)
        {
            _context = context;
        }

        public List<ShareGetDTO> GetShares(int UserId = 0 , int PostId = 0)
        {
            IQueryable<Share> query = _context.Shares;

            if (UserId > 0)
            {
                 query = query.Where(e => e.UserId == UserId && e.IsDeleted == false);
            }
            else if(PostId > 0)
            {
                 query = query.Where(e => e.PostId == PostId && e.IsDeleted == false);
            }

            var shares = query.ToList();

            if (shares != null)
            {
                var share = shares.Select(e => new ShareGetDTO
                {
                    ShareId = e.ShareId,
                    UserId = e.UserId,
                    PostId = e.PostId,
                    DateShared = e.DateShared
                }).ToList();

                return share;
            }
            else
            {
                return null;
            }
        }

        public string DeleteShare(int ShareId)
        {
            var share = _context.Shares.FirstOrDefault(e => e.ShareId == ShareId && e.IsDeleted == false);

            if (share != null)
            {
                share.IsDeleted = true;

                _context.SaveChanges();

                return "Sucessfully Deleted";
            }
            else
            {
                return null;
            }
        }


        public String AddShare(ShareDTO share)
        {
            bool Userexists = _context.Users.Any(e => e.UserId.Equals(share.UserId) && e.IsDeleted == false);
            bool Postexists = _context.Posts.Any(e => e.PostId.Equals(share.PostId) && e.IsDeleted == false);

            if (Userexists && Postexists)
            {

                Share shares = new Share()
                {
                    UserId = share.UserId,
                    PostId = share.PostId,
                    DateShared = DateTime.Now
                };

                _context.Shares.Add(shares);
                _context.SaveChanges();

                return "successfully shared";
            }
            else
            {
                return null;
            }
        }
    }
}
