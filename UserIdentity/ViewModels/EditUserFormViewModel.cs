using System.ComponentModel.DataAnnotations;

public class EditUserFormViewModel {
    

    public string UserId{get;set;}
    
    [Required]
    [Display(Name = "First Name ")]
    public string FirstName{get;set;}
    [Required]
    [Display(Name = "Last Name ")]
    public string LastName{get;set;}
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "UserName")]
    public string UserName{get;set;}
}