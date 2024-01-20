using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    [Index("Email", IsUnique = true)]
    public class ResetPasswordDto
    {

        [MaxLength(100)]
        public string Email { get; set; } = "";

        public string newPassword { get; set; } = "";

        [MaxLength(300)]
        public string Token { get; set; } = "";


    }
}
