using EFExample.DTO;
using EFExample.Models;
namespace EFExample.Interfaces

{
    public interface Iuser
    {
        public IQueryable<GetAllDTO> GetAll();
        public GetAllDTO GetById(int UserId);
        public String newUser(UserDTO userDTO);
        public GetAllDTO GetByName(String Username);
        public string DeleteUser(int UserId);

        //public List<GetAllDTO> GetAllUsers();
        public String UpdateById(UserUpdateDTO updateDTO);

    }
}
