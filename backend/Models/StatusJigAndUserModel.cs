using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("STATUSJIGANDUSER")]
    public class StatusJigAndUserModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("MonitorEsd")]
        public int MonitorEsdId { get; set; }

        [Column("User")]
        public int? UserId { get; set; }

        [Column("Jig")]
        public int? JigId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("MessageType")]
        public string? MessageType { get; set; }

        [Required]
        [Column("LogMonitorEsd")]
        public int? Status { get; set; }

        [ForeignKey("LogMonitorEsd")]
        public int LastLogId { get; set; }

        //[Required]
        //[Column("Created")]
        //public DateTime Created { get; set; }

        [Required]
        [Column("LastUpdated")]
        public DateTime LastUpdated { get; set; }

        // Navigation properties

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual MonitorEsdModel? MonitorEsd { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual UserModel? User { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual JigModel? Jig { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual LogMonitorEsdModel? LogMonitorEsd { get; set; }
    }
}
