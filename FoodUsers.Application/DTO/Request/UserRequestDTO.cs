namespace FoodUsers.Application.DTO.Request
{
    public class UserRequestDTO
    {
        public long Id { get; set; }
        public int DNI { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
