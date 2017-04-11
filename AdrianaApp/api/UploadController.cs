using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Runtime.InteropServices.ComTypes;
using AdrianaApp.Models;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Xml.Linq;
using AdrianaApp.Models.Data;
using DocumentFormat.OpenXml.Drawing;

namespace AdrianaApp.api
{
    public class UploadController : ApiController
    {
        // GET: api/Upload
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            string uploadPath = HttpContext.Current.Server.MapPath("~/uploads");
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                "This request is not properly formatted"));

            using (var db = new OcphDbContext())
            {
                var trans = db.Connection.BeginTransaction();
                try
                {
                    var provider = new MyStreamProvider(uploadPath);
                    await Request.Content.ReadAsMultipartAsync(provider);
                    var a = new Models.Data.abstrak();
                    var mah = new Models.Data.mahasiswa();
                    FileInfo fi = null;
                    foreach (var file in provider.FileData)
                    {
                        var f = file.Headers.ContentDisposition.FileName;
                        a.FileName = f.Substring(1, f.Length - 2);

                        a.FileTipe = file.Headers.ContentType.MediaType;
                        a.FileExtention = a.FileName.Split('.')[1];
                        fi = new FileInfo(file.LocalFileName);
                    }

                    foreach (HttpContent ctnt in provider.Contents)
                    {
                        var name = ctnt.Headers.ContentDisposition.Name;
                        var field = name.Substring(1, name.Length - 2);
                        if (field == "Judul")
                        {
                            a.Judul = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "MahasiswaId")
                        {
                            a.IdMahasiswa = Convert.ToInt32(await ctnt.ReadAsStringAsync());
                        }
                        else if (field == "Pembimbing1")
                        {
                            a.Pembimbing1 = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "Pembimbing2")
                        {
                            a.Pembimbing2 = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "NPM")
                        {
                            mah.NPM = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "Nama")
                        {
                            mah.Nama = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "Jurusan")
                        {
                            mah.Jurusan = await ctnt.ReadAsStringAsync();
                        }
                        else if (field == "Alamat")
                        {
                            mah.Alamat = await ctnt.ReadAsStringAsync();
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    using (WordprocessingDocument pac = WordprocessingDocument.Open(uploadPath + "\\" + a.FileName, true))
                    {
                        string txt = string.Empty;
                        OpenXmlElement element = pac.MainDocumentPart.Document.Body;
                        if (element != null)
                        {
                            sb.Append(Helper.GetPlainText(element));
                        }
                    }
                    a.Abstraksi = sb.ToString();

                   var id= db.Mahasiswa.InsertAndGetLastID(mah);
                   
                    if (id > 0)
                    {
                        a.IdMahasiswa = id;
                        a.Id = db.Abstraks.InsertAndGetLastID(a);
                        trans.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, a.Id);
                    }
                    else
                    {

                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data tidak dapat ditambahkan");
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message);
                }
            }



        }



        [HttpPost]
        public async Task<HttpResponseMessage> Proccess()
        {
            string uploadPath = HttpContext.Current.Server.MapPath("~/uploads");

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                "This request is not properly formatted"));

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var a = new Models.Data.abstrak();
            var setting = new Models.Views.AbstractSetting();

            foreach (HttpContent ctnt in provider.Contents)
            {
                var name = ctnt.Headers.ContentDisposition.Name;
                var field = name.Substring(1, name.Length - 2);
              
                if (field == "file")
                {
                    var f = ctnt.Headers.ContentDisposition.FileName;
                    a.FileName= f.Substring(1, f.Length - 2);

                 //   a.FileTipe = file.Headers.ContentType.MediaType;
                    a.FileExtention = a.FileName.Split('.')[1];
                    //now read individual part into STREAM
                    var stream = await ctnt.ReadAsStreamAsync();

                    byte[] data = new byte[stream.Length];


                    if (stream.Length != 0)
                    {
                        await stream.ReadAsync(data, 0, (int)stream.Length);
                        a.data = data;
                    }
                }
              else if (field == "Judul")
                {
                    a.Judul = await ctnt.ReadAsStringAsync();
                }
                else if (field == "TandaBaca")
                {
                    var x= await ctnt.ReadAsStringAsync();
                    setting.TandaBaca = Convert.ToBoolean(x);
                }
                else if (field == "Angka")
                {
                    var x = await ctnt.ReadAsStringAsync();
                    setting.Angka = Convert.ToBoolean(x);
                }
                else if (field == "HurufBesar")
                {
                    var x = await ctnt.ReadAsStringAsync();
                    setting.HurufBesar = Convert.ToBoolean(x);
                }

            }


            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();




            using (var db = new OcphDbContext())
            {
                try
                {   // Open the text file using a stream reader.

                    var stream = new MemoryStream(a.data);
                    using (WordprocessingDocument pac = WordprocessingDocument.Open(stream, true))
                    {
                        string txt = string.Empty;
                        OpenXmlElement element = pac.MainDocumentPart.Document.Body;
                        if (element != null)
                        {
                            sb.Append(Helper.GetPlainText(element));
                        }
                    


                    a.Abstraksi = sb.ToString();
                    
                    Plagiat plagiat = new Plagiat(a,setting);
                  
                        var pars = element.Elements<Paragraph>();
                        DocumentFormat.OpenXml.Wordprocessing.Color color = new DocumentFormat.OpenXml.Wordprocessing.Color();
                        DocumentFormat.OpenXml.Wordprocessing.Highlight hil = new DocumentFormat.OpenXml.Wordprocessing.Highlight();
                        foreach (var para in pars)
                        {
                            foreach(var run in para.Elements<Run>())
                            {
                                foreach(var text in run.Elements<Text>())
                                {
                                    if(text.Text.Contains("eluan"))
                                    {
                                        color.Val = "365F91";
                                        run.Append(color);
                                        pac.MainDocumentPart.Document.Save();

                                    }
                                }
                            }
                        }
                       

                        HtmlConverterSettings settings = Helper.GetHtmlConverterSettings(a.FileName);
                        XElement html = HtmlConverter.ConvertToHtml(pac, settings);

                        // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                        // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                        // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                        // for detailed explanation.
                        //
                        // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                        // must do it correctly, or entities will not be serialized properly.

                        sb1.Append(html.ToString(SaveOptions.DisableFormatting));
                        AbstractResultView res = new AbstractResultView();
                        res.Data = plagiat.Result.Where(O => O.ProsentaseAbstrak > 50 || O.ProsentaseJudul > 50);
                        res.StrHTML = sb1.ToString();

                        return Request.CreateResponse(HttpStatusCode.OK, res);
                    }

                 
                }
                catch (Exception e)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data tidak dapat ditambahkan");
                }
            }






        }
     
    }

    public class AbstractResultView
    {
        public IEnumerable<abstrak> Data { get; internal set; }
        public string StrHTML { get; internal set; }
    }


    public class MyStreamProvider : MultipartFormDataStreamProvider
    {
        public MyStreamProvider(string uploadPath)
            : base(uploadPath)
        {

        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString() + ".data";
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}
