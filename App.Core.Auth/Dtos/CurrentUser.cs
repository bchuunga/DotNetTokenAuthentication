namespace App.Core.Auth.Dtos
{
    public class CurrentUser : UserDto
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Roles { get; set; }
    }
}
