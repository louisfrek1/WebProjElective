namespace WebProjElective.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string StAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public int PNumber { get; set; }  // Add this property
        public string Gender { get; set; }
        public DateTime BDate { get; set; }
        public string AcctType { get; set; }

    }
}
