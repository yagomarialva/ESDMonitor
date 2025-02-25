using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("LINKSTATIONANDLINE")]
    public class LinkStationAndLineModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("ORDERSLIST")]
        [IgnoreDataMember]
        public int OrdersList { get; set; }
        [Column("LINEID")]
        public int LineID { get; set; }

        [Column("STATIONID")]
        public int StationID { get; set; }

        [Column("CREATED")]
        public DateTime? Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime? LastUpdated { get; set; }

        //Propriedades de navegação

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual StationModel? Station { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [IgnoreDataMember]
        public virtual LineModel? Line { get; set; }
    }
}
