using Microsoft.AspNetCore.Identity;

public class ApiUser : IdentityUser
{ 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set;}

}