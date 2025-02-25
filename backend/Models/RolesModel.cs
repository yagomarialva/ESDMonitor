using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("ROLES")]
    public class RolesModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Required]
        [Column("ROLESNAME")]
        public string? RolesName { get; set; }
        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }
    }
}
