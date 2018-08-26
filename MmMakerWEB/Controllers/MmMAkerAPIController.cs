using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MmMaker.Model;
using MmMakerExcelManager;


namespace MmMakerWEB.Controllers
{
    [Route("api/mmmakerapi/{action}")]
    public class MmMAkerAPIController : ApiController
    {
        [HttpPost]
        public List<ExcelContent> Transformfile()
        {
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Plik jest wymagany."));
            }

            Stream filename = HttpContext.Current.Request.Files[0].InputStream;

            ExcelParser mmMaker = new ExcelParser(filename);
            List<ExcelContent> parsedContent = mmMaker.ParseExcel();

            return parsedContent;
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(List<ExcelContent> content)
        {
            if (content == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Dane do zasilenia pliku są wymagane."));
            }

            MemoryStream stream = new MemoryStream();
            MmMakerExporter exporter = new MmMakerExporter(stream);

            bool status = exporter.ExportToExcel(content);

            if (status == false)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Nie udało się wygenerować pliku XLS"));
            }

            stream.Position = 0;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(stream.ToArray());
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "Scalone.xls";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            
            return response;

        }
    }
}