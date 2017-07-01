using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AdrianaApp.api
{
    public class DosenController : ApiController
    {
        public IEnumerable<Models.Data.dosen> Get()
        {
            using (var db = new OcphDbContext())
            {
                return db.Dosens.Select();
            }
        }

        // GET: api/Mahasiswa/5

        // POST: api/Mahasiswa
        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<Models.Data.dosen>();
            using (var db = new OcphDbContext())
            {
                var id = db.Dosens.InsertAndGetLastID(item);
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
            var item = await Request.Content.ReadAsAsync<Models.Data.dosen>();
            using (var db = new OcphDbContext())
            {
                var isUpdate = db.Dosens.Update(O => new { O.Nama, O.NIDN }, item, O => O.Id == item.Id);
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
                try
                {
                    var deleted= db.Dosens.Delete(O=>O.Id==id);
                    if (deleted)
                        return Request.CreateResponse(HttpStatusCode.OK, "Data Berhasil Dihapus");
                    else
                        throw new SystemException("Data Tidak Dapat Dihapus");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message);
                }

            }


        }
    }
}
