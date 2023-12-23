using EFExample.Interfaces;
using EFExample.Models;
using EFExample.DTO;
using Microsoft.EntityFrameworkCore;
using EFExample.Controllers;

namespace EFExample.Service
{
    public class FriendService : Ifriend
    {
        public readonly SocialMediaContext _Context;
        public readonly ILogger<FriendController> _logger;
        
        public FriendService(SocialMediaContext context , ILogger<FriendController> logger)
        {
            _Context = context;
            _logger = logger;
        }

        /// <summary>
        /// This method using for the Send FriendRequest  based on friendDTO parameter
        /// </summary>
        public async Task<String> AddFriend(FriendDTO friendDTO)
        {
            try {
                var friend = (from u in _Context.Users
                              where u.UserId == friendDTO.UserId
                              select new
                              {
                                  u = u.UserId,
                                  a = u.AccountType

                              } into k
                              join uk in _Context.Users on friendDTO.FollowersId equals uk.UserId
                              select new
                              {
                                  userid = k.u,
                                  followersid = uk.UserId,
                                  accounttype = k.a
                              });

                int userid = 0;
                int followersid = 0;
                string accounttype = null;

                foreach (var i in friend)
                {
                    userid = i.userid;
                    followersid = i.followersid;
                    accounttype = i.accounttype;
                }


                string type = null;


                if (accounttype == "public")

                {
                    type = "Accepted";
                }
                else if (accounttype == "private")

                {
                    type = "Pending";
                }

                if (userid != 0 && followersid != 0)
                {
                    var dto = new Friend()
                    {

                        UserId = friendDTO.UserId,
                        FollowersId = friendDTO.FollowersId,
                        RequestStatus = type

                    };

                    _Context.Friends.Add(dto);
                    await _Context.SaveChangesAsync();

                    return "request sent ";


                }
                else
                {
                    return null;
                }

            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);   
                return ex.Message;
            }
            }

        /// <summary>
        /// This method using for the Accept FriendRequest  based on friendDTO parameter
        /// </summary>
        public async Task<string> AcceptRequest(FriendDTO friendDTO)
        {
            try {
                var friend = _Context.Friends.FirstOrDefault(e => e.UserId == friendDTO.UserId && e.FollowersId == friendDTO.FollowersId);

                if (friend == null)
                {
                    return null;
                }
                else
                {

                    friend.RequestStatus = "Accepted";


                    await _Context.SaveChangesAsync();

                    return "Request Accepted";
                }
            }
            catch(Exception ex)
            { 
                _logger.LogError(ex.Message, ex);
                return ex.Message;
            }
            }
    }
}
