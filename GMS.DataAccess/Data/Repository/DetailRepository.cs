using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GMS.DataAccess.Data.Repository
{
    public class DetailRepository : IDetailRepository
    {
        private IDbConnection db;

        public DetailRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Trainer GetTrainerWithClients(int id)
        {
            var p = new
            {
                Id = id
            };

            var sql = "SELECT * FROM Trainer WHERE Id = @Id;"
                + "SELECT * FROM Clients WHERE TrainerId = @Id; ";

            Trainer trainer;

            using (var lists = db.QueryMultiple(sql, p))
            {
                trainer = lists.Read<Trainer>().ToList().FirstOrDefault();
                trainer.Clients = lists.Read<Client>().ToList();
            }

            return trainer;
        }
    }
}
