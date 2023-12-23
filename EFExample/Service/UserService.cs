using EFExample.Models;
using EFExample.Secutiy;
using EFExample.DTO;
using EFExample.Email;
using EFExample.Interfaces;
using EFExample.Cache;
using Microsoft.EntityFrameworkCore;

namespace EFExample.Service
{

    public class UserService : Iuser
    {
        public readonly SocialMediaContext _context;

        public readonly Icache _cache;

        public readonly ILogger<UserService> _logger;

        public readonly EmailService _emailService;
        public UserService(SocialMediaContext context, EmailService emailService, Icache cache, ILogger<UserService> logger)
        {
            _context = context; 
            _emailService = emailService;
            _cache = cache;
            _logger = logger;
        }




        /// <summary>
        /// This method using for the GetAll the user 
        /// </summary>
        public async Task<IQueryable<GetAllDTO>> GetAll()
        {
            try
            {
                var Cache = _cache.GetData<IQueryable<GetAllDTO>>("Users");

                if (Cache != null)
                {

                    return Cache;
                }

                var users = _context.Users.Select(u => new GetAllDTO

                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    DateOfBirth = u.DateOfBirth,
                    DateUserJoined = u.DateUserJoined
                });

                var expirationTime = DateTimeOffset.Now.AddHours(2);

                _cache.SetData<IQueryable<GetAllDTO>>("Users", users, expirationTime);

                 
                return users;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return new List<GetAllDTO>
                {
                  new GetAllDTO
                {
                ErrorMessage = ex.Message

            }
        }.AsQueryable();

            }
        }
        /// <summary>
        /// This method using for the Get the user based on UserId parameter
        /// </summary>
        public async Task<GetAllDTO> GetById(int UserId)
        {
            try
            {
                var users = _context.Users.FirstOrDefault(e => e.UserId == UserId && e.IsDeleted == false);
                if (users != null)
                {
                    GetAllDTO user = new GetAllDTO()
                    {
                        UserId = users.UserId,
                        Username = users.Username,
                        Email = users.Email,
                        DateOfBirth = users.DateOfBirth,
                        DateUserJoined = users.DateUserJoined

                    };
                    return user;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetAllDTO
                {
                    ErrorMessage = ex.Message
                };

            }
        }
        /// <summary>
        /// This method using for the delete the user based on Username parameter
        /// </summary>

        public async Task<GetAllDTO> GetByName(String Username)
        {
            try
            {
                var users = _context.Users.FirstOrDefault(e => e.Username.Equals(Username) && e.IsDeleted == false);
                if (users != null)
                {
                    GetAllDTO user = new GetAllDTO()
                    {
                        UserId = users.UserId,
                        Username = users.Username,
                        Email = users.Email,
                        DateOfBirth = users.DateOfBirth,
                        DateUserJoined = users.DateUserJoined

                    };
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetAllDTO
                {
                    ErrorMessage = ex.Message
                };

            }
        }

        /// <summary>
        /// This method using for the delete the user based on UserId parameter
        /// </summary>
        //public async Task<string> DeleteUser(int UserId , int Id)
        //{
        //    try
        //    {
        //        if (UserId == Id)
        //        {

        //            var user = _context.Users.FirstOrDefault(e => e.UserId == UserId && e.IsDeleted == false);

        //            if (user != null)

        //            {
        //                user.IsDeleted = true;

        //                var UserPost = _context.Posts.Where(e => e.PostUserId == UserId && e.IsDeleted == false).ToList();

        //                foreach (var Post in UserPost)
        //                {
        //                    Post.IsDeleted = true;

        //                    var Comment = _context.Comments.Where(e => e.PostId == Post.PostId && e.IsDeleted == false).ToList();

        //                    foreach (var Comments in Comment)
        //                    {
        //                        Comments.IsDeleted = true;

        //                        var Replies = _context.Replies.Where(e => e.CommentId == Comments.CommentId && e.IsDeleted == false).ToList();

        //                        foreach (var Reply in Replies)
        //                        {
        //                            Reply.IsDeleted = true;

        //                            var Likes = _context.Likes.Where(e => e.LikeTypeId == Reply.ReplyId || e.LikeTypeId == Comments.CommentId || e.LikeTypeId == Post.PostId && e.UnLike == false).ToList();

        //                            foreach (var like in Likes)
        //                            {
        //                                like.UnLike = true;
        //                            }
        //                        }
        //                    }

        //                }

        //            }
        //            _cache.RemoveData("Users");

        //            await _context.SaveChangesAsync();

        //            return "Delete Successfull";
        //        }
        //        else
        //        {
        //            return "UnAuthorised";
        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        _logger.LogError(ex.Message);
        //        return ex.Message;

        //    }

        //  }

        public async Task<string> DeleteUser(int userId, int id)
        {
            try
            {
                if (userId == id)
                {
                    var user = await _context.Users
                        .Include(u => u.Posts)
                        .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                        .Include(p => p.Posts)
                        .ThenInclude(u => u.Shares)
                        .SingleOrDefaultAsync(u => u.UserId == userId && u.IsDeleted == false);

                    if (user != null)
                    {
                        user.IsDeleted = true;

                        foreach (var post in user.Posts)
                        {
                            post.IsDeleted = true;

                            foreach (var comment in post.Comments)
                            {
                                comment.IsDeleted = true;

                                foreach (var reply in comment.Replies)
                                {
                                    reply.IsDeleted = true;
                                }
                            }
                            foreach (var shares in post.Shares)
                            {
                                shares.IsDeleted = true;
                            }

                        }


                        await _context.SaveChangesAsync();

                        return "User deleted successfully.";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return "UnAuthorised";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
        }


        /// <summary>
        /// This method using for the Update the user based on updateDTO parameter
        /// </summary>

        public async Task<String> UpdateById(UserUpdateDTO updateDTO, int Id)
        {
            try
            {
                if (updateDTO.UserId == Id)
                {

                    var users = _context.Users.FirstOrDefault(e => e.UserId == updateDTO.UserId && e.IsDeleted == false);


                    users.Username = updateDTO.Username;
                    users.Email = updateDTO.Email;
                    users.DateOfBirth = updateDTO.DateOfBirth;
                    users.Password = Encrypt.EncryptPassword(updateDTO.Password);

                    _cache.RemoveData("Users");
                    await _context.SaveChangesAsync();

                    return "update successfull";
                }
                else
                {
                    return "UnAuthorised";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;

            }

        }

        /// <summary>
        /// This method using for the Inserting the user based on user parameter
        /// </summary>

        public async Task<string> newUser(UserDTO user)
        {
            try
            {
                bool UserExits = _context.Users.Any(u => u.Username.Equals(user.Username) && u.Email.Equals(user.Email));

                if (UserExits == true)
                {
                    return "User Already Present";
                }
                else
                {
                    User users = new User()
                    {
                        Username = user.Username,
                        Email = user.Email,
                        DateOfBirth = user.DateOfBirth,
                        Password = Encrypt.EncryptPassword(user.Password),
                        DateUserJoined = DateTime.Now,
                        AccountType = user.AccountType

                    };
                    _context.Users.Add(users);
                    _cache.RemoveData("Users");
                    await _context.SaveChangesAsync();



                    _emailService.SendEmail(users.Email, "newUser", users.Username, null);

                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;

            }
            return "insertion Successfull";
        }

        public string AddProfilePicture(int UserId, IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                profilePicture.CopyTo(memoryStream);

                byte[] profilePictureBytes = memoryStream.ToArray();

                var user = _context.Users.FirstOrDefault(e => e.UserId == UserId);

                if (user != null)
                {
                    user.ProfilePicture = profilePictureBytes;
                    _context.SaveChanges();
                }
            }
            return "Profile pic Inserted Successfully";
        }

        public byte[] GetProfilePictureByUserId(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                return user.ProfilePicture;
            }
            else
            {
                return null;
            }

        }
    }
}



