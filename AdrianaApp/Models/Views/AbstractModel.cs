using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdrianaApp.Models.Views
{
    public class AbstractModel: DAL.BaseNotifyProperty
    {
      
        private AbstractSetting setting;
        StemmingProcces sp = new StemmingProcces();

        public List<MyData> SourceJudul = new List<MyData>();
        public List<MyData> KGramJudul = new List<MyData>();

        public List<MyData> SourceAbstrak = new List<MyData>();
        public List<MyData> KGramAbstrak = new List<MyData>();

        public AbstractModel( int Id, string Judul, string AbstractText, AbstractSetting setting)
        {
            this.Id = Id;
            this.setting = setting;
            this.Judul = Judul;
            this.AbstrakText = AbstractText;
        }



       private string judul;

        public string Judul
        {
            get {
               
                return judul;
            }
            set
            {
                judul = value;
                CaseFolding(ref judul);
                Filtering(ref judul);
                Tokenizing(ref judul, ref SourceJudul);
                Stemming(ref judul,ref SourceJudul);
                KGramProcces(judul, ref KGramJudul);
                OnPropertyChange("Judul");

            }
        }


        private string abstrak;

        public string AbstrakText
        {
            get
            {

                return abstrak;
            }
            set
            {
                abstrak = value;
                CaseFolding(ref abstrak);
                Filtering(ref abstrak);
                Tokenizing(ref abstrak, ref SourceAbstrak);
                Stemming(ref abstrak, ref SourceAbstrak);
                KGramProcces(abstrak,ref KGramAbstrak);
                OnPropertyChange("AbstrakText");

            }
        }

        public int Id { get; private set; }
        public double ProsentaseJudul { get; internal set; }
        public double ProsentaseAbstrak { get; internal set; }

        //proccess
        private void KGramProcces(string text,ref List<MyData> list)
        {
            list = new List<MyData>();
            var result = KGram(text, setting.KgramLenght);
            var s = result.Split('|').ToList();
            for (int i = 0; i < s.Count; i++)
            {
                if (!string.IsNullOrEmpty(s[i].ToString()))
                    list.Add(new MyData { data = s[i].ToString(), HasCode = this.GetHashCode( s[i].ToString())});
            }
        }
        public string KGram(string text, int Lenght)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length - (Lenght-1); i++)
            {
                var txt = text.Substring(i, Lenght);
                sb.Append(txt + "|");
            }
            return sb.ToString();
        }

        private void Stemming(ref string text, ref List<MyData> list)
        {
            foreach (var item in list)
            {
                item.RootWord = sp.Stemming(item.data);
            }
           text = String.Join("",list.Select(O => O.RootWord));

        }

        private void Tokenizing(ref string text, ref List<MyData> list)
        {
            var s = text.Split(' ').ToList();
            for (int i = 0; i < s.Count; i++)
            {
                if (!string.IsNullOrEmpty(s[i].ToString()))
                   list.Add(new MyData { data = s[i].ToString() });
            }
        }

        private void Filtering(ref string text)
        {
            string WordToRemove = (@"yang,juga,dari,dia,kami,kamu,aku,saya,ini,itu,atau,dan, tersebut, pada, dengan, adalah, yaitu, ke, tak, tidak, di, pada, jika, maka, ada, pun, lain, saja, hanya, namun, seperti, kemudian").Replace(" ", "");
            var pattern = WordToRemove.Split(',');
            var datas = text.Split(' ').ToList();

            var result = datas.Except(pattern).ToList();
            text = String.Join(" ", result);
        }

        private void CaseFolding( ref string text)
        {
            if (setting.HurufBesar)
            {
                text = Regex.Replace(text.ToLower(), @"\s+", " ");
            }

            if (setting.Angka)
            {
                text = new string(text.Where(O => !Char.IsDigit(O)).ToArray());
            }

            if (setting.TandaBaca)
            {
                text = new string(text.Where(O => !Char.IsPunctuation(O)).ToArray());
            }

        }

        //end proccess
        private int GetHashCode(string v)
        {
            var ascci = Encoding.ASCII.GetBytes(v);
            var angkat = ascci.Length - 1;
            int prima =setting.Primes;
            int hasil = 0;
            foreach (var item in ascci)
            {
                var value = Convert.ToInt32(item);
                var va = value * (Math.Pow(prima, angkat));
                hasil += Convert.ToInt32(va);
                angkat -= 1;
            }
            return hasil;
        }

    }


    public class AbstractSetting
    {
        public AbstractSetting(bool tandabaca,bool angka, bool hurufBesar)
        {
            this.TandaBaca = tandabaca;
            this.Angka = angka;
            this.HurufBesar = hurufBesar;
        }

        public AbstractSetting()
        {
        }

        // true jika ingin digunakan
        public bool TandaBaca { get; set; }
        public bool Angka { get; set; }
        public bool HurufBesar { get; set; }
        public double MinProsentase
        {
            get {
                if (min <= 0)
                    return 50;
                else
                    return min;
            }
            set {
                min = value;
            }
        }
        private int _lengt;
        private double min;
        private int _primes;

        public int KgramLenght
        {
            get {
                if (_lengt <= 0)
                    return 5;
                else
                return _lengt; }
            set { _lengt = value; }
        }

        public int Primes
        {
            get
            {
                if (_primes <= 0)
                    return 7;
                return _primes;
            }
            set
            {
                _primes = value;
            }
        }
    }




    public class MyData
    {
        public string data { get; set; }
        public int HasCode { get; internal set; }
        public string RootWord { get; set; }
    }
}
