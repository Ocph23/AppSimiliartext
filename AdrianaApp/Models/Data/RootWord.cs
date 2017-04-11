using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianaApp.Models.Data
{
    [TableName("tb_rootword")]
    public class RootWord
    {
        [PrimaryKey("Id")]
        [DbColumn("id_ktdasar")]
        public int Id { get; set; }

        [DbColumn("rootword")]
        public string Word { get; set; }

        [DbColumn("tipe_katadasar")]
        public string TypeRootWord { get; set; }


    }
}
