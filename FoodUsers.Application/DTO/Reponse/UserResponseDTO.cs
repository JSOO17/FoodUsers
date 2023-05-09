namespace FoodUsers.Application.DTO.Response
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int Cellphone { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }
    }
}