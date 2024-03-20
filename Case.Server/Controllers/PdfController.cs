using Microsoft.AspNetCore.Mvc;


namespace Case.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfController : ControllerBase
{
    [HttpGet]
    public FileContentResult Get()
    {
        ChromePdfRenderer renderer = new ChromePdfRenderer();
        PdfDocument pdf = renderer.RenderHtmlAsPdf("<h1>Hello World<h1>");

        return File(pdf.BinaryData, "application/pdf", "Wiki.Pdf");
    }

}
