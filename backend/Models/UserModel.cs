using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("USERS")]
    public class UserModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O Username deve ter no máximo 50 caracteres")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9_\\-\\s]+$", ErrorMessage = "O Name deve conter apenas letras, números, underscores (_), hífens (-) e espaços, e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("NAME")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^[a-zA-Z0-9]{1,255}$", ErrorMessage = "O Badge deve conter apenas letras e números.")]
        [Column("BADGE")]
        public string? Badge { get; set; }
        [Column("CREATED")]
        public DateTime Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime LastUpdated { get; set; }


    }
}
