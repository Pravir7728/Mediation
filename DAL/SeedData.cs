using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    internal sealed class SeedData : DbMigrationsConfiguration<MediationEntities>
    {
        public SeedData()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MediationEntities context)
        {
            context.Users.AddOrUpdate(new User
            {
                UserId = Guid.NewGuid(),
                UserName = "Admin",
                Password = "Admin",
                DateCaptured = DateTime.Now
            });
        }
    }
}
