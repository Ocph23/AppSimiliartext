using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianaApp.Models.Views;
using DAL;
namespace AdrianaApp.Models.Data
{
    [TableName("abstrak")]
    public class abstrak : BaseNotifyProperty
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

        [DbColumn("IdMahasiswa")]
        public int IdMahasiswa
        {
            get { return _idmahasiswa; }
            set
            {
                _idmahasiswa = value;
                OnPropertyChange("IdMahasiswa");
            }
        }

        [DbColumn("Judul")]
        public string Judul
        {
            get { return _judul; }
            set
            {
                _judul = value;
                OnPropertyChange("Judul");
            }
        }

        [DbColumn("Pembimbing1")]
        public int Pembimbing1
        {
            get { return _pembimbing1; }
            set
            {
                _pembimbing1 = value;
                OnPropertyChange("Pembimbing1");
            }
        }

        [DbColumn("Pembimbing2")]
        public int Pembimbing2
        {
            get { return _pembimbing2; }
            set
            {
                _pembimbing2 = value;
                OnPropertyChange("Pembimbing2");
            }
        }

        [DbColumn("Abstraksi")]
        public string Abstraksi
        {
            get { return _abstraksi; }
            set
            {
                _abstraksi = value;
                OnPropertyChange("Abstraksi");
            }
        }

        [DbColumn("FileName")]
        public string FileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChange("FileName");
            }
        }

        [DbColumn("FileTipe")]
        public string FileTipe
        {
            get { return _filetipe; }
            set
            {
                _filetipe = value;
                OnPropertyChange("FileTipe");
            }
        }

        [DbColumn("FileExtention")]
        public string FileExtention
        {
            get { return _fileextention; }
            set
            {
                _fileextention = value;
                OnPropertyChange("FileExtention");
            }
        }

        public double ProsentaseJudul
        {
            get
            {
                return Helper.RoundUp(_ProsentaseJudul, 2);
            }
            set
            {
                _ProsentaseJudul = value;
            }
        }
        public double ProsentaseAbstrak
        {
            get
            {
                return Helper.RoundUp(_ProsentaseAbstrak, 2);
            }
            set
            {
                _ProsentaseAbstrak = value;
            }
        }

       

        public byte[] data { get; internal set; }
        public mahasiswa Mahasiswa { get; internal set; }
        public AbstractModel ProccessResult { get; internal set; }

        private int _id;
        private int _idmahasiswa;
        private string _judul;
        private int _pembimbing1;
        private int _pembimbing2;
        private string _abstraksi;
        private string _filename;
        private string _filetipe;
        private string _fileextention;
        private double _ProsentaseJudul;
        private double _ProsentaseAbstrak;
    }
}