using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("FC_EYE")]
    public class FcEyeModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("USERID")]
        public int UserId { get; set; }
        public virtual UserModel? User { get; set; }
        [Column("LEFTEYE")]
        public string? LeftEye { get; set; }
        [Column("RIGHTEYE")]
        public string? RightEye { get; set; }
    }
}
