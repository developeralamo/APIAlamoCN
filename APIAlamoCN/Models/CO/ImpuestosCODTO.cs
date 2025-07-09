using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIAlamoCN.Converters;

namespace APIAlamoCN.Models.CO
{
    public class ImpuestosCODTO
    {
        [StringLength(40, ErrorMessage = "El campo 'identificador' no puede exceder los 40 caracteres.")]
        public string? identificador { get; set; }

        [JsonConverter(typeof(IntNullableConverter))]
        public int? numeroitem { get; set; }

        [StringLength(1, ErrorMessage = "El campo 'tipoDeConcepto' no puede exceder 1 carácter.")]
        public string? tipoDeConcepto { get; set; }

        [StringLength(6, ErrorMessage = "El campo 'concepto' no puede exceder los 6 caracteres.")]
        public string? concepto { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? importeIngresado { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? importeGravado { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? tasa { get; set; }
    }
}
