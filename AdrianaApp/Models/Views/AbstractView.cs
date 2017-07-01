using AdrianaApp.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianaApp.Models.Views
{
   public  class AbstractView:Models.Data.abstrak
    {

        public int MahasiswaId { get; set; }
        public string NPM { get; set; }
        public string Nama{ get; set; }
        public string Jurusan{ get; set; }
        public string Alamat{ get; set; }
        public dosen PembimbingI { get; set; }
        public dosen PembimbingII { get; set; }

    }
}
