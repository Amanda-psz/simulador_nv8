using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Dialect;
using NHibernate.Driver;
using simulador_nivel8.Data.Models;

namespace simulador_nivel8.Data
{
    public class ConfigDB
    {
        public static NHibernate.Cfg.Configuration SimpleConfig()
        {
            string connectionstr = "Data Source=AMANDAPS\\SQLEXPRESS;Initial Catalog=gerenciador_de_pecas_veiculos;Trusted_Connection=True";

            var config = new NHibernate.Cfg.Configuration();
            config.DataBaseIntegration(x =>
            {
                x.ConnectionString = connectionstr;
                x.Dialect<MsSql2012Dialect>();
                x.Driver<SqlClientDriver>();
            });

            config.AddAssembly(Assembly.GetExecutingAssembly());
            config.AddAssembly(typeof(Produto).Assembly);

            config.AddFile(@"C:\Users\prsou\source\repos\simulador_nivel8\simulador_nivel8\Mappings\Produto.hbm.xml");

            return config;
        }
    }
}
