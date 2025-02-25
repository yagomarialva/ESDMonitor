using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("LOGMONITORESD")]
    public class LogMonitorEsdModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O tipo de mensagem deve ter no máximo 100 caracteres")]
        [RegularExpression("^(?=.*\\S)[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/]+$", ErrorMessage = "O campo deve conter pelo menos um caractere válido e não pode ser vazio ou apenas espaços.")]
        [Column("MESSAGETYPE")]
        public string? MessageType { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O SerialNumber deve ter no máximo 100 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "O campo deve conter apenas letras e números, sem espaços ou caracteres especiais.")]
        [Column("SERIALNUMBER")]
        public string? SerialNumberEsp { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O MonitorEsdId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("MONITORESDID")]
        public int MonitorEsdId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual MonitorEsdModel? MonitorEsd { get; set; }

        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9]+$", ErrorMessage = "O MonitorEsdId deve conter apenas letras e números e não pode ser vazio ou conter apenas espaços em branco")]
        [Column("JIGID")]
        public int JigId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual JigModel? Jig { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "O IP deve ser um endereço IPv4 válido.")]
        [Column("IP")]
        public string? IP { get; set; }

        [RegularExpression("^[01]$", ErrorMessage = "O Valor deve ser 0 ou 1")]
        [Column("Status")]
        public int? Status { get; set; }

        [StringLength(20, ErrorMessage = "O conteúdo da mensagem deve ter no máximo 20 caracteres")]
        [RegularExpression("^(?=.*\\S)[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/.,…]+$", ErrorMessage = "O campo deve conter pelo menos um caractere válido e não pode ser vazio ou apenas espaços.")]

        [Column("MESSAGECONTENT")]
        public string? MessageContent { get; set; }

        [StringLength(250, ErrorMessage = "O Description deve ter no máximo 250 caracteres")]
        [RegularExpression("^[a-zA-Z0-9_\\-\\sáéíóúãõâêîôûçÁÉÍÓÚÃÕÂÊÎÔÛÇ/]+$", ErrorMessage = "O campo deve conter apenas letras, números, underscores (_), hífens (-), barras (/), " +
            "espaços e caracteres especiais do português (acentos e cedilha), e não pode ser vazio.")]
        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime LastUpdated { get; set; }

    }
}
