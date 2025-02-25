using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("RECORDSTATUSPRODUCE")]
    public class RecordStatusProduceModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O ProduceActivityId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("PRODUCEACTIVITYID")]
        public int ProduceActivityId { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O UserId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("USERID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(250, ErrorMessage = "O Description deve ter no máximo 250 caracteres")]
        [RegularExpression("^[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/]+$", ErrorMessage = "O campo deve conter apenas letras, números, underscores (_), hífens (-), barras (/), " +
            "espaços e caracteres especiais do português (acentos e cedilha), e não pode ser vazio.")]
        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("DATEEVENT")]
        public DateTime? DateEvent { get; set; }

        //Propriedades de navegação

        [IgnoreDataMember]
        public virtual ProduceActivityModel? ProduceActivity { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual UserModel? User { get; set; }
    }
}
