namespace StudentsWebsite.Service
{
    public abstract class UserInfo
    {
        public string Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
