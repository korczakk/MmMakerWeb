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


    }
}