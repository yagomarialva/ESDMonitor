using System.ComponentModel.DataAnnotations.Schema;

namespace BiometricFaceApi.Models
{
    [Table("FC_EMBEDDING")]
    public class FcEmbeddingModel
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("USERID")]
        public int UserId { get; set; }
        public virtual UserModel? User { get; set; }
        [Column("EMBEDDINGVALUE")]
        public string? EmbeddingValue { get; set; }
    }
}
