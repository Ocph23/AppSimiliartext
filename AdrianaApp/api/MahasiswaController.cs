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
    [Authorize(Roles ="Administrator")]
    public class MahasiswaController : ApiController
    {
        // GET: api/Mahasiswa
        public IEnumerable<Models.Data.mahasiswa> Get()
        {
            using (var db = new OcphDbContext())
            {
                return db.Mahasiswa.Select();
            }
        }

        // GET: api/Mahasiswa/5
     
        // POST: api/Mahasiswa
        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.mahasiswa>();
            using (var db = new OcphDbContext())
            {
                var id = db.Mahasiswa.InsertAndGetLastID(item);
                if (id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Gagal Ditambahkan");
            }
        }
        // PUT: api/Mahasiswa/5
        public async Task<HttpResponseMessage> Put()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.mahasiswa>();
            using (var db = new OcphDbContext())
            {
                var isUpdate = db.Mahasiswa.Update(O=>new { O.Alamat,O.Jurusan,O.Nama,O.NPM},item,O=>O.Id==item.Id);
                if (isUpdate)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Diperbaharui");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Gagal Diubah");
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
                    var mahasiswa = db.Mahasiswa.Where(O => O.Id == id).FirstOrDefault();
                    var abst = db.Abstraks.Where(O => O.IdMahasiswa == mahasiswa.Id).FirstOrDefault();
                 
                    bool isDeletedAbstract = true;
                    var fileIsDeleted = true;
                    if(abst!=null)
                    {
                        isDeletedAbstract = db.Abstraks.Delete(O => O.IdMahasiswa == mahasiswa.Id);
                        string f = HttpContext.Current.Server.MapPath("~/uploads") +"//"+ abst.FileName;
                        if(System.IO.File.Exists(f))
                        {
                            System.IO.File.Delete(f);
                        }
                    }
                    var isDeleted = db.Mahasiswa.Delete(O => O.Id == id);
                    if (isDeletedAbstract && isDeleted && fileIsDeleted)
                    {
                        trans.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Dihapus");

                    }
                    else
                    {
                        throw new SystemException("Data Gagal Dihapus");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new SystemException(ex.Message);
                }
             
            }


        }
    }
}
