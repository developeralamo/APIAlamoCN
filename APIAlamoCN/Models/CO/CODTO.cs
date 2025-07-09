using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIAlamoCN.Converters;

namespace APIAlamoCN.Models.CO
{
    public class CODTO
    {
        [StringLength(40, ErrorMessage = "identificador no puede exceder los 40 caracteres.")]
        public string? identificador { get; set; }

        [StringLength(6, ErrorMessage = "ComprobanteProveedores no puede exceder los 6 caracteres.")]
        public string? comprobanteProveedores { get; set; }

        [StringLength(4, ErrorMessage = "CircuitoOrigen no puede exceder los 4 caracteres.")]
        public string? circuitoOrigen { get; set; }

        [StringLength(4, ErrorMessage = "CircuitoAplicacion no puede exceder los 4 caracteres.")]
        public string? circuitoAplicacion { get; set; }

        public string? fechaDeMovimiento { get; set; }

        public string? fechaDeEmision { get; set; }

        public string? observaciones { get; set; }

        public string? comprobanteOriginal { get; set; }

        public ICollection<ItemCODTO> items { get; set; }
        public ICollection<ImpuestosCODTO> impuestos { get; set; }
    }
}
