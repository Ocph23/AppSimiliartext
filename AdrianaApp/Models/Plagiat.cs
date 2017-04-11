using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianaApp.Models.Data;
using AdrianaApp.Models.Views;

namespace AdrianaApp.Models
{
    public class Plagiat
    {
        private List<abstrak> AllAbstraks { get; set; }
        private List<abstrak> ResultAbstraks { get;set; }
        public List<MyData> SameData = new List<MyData>();
        public Plagiat(abstrak a, Views.AbstractSetting setting)
        {
            ResultAbstraks = new List<abstrak>();
            using (var db = new OcphDbContext())
            {
                this.AllAbstraks = db.Abstraks.Select().ToList();
            }

            var newabstract = new Models.Views.AbstractModel(a.Id, a.Judul, a.Abstraksi, setting);
            List<Task> tasks = new List<Task>();
            foreach (var item in AllAbstraks)
            {
              tasks.Add(  Task.Factory.StartNew(()=> {
                    var aa = new Models.Views.AbstractModel(item.Id, item.Judul, item.Abstraksi, setting);
                    item.ProsentaseJudul = RabinKap(newabstract.KGramJudul, aa.KGramJudul);
                    item.ProsentaseAbstrak = RabinKap(newabstract.KGramAbstrak, aa.KGramAbstrak);
                    ResultAbstraks.Add(new abstrak
                    {
                        FileExtention = item.FileExtention,
                        FileName = item.FileName,
                        Id = item.Id,
                        Judul = item.Judul,
                        IdMahasiswa = item.IdMahasiswa,
                        Pembimbing1 = item.Pembimbing1,
                        Pembimbing2 = item.Pembimbing2,
                        ProsentaseAbstrak = item.ProsentaseAbstrak,
                        ProsentaseJudul = item.ProsentaseJudul
                    });

                }));
            }
            Task.WaitAll(tasks.ToArray());
        }

      

        public List<abstrak> Result
        {
            get { return ResultAbstraks; }
        }


        private double RabinKap(List<MyData> kGramJudul1, List<MyData> kGramJudul2)
        {
            int sama = 0;
            foreach (var item in kGramJudul1.GroupBy(O => O.HasCode))
            {
                for (int j = 0; j < kGramJudul2.Count; j++)
                {
                    if (item.Key == kGramJudul2[j].HasCode)
                    {
                        sama += 1;
                        this.SameData.Add(kGramJudul2[j]);
                    }
                }
            }
            double total = kGramJudul1.Count + kGramJudul2.Count - sama;
            double res = (sama / total) * 100;
            return res;
        }

     
    }
}
