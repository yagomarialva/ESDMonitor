using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("PRODUCEACTIVITY")]
    public class ProduceActivityModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O UserId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("USERID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O JigId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("JIGID")]
        public int JigId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O MonitorEsdId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("MONITORESDID")]
        public int MonitorEsdId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O LinkStationLineId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("LINKSTATIONANDLINEID")]
        public int LinkStationAndLineID { get; set; }

        [Column("ISLOCKED")]
        public int IsLocked { get; set; }

        [StringLength(250, ErrorMessage = "O Description deve ter no máximo 250 caracteres")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ]+$", ErrorMessage = "O Name deve conter apenas letras, números, underscores (_), hífens (-), espaços e caracteres especiais do português (acentos e cedilha), e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }

        //Propriedades de navegação

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual UserModel? User { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual JigModel? Jig { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual MonitorEsdModel? MonitorEsd { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual LinkStationAndLineModel? LinkStationAndLine { get; set; }


    }
}
