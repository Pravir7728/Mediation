using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL.Mappings
{
    public class UserMapping: EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            ToTable("User");
            HasKey(pk => pk.UserId);
            Property(pr => pr.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
