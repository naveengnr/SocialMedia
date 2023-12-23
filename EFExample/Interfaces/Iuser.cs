using EFExample.DTO;
using EFExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFExample.Interfaces

{
    public interface Iuser
    {
        public Task<IQueryable<GetAllDTO>> GetAll();
        public Task<GetAllDTO> GetById(int UserId);
        public Task<String> newUser(UserDTO userDTO);
        public Task<GetAllDTO> GetByName(String Username);
        public Task<string> DeleteUser(int UserId , int Id);

        //public List<GetAllDTO> GetAllUsers();
        public Task<String> UpdateById(UserUpdateDTO updateDTO , int Id);
        public String AddProfilePicture(int UserId, IFormFile profilePicture);
        public byte[] GetProfilePictureByUserId(int userId);

    }
}
