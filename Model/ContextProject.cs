using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.DataModel;

namespace Model
{
    public class ContextProject: DbContext
    {
        public ContextProject(DbContextOptions<ContextProject> contextOptions) : base(contextOptions)
        {

        }
        public DbSet<Capteur> Capteur { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Capteur>(entity =>
            {
                // Définir Id comme clé primaire et auto-incrémenté
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()  // Auto-incrémentation
                      .IsRequired();          // Non nul

          
            });
            // Insertion de données initiales
            modelBuilder.Entity<Capteur>().HasData(
                new Capteur
                {
                    Id = 1,
                    Name = "Capteur 01",
                    Type = "Type01",
                    Value = 22.5,
                    Dt_Modif = DateTime.UtcNow
                },
                new Capteur
                {
                    Id = 2,
                    Name = "Capteur 02",
                    Type = "Type02",
                    Value = 55.0,
                    Dt_Modif = DateTime.UtcNow
                }
            );
        }
    }
}
