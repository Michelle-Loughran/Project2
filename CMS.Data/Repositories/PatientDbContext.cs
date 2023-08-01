using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// import the Entities (database models representing structure of tables in database)
using CMS.Data.Entities; 

namespace CMS.Data.Repositories
{
    // The Context is How EntityFramework communicates with the database
    // We define DbSet properties for each table in the database
    public class PatientDbContext : DbContext
    {
         // authentication store
        public DbSet<User> Users { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
         public DbSet<Patient> Patients { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<PatientCareEvent> PatientCareEvents { get; set; }

        // Configure the context with logging - remove in production
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // remove in production 
             optionsBuilder
               .UseSqlite("Filename= data.db")
               //.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging()
               ;               
        }

        // Convenience method to recreate the database thus ensuring the new database takes 
        // account of any changes to Models or DatabaseContext. ONLY to be used in development
        public void Initialise()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

    }
}
