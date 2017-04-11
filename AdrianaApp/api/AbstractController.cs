using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AdrianaApp.api
{
    [Authorize(Roles = "Administrator")]
    public class AbstractController : ApiController
    {
        // GET: api/Abstract
        public IEnumerable<Models.Views.AbstractView> Get()
        {
            using (var db = new OcphDbContext())
            {
                var result = from a in db.Abstraks.Select()
                             join b in db.Mahasiswa.Select()
                             on a.IdMahasiswa equals b.Id
                             select new Models.Views.AbstractView
                             {
                                 Abstraksi = a.Abstraksi,
                                 Pembimbing1 =a.Pembimbing1,
                                 Pembimbing2 =a.Pembimbing2,
                                 FileExtention = a.FileExtention,
                                 FileName = a.FileName,
                                 FileTipe = a.FileTipe,
                                 Id = a.Id,
                                 IdMahasiswa = a.IdMahasiswa,
                                 Judul = a.Judul,
                                 Alamat=b.Alamat, Jurusan=b.Jurusan, MahasiswaId=b.Id
                                , Nama=b.Nama, NPM=b.NPM,  
                             };
                return result.ToList();
            }
        }

        // GET: api/Abstract/5
      
        // POST: api/Abstract
        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.abstrak>();
            using (var db = new OcphDbContext())
            {
                var id = db.Abstraks.InsertAndGetLastID(item);
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
            var item = await Request.Content.ReadAsAsync<Models.Data.abstrak>();
            using (var db = new OcphDbContext())
            {
                var isUpdate = db.Abstraks.Update(O => new { O.Judul,O.Pembimbing1,O.Pembimbing2 }, item, O => O.Id == item.Id);
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
                var trans = db.Connection.BeginTransaction();
                try
                {
                    var abst = db.Abstraks.Where(O => O.Id == id).FirstOrDefault();
                    if (abst != null)
                    {
                        
                        string f = HttpContext.Current.Server.MapPath("~/uploads") + "//" + abst.FileName;
                        if (System.IO.File.Exists(f))
                        {
                            System.IO.File.Delete(f);
                        }
                    }

                    if(db.Abstraks.Delete(O => O.Id == id))
                    {
                        trans.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Dihapus");
                    }else
                    {
                        throw new System.Exception("Data Gagal Dihapus");
                    }


                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }


        }
    }
}
