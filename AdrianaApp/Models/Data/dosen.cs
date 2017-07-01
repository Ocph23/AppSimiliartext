using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdrianaApp.Models.Data
{
    [TableName("dosen")]
    public class dosen:BaseNotifyProperty
    {
     
        [PrimaryKey("Id")]
        [DbColumn("Id")]
       
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChange("Id");
            }
        }


        [DbColumn("Nama")]
        public string Nama
        {
            get { return _nama; }
            set { _nama = value; }
        }



        
        [DbColumn("NID")]
        public string NIDN
        {
            get { return _nidn; }
            set { _nidn = value;
                OnPropertyChange("NIDN");
            }
        }

        private int _id;
        private string _nama;
        private string _nidn;
    }
}