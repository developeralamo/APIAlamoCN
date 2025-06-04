using APIAlamoCN.Models.Response;

namespace APIAlamoCN.Models.Response.Pdf
{
    public class PdfResponse : BaseResponse<PdfDTO>
    {
        public PdfResponse(PdfDTO response) : base(response)
        {

        }
    }
}
