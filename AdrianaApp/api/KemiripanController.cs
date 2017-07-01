using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace AdrianaApp.api
{
    public class KemiripanController : ApiController
    {
        // GET: api/Kemiripan
       [HttpGet]
        public async Task<HttpResponseMessage> View(int Id)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (var db = new OcphDbContext())
                {
                    var result = db.Abstraks.Where(O => O.Id == Id).FirstOrDefault();

                    var path = HttpContext.Current.Server.MapPath("~/uploads/")+result.FileName;

                    if (result.FileTipe=="application/pdf")
                    {
                      sb.Append(Convert.ToBase64String(Helper.GetPdfText(path)));
                    }else if(result.FileTipe=="text/plain")
                    {
                        string txt = HttpContext.Current.Server.HtmlEncode(Helper.ExtractPlainText(path));
                        sb.Append(txt);
                    } else
                    {
                       
                      sb.Append(Helper.GetHtml(path).ToString());
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, sb.ToString());
                }
               
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // POST: api/Kemiripan
        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.kemiripan>();
            using (var db = new OcphDbContext())
            {
                var id = db.Kemiripan.InsertAndGetLastID(item);
                if (id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Gagal Ditambahkan");
            }
        }

        // PUT: api/Abstract/5
        public async Task<HttpResponseMessage> Put()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.kemiripan>();
            using (var db = new OcphDbContext())
            {
                var isUpdate = db.Kemiripan.Update(O => new { O.IdJudul, O.IdKemiripan, O.Prosentase}, item, O => O.Id == item.Id);
                if (isUpdate)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Diperbaharui");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Gagal Ditambahkan");
            }
        }

        // DELETE: api/Mahasiswa/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            using (var db = new OcphDbContext())
            {
                var isDeleted = db.Kemiripan.Delete(O => O.Id == id);
                if (isDeleted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Dihapus");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Gagal Dihapus");
            }


        }
    }
}
