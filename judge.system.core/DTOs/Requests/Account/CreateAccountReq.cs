namespace judge.system.core.DTOs.Requests.Account
{
    public class CreateAccountReq
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
