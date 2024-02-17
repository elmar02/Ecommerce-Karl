using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
