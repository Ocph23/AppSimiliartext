using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using DAL;namespace AdrianaApp.Models.Data 
{ 
     [TableName("kemiripan")] 
     public class kemiripan:BaseNotifyProperty  
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

          [DbColumn("IdJudul")] 
          public int IdJudul 
          { 
               get{return _idjudul;} 
               set{ 
                      _idjudul=value; 
                     OnPropertyChange("IdJudul");
                     }
          } 

          [DbColumn("IdKemiripan")] 
          public int IdKemiripan 
          { 
               get{return _idkemiripan;} 
               set{ 
                      _idkemiripan=value; 
                     OnPropertyChange("IdKemiripan");
                     }
          } 

          [DbColumn("Prosentase")] 
          public double Prosentase 
          { 
               get{return _prosentase;} 
               set{ 
                      _prosentase=value; 
                     OnPropertyChange("Prosentase");
                     }
          } 

          private int  _id;
           private int  _idjudul;
           private int  _idkemiripan;
           private double  _prosentase;
      }
}


