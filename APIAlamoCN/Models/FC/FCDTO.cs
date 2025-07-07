using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIAlamoCN.Converters;

namespace APIAlamoCN.Models.FC
{
    public class FCDTO
    {
        [StringLength(40, ErrorMessage = "identificador no puede exceder los 40 caracteres.")]
        public string? identificador { get; set; }

        [StringLength(6, ErrorMessage = "comprobanteVentas no puede exceder los 6 caracteres.")]
        public string? comprobanteVentas { get; set; }

        [StringLength(6, ErrorMessage = "sucursal no puede exceder los 6 caracteres.")]
        public string? sucursal { get; set; }

        [StringLength(6, ErrorMessage = "numero no puede exceder los 6 caracteres.")]
        public string? numero { get; set; }

        public string? fechaDeMovimiento { get; set; } // DATE en SQL, no aplica StringLength

        [StringLength(20, ErrorMessage = "cliente no puede exceder los 20 caracteres.")]
        public string? cliente { get; set; }

        [StringLength(6, ErrorMessage = "vendedor no puede exceder los 6 caracteres.")]
        public string? vendedor { get; set; }

        [StringLength(6, ErrorMessage = "condicionDePago no puede exceder los 6 caracteres.")]
        public string? condicionDePago { get; set; }

        public string? observaciones { get; set; } // TEXT en SQL, sin longitud máxima definida

        [StringLength(20, ErrorMessage = "numeroDeCae no puede exceder los 20 caracteres.")]
        public string? numeroDeCae { get; set; }

        public string? vencimientoDeCae { get; set; } // DATE en SQL, no aplica StringLength

        [StringLength(1, ErrorMessage = "generadoPorRechazoDelComprobanteOriginal no puede exceder 1 carácter.")]
        public string? generadoPorRechazoDelComprobanteOriginal { get; set; }

        [StringLength(6, ErrorMessage = "tipoDeDatoOpcionalAfip no puede exceder los 6 caracteres.")]
        public string? tipoDeDatoOpcionalAfip { get; set; }

        [StringLength(60, ErrorMessage = "valorDatoOpcionalAfip no puede exceder los 60 caracteres.")]
        public string? valorDatoOpcionalAfip { get; set; }

        [StringLength(6, ErrorMessage = "comprobanteVentasAplica no puede exceder los 6 caracteres.")]
        public string? comprobanteVentasAplica { get; set; }

        [StringLength(6, ErrorMessage = "sucursalAplica no puede exceder los 6 caracteres.")]
        public string? sucursalAplica { get; set; }

        [JsonConverter(typeof(IntNullableConverter))]
        public int? numeroDeFormularioAplica { get; set; }

        public ICollection<ItemFCDTO> items { get; set; }
        public ICollection<ImpuestosFCDTO> impuestos { get; set; }
        public ICollection<PagosFCDTO> pagos { get; set; }
    }
}
