using EFExample.Models;
using Microsoft.AspNetCore.Mvc;
using EFExample.Secutiy;
using EFExample.DTO;
using Microsoft.EntityFrameworkCore;
using EFExample.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EFExample.Service
{

    public class UserService : Iuser
    {
        public readonly SocialMediaContext _context;
      
        public UserService(SocialMediaContext context)
        {
            _context = context;
        }


        //public List<GetAllDTO> GetAllUsers()
        //{
        //    var users = _context.Users.Select(u => new GetAllDTO

        //    {
        //        UserId = u.UserId,
        //        Username = u.Username,
        //        Email = u.Email,
        //        DateOfBirth = u.DateOfBirth,
        //        DateUserJoined = u.DateUserJoined
        //    }).ToList();

        //    return users;
        //}

        public IQueryable<GetAllDTO> GetAll()
        {
            var users = _context.Users.Select( u => new GetAllDTO
    
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    DateOfBirth = u.DateOfBirth,
                    DateUserJoined = u.DateUserJoined
            });

            return users;
        }

        public GetAllDTO GetById(int UserId)
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

        public GetAllDTO GetByName(String Username)
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

            public string DeleteUser(int UserId)
        {
            var user = _context.Users.FirstOrDefault(e => e.UserId == UserId && e.IsDeleted == false);

            //_context.Users.Remove(user);
            user.IsDeleted = true;
            _context.SaveChanges();

            return "Delete Successfull";
        }

        public String UpdateById(UserUpdateDTO updateDTO)
        {
            var users = _context.Users.FirstOrDefault(e => e.UserId == updateDTO.UserId && e.IsDeleted == false);


            users.Username = updateDTO.Username;
            users.Email = updateDTO.Email;
            users.DateOfBirth = updateDTO.DateOfBirth;
            users.Password = Encrypt.EncryptPassword(updateDTO.Password);
            
            _context.SaveChanges();

            return "update successfull";
        }

       
        public string newUser(UserDTO user)
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
                        DateUserJoined = DateTime.Now
                    };
                    _context.Users.Add(users);
                    _context.SaveChanges();

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "insertion Successfull";
        }
    }

    }
