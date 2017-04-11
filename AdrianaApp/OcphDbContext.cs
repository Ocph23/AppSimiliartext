using DAL.DContext;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AdrianaApp.Models.Data;

namespace AdrianaApp
{
    public class OcphDbContext : IDbContext, IDisposable
    {
        private string ConnectionString;
        private IDbConnection _Connection;

        public OcphDbContext(string constring)
        {

            this.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public OcphDbContext()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        internal IRepository<abstrak> Abstraks { get { return new Repository<abstrak>(this); } }
        internal IRepository<mahasiswa> Mahasiswa{ get { return new Repository<mahasiswa>(this); } }
        internal IRepository<kemiripan> Kemiripan{ get { return new Repository<kemiripan>(this); } }
        internal IRepository<RootWord> RoadWords { get { return new Repository<RootWord>(this); } }

        public IDbConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new MySqlDbContext(this.ConnectionString);
                    return _Connection;
                }
                else
                {
                    return _Connection;
                }
            }
        }


        public void Dispose()
        {
            if (_Connection != null)
            {
                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
        }
    }
}
