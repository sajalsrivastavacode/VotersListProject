using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace VotersListProject.Models
{
    public class AddDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=database-1.c1w04g6a0x81.ap-south-1.rds.amazonaws.com;Database=test_mydb;User=admin;Password=ABCD12345",
                ServerVersion.AutoDetect("Server = database-1.c1w04g6a0x81.ap-south-1.rds.amazonaws.com; Database = test_mydb; User = admin; Password = ABCD12345")
                );
        }
        public DbSet<VoterModel> votertable { get; set; }
        public DbSet<UpdateRequest> UpdateRequests { get; set; }

        


    }
}
