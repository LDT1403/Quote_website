namespace Quote.Modal
{
    public class RegisterModal
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
    }
    public class UserInfoModal
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Images { get; set; }

        public string Position { get; set; }
        //public DateTime? Date { get; set; }
    }
}
