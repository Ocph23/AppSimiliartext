using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using DAL;namespace AdrianaApp.Models.Data 
{ 
     [TableName("mahasiswa")] 
     public class mahasiswa:BaseNotifyProperty  
   {
          [PrimaryKey("Id")] 
          [DbColumn("Id")] 
          public int Id 
          { 
               get{return _id;} 
               set{ 
                      _id=value; 
                     OnPropertyChange("Id");
                     }
          } 

          [DbColumn("NPM")] 
          public string NPM 
          { 
               get{return _npm;} 
               set{ 
                      _npm=value; 
                     OnPropertyChange("NPM");
                     }
          } 

          [DbColumn("Nama")] 
          public string Nama 
          { 
               get{return _nama;} 
               set{ 
                      _nama=value; 
                     OnPropertyChange("Nama");
                     }
          } 

          [DbColumn("Jurusan")] 
          public string Jurusan 
          { 
               get{return _jurusan;} 
               set{ 
                      _jurusan=value; 
                     OnPropertyChange("Jurusan");
                     }
          } 

          [DbColumn("Alamat")] 
          public string Alamat 
          { 
               get{return _alamat;} 
               set{ 
                      _alamat=value; 
                     OnPropertyChange("Alamat");
                     }
          } 

          private int  _id;
           private string  _npm;
           private string  _nama;
           private string  _jurusan;
           private string  _alamat;
      }
}


