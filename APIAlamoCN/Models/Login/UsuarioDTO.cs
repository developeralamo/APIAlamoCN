using System.ComponentModel.DataAnnotations;

namespace APIAlamoCN.Models.Login
{
    public class UsuarioDTO
    {
        [MaxLength(255)]
        public string usuario { get; set; }
        [MaxLength(120)]
        public string password { get; set; }
    }
}
