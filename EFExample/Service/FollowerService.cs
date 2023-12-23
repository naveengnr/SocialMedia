using EFExample.Models;
using EFExample.DTO;
using Microsoft.AspNetCore.Components.Forms;
using EFExample.Interfaces;

namespace EFExample.Service
{
    public class FollowerService : Ifollower
    {
        public readonly SocialMediaContext _context;

        public FollowerService(SocialMediaContext context)
        {
            _context = context;
        }

        public String AddFollowing(FollowerDTO follower)
        { 
            
            var Users = _context.Followers.FirstOrDefault(e => e.UserId.Equals(follower.FollowingUserId));
            bool Exists = Users.FollowingUserId.Equals(follower.UserId);
            var Follower = _context.Followers.FirstOrDefault(e => e.UserId.Equals(follower.UserId));
            bool UserExists = _context.Users.Any(e => e.UserId.Equals(follower.UserId));
            bool FollowerExists = _context.Users.Any(e => e.UserId.Equals(follower.FollowingUserId));

            if (UserExists && FollowerExists)
            {

                if (Exists && Follower != null)
                {
                    Users.FollowerUserId = follower.FollowingUserId;
                    Follower.FollowingUserId = follower.UserId;

                    _context.SaveChanges();

                    return "Followed Successfully";
                }
                else
                {

                    Follower follower1 = new Follower()
                    {
                        UserId = follower.UserId,
                        FollowingUserId = follower.FollowingUserId,
                        DateFollowed = DateTime.Now
                    };
                    _context.Followers.Add(follower1);

                    Follower follower2 = new Follower()
                    {
                        UserId = follower1.FollowingUserId,
                        FollowerUserId = follower1.UserId,
                        DateFollowed = DateTime.Now
                    };
                    _context.Followers.Add(follower2);
                    _context.SaveChanges();

                    return "Followed Successfully";
                }
            }
            else
            {
                return null;
            }
        }
    }
}
