namespace FastDrive.Models
{
    public class User
    {
        public int IDUsuario { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Surname {  get; set; }
        public int DNI { get; set; }
        public EUserType UserType { get; set; }

    }

    public enum EUserType
    {
        Customer,
        Operator,
        Admin
    }
}
