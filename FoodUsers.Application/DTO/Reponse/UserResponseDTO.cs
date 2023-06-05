namespace FoodUsers.Application.DTO.Response
{
    public class UserResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int DNI { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}