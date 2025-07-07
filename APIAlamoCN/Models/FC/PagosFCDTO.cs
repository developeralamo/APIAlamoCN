using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIAlamoCN.Converters;

namespace APIAlamoCN.Models.FC
{
    public class PagosFCDTO
    {
        [StringLength(40, ErrorMessage = "El campo 'identificador' no puede exceder los 40 caracteres.")]
        public string? identificador { get; set; }

        [JsonConverter(typeof(IntNullableConverter))]
        public int? numeroitem { get; set; }

        [StringLength(1, ErrorMessage = "El campo 'tipoDeConcepto' no puede exceder 1 carácter.")]
        public string? tipoDeConcepto { get; set; }

        [StringLength(6, ErrorMessage = "El campo 'codigoDeConcepto' no puede exceder los 6 caracteres.")]
        public string? codigoDeConcepto { get; set; }

        [StringLength(60, ErrorMessage = "El campo 'chequeDocumento' no puede exceder los 60 caracteres.")]
        public string? chequeDocumento { get; set; }

        [JsonConverter(typeof(DecimalNullableConverter))]
        public decimal? importe { get; set; }
    }
}
