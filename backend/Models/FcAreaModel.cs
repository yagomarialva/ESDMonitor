using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("FC_AREA")]
    public class FcAreaModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("USERID")]
        public int UserId { get; set; }
        public virtual UserModel? User { get; set; }
        [Column("FACECONFIDENCE")]
        public string? FaceConfidence { get; set; }
        [Column("H")]
        public string? H { get; set; }
        [Column("W")]
        public string? W { get; set; }
        [Column("X")]
        public string? X { get; set; }
        [Column("Y")]
        public string? Y { get; set; }
    }
}
