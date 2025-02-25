using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BiometricFaceApi.Models
{
    [Table("IMAGES")]
    public class ImageModel
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("USERID")]
        public int UserId { get; set; }
       

        [Column("PICTURESTREAM")]
        public byte[]? PictureStream { get; set; }

        [Column ("EMBEDDING")]
        public string? Embedding {  get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }
        [Column("LASTUPDATED")]
        public DateTime LastUpdated { get; set; }

        [IgnoreDataMember, NotMapped]
        public IFormFile ImageFile
        {
            get
            {
                return this.ObservableImageFile;
            }
            set
            {
                this.ObservableImageFile = value;
                if (this.ObservableImageFile is not null && this.ObservableImageFile.Length > 0)
                {
                    using (var filestream = new MemoryStream())
                    {
                        this.ImageFile.OpenReadStream().CopyTo(filestream);
                        this.PictureStream = filestream.ToArray();

                    }
                }
            }
        }
        private IFormFile? ObservableImageFile { get; set; }
        //converte imagem pra string
        public string GetPictureToBase()
        {
            try
            {
                return Convert.ToBase64String(PictureStream);
            }
            catch
            {
                return string.Empty;
            }
        }

        //Propriedades de navegação
        public virtual UserModel? User { get; set; }
    }
}

