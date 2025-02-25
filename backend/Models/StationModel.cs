using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("STATION")]
    public class StationModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O Username deve ter no máximo 50 caracteres")]
        [RegularExpression("^[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/]+$", ErrorMessage = "O campo deve conter apenas letras, números, underscores (_), hífens (-), barras (/), " +
            "espaços e caracteres especiais do português (acentos e cedilha), e não pode ser vazio.")]
        [Column("NAME")]
        public string? Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O dimensional minimo valido é 1")]
        [Column("SIZEX")]
        public int SizeX { get; set; }
        [Range(1,int.MaxValue, ErrorMessage = "O dimensional minimo valido é 1")]
        [Column("SIZEY")]
        public int SizeY { get; set; }
        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }

    }
}
