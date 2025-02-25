using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("STATIONVIEW")]
    public class StationViewModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("MONITORESDID")]
        public int MonitorEsdId { get; set; }

        [Column("LINKSTATIONANDLINEID")]
        public int LinkStationAndLineId { get; set; }

        [Column("POSITIONSEQUENCE")]
        public int PositionSequence { get; set; }
        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }

        //Propriedades de navegação

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual MonitorEsdModel? MonitorEsd { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual LinkStationAndLineModel? LinkStationAndLine { get; set; }

    }
}
