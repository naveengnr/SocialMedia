using EFExample.Models;
using EFExample.DTO;
using EFExample.Interfaces;
using System.Threading.Tasks;
using System;
using EFExample.Email;
using Microsoft.EntityFrameworkCore;


namespace EFExample.Service
{
    public class ShareService : Ishare
    {
        public readonly SocialMediaContext _context;
        public readonly EmailService _emailService;
        public readonly ILogger<ShareService> _logger;
        public ShareService(SocialMediaContext context, EmailService emailService, ILogger<ShareService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// This method using for the Get the shares based on UserId  and PostId parameter
        /// </summary>
        public async Task<List<ShareGetDTO>> GetShares(int UserId = 0, int PostId = 0)
        {
            try
            {
                IQueryable<Share> query = _context.Shares;

                if (UserId > 0)
                {
                    query = query.Where(e => e.UserId == UserId && e.IsDeleted == false);
                }
                else if (PostId > 0)
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new List<ShareGetDTO> { new ShareGetDTO { ErrorMessage = ex.Message } };

            }
        }

        /// <summary>
        /// This method using for the Delete the share based on ShareId parameter
        /// </summary>
        public async Task<string> DeleteShare(int ShareId)
        {
            try {
                var share = _context.Shares.FirstOrDefault(e => e.ShareId == ShareId && e.IsDeleted == false);

                if (share != null)
                {
                    share.IsDeleted = true;

                    await _context.SaveChangesAsync();

                    return "Sucessfully Deleted";
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
            }

        /// <summary>
        /// This method using for the Adding the share based on share parameter
        /// </summary>
        public async Task<String> AddShare(ShareDTO share)
        {

            try
            {
                bool Userexists =  _context.Users.Any(e => e.UserId.Equals(share.UserId) && e.IsDeleted == false);
                bool Postexists = _context.Posts.Any(e => e.PostId.Equals(share.PostId) && e.IsDeleted == false);

                if (Userexists == false || Postexists == false)
                {

                    return null;

                }
                Share shares = new Share()
                {
                    UserId = share.UserId,
                    PostId = share.PostId,
                    DateShared = DateTime.Now
                };

                _context.Shares.Add(shares);

                var ShareMail = from p in _context.Posts
                                join s in _context.Shares on p.PostId equals s.PostId
                                join u in _context.Users on p.PostUserId equals u.UserId
                                where (p.PostId == share.PostId)
                                select new
                                {
                                    Puserid = p.PostUserId,
                                }
                                into k
                                join user in _context.Users on k.Puserid equals user.UserId
                                select new
                                {
                                    PUser = user.Username,
                                    pUserEmail = user.Email
                                }
                                into m
                                join users in _context.Users on share.UserId equals users.UserId
                                select new
                                {
                                    shareusername = users.Username,
                                    PostUsername = m.PUser,
                                    PostUserEmail = m.pUserEmail
                                };


                string ReceiverName = null;
                string SenderName = null;
                string ReceiverEmail = null;

                foreach (var s in ShareMail)
                {
                    ReceiverName = s.PostUsername;
                    SenderName = s.shareusername;
                    ReceiverEmail = s.PostUserEmail;
                };

                _emailService.SendEmail( ReceiverEmail,"postShare" , SenderName , ReceiverName); 


                await _context.SaveChangesAsync();



            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return ex.Message;
            }
            return "successfully shared";
        }
    }
}
