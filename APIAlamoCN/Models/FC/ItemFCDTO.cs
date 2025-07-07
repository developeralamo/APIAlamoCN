using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIAlamoCN.Converters;

namespace APIAlamoCN.Models.FC
{
    public class ItemFCDTO
    {
        [StringLength(40, ErrorMessage = "El campo 'identificador' no puede exceder los 40 caracteres.")]
        public string? identificador { get; set; }

        [JsonConverter(typeof(IntNullableConverter))]
        public int? numeroitem { get; set; }

        [StringLength(6, ErrorMessage = "El campo 'tipoDeProducto' no puede exceder los 6 caracteres.")]
        public string? tipoDeProducto { get; set; }

        [StringLength(30, ErrorMessage = "El campo 'producto' no puede exceder los 30 caracteres.")]
        public string? producto { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? cantidad { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? precio { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? bonificacion1 { get; set; }

        // No se puede usar [StringLength] en campos tipo TEXT; validar manualmente si hace falta
        public string? observaciones { get; set; }
    }
}
