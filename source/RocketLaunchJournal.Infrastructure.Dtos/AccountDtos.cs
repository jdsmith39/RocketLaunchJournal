using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RocketLaunchJournal.Model.Constants;

namespace RocketLaunchJournal.Infrastructure.Dtos
{
    public class LoginDto
    {
        [Display(Name = "Email"), Required(ErrorMessage = ErrorMessages.RequiredField), EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string Password { get; set; }

        [Display(Name = "Remember My Email")]
        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordDto
    {
        [Display(Name = "Email"), Required(ErrorMessage = ErrorMessages.RequiredField), EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }

    public class TwoFactorCodeDto
    {
        [Display(Name = "Two Factor Code"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember Browser")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember My Email")]
        public bool RememberMe { get; set; }

        public string Email { get; set; }
        public int? AuthyUserId { get; set; }
        public int? UserId { get; set; }
    }

    public class TwoFactorSetupDto
    {
        [Display(Name = "Phone Number"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }
        public int? AuthyUserId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Display(Name = "Email"), Required(ErrorMessage = ErrorMessages.RequiredField), EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [StringLength(1000, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = Model.Constants.FieldSizes.PasswordMinimumLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ChangePasswordDto
    {
        [Display(Name = "Current Password"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [StringLength(1000, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = Model.Constants.FieldSizes.PasswordMinimumLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterDto : UserRoot
    {
        [Required(ErrorMessage = ErrorMessages.RequiredField)]
        [Display(Name = "Confirm Email")]
        [Compare(nameof(Email), ErrorMessage = "Email and email confirmation do not match.")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [StringLength(1000, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = Model.Constants.FieldSizes.PasswordMinimumLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password), Required(ErrorMessage = ErrorMessages.RequiredField)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public string RecaptchaToken { get; set; }
        public string CountryCode { get; set; }
        public int? AuthyUserId { get; set; }
    }

    public class ConfirmEmailDto
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }

    public class ImpersonateDto
    {
        public int? UserId { get; set; }
    }
}
