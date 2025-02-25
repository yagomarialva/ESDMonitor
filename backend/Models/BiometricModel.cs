using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace BiometricFaceApi.Models
{
    public class BiometricModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O Name deve ter no máximo 50 caracteres")]
        [RegularExpression("^(?!\\s*$)[a-zA-Z0-9_\\-\\s]+$", ErrorMessage = "O UserId deve conter apenas letras, números, underscores (_), hífens (-) e espaços, e não pode ser vazio ou conter apenas espaços em branco")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression("^[a-zA-Z0-9]{1,255}$", ErrorMessage = "O Badge deve conter apenas letras e números.")]
        public string? Badge { get; set; }

       
        [SwaggerIgnore]
        public string? Embedding { get; set; }


        [NotMapped,JsonIgnore]
        public IFormFile? Image { get; set; }

        [SwaggerSchema(ReadOnly = true, Description = "This field is ignored in the Swagger documentation")]
        public byte[]? Stream { get; set; }
    }

}
