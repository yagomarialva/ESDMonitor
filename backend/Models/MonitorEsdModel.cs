using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("MONITORESD")]
    public class MonitorEsdModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O SerialNumber deve ter no máximo 100 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "O campo deve conter apenas letras e números, sem espaços ou caracteres especiais.")]
        [Column("SERIALNUMBER")]
        public string? SerialNumberEsp { get; set; }

        [StringLength(250, ErrorMessage = "O Description deve ter no máximo 250 caracteres")]
        [RegularExpression("^[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/]+$", ErrorMessage = "O campo deve conter apenas letras, números, underscores (_), hífens (-), barras (/), " +
            "espaços e caracteres especiais do português (acentos e cedilha), e não pode ser vazio.")]
        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }
    }
}
