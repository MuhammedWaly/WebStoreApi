using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebStoreApi.Models.DTOS
{
    public class UpdatePasswordDto
    {
        
        public string OldPassword { get; set; }

        [Required, MaxLength(70), MinLength(8)]
        public string NewPassword { get; set; }
    }
}
