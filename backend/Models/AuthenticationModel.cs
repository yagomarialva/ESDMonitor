using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("AUTHENTICATION")]
    public class AuthenticationModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(16, ErrorMessage = "O Username deve ter no máximo 16 caracteres")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9_\\- ]+$", ErrorMessage = "O Username deve conter apenas letras, números, underscores (_), hífens (-) e espaços, e não pode ser vazio ou conter apenas espaços em branco")]

        [Column("USERNAME")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]

        [Column("ROLESNAME")]
        public required string RolesName { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^[a-zA-Z0-9]{1,255}$", ErrorMessage = "O Badge deve conter apenas letras e números.")]

        [Column("BADGE")]
        public required string Badge { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^[A-Za-z\\d@$!%*?&#]{6,12}$",
        ErrorMessage = "A senha deve ter entre 6 e 12 caracteres, sem espaços.")]

        [Column("PASSWORD")]
        public required string Password { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime LastUpdated { get; set; }



    }
}
