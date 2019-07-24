using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAppTry3.DBEntities;

namespace WebAppTry3.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            //Database.SetInitializer<ApplicationContex>(null);
        }

        //

        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> DBUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Realization relationship many to many between Track and Album in EF Core
           

            //Realization relationship one to many between User and Album in EF Core


            //Realization relationship one to many between User and Track in EF Core
            modelBuilder.Entity<Track>()
                .HasOne<User>(t => t.User)
                .WithMany(dbu => dbu.Tracks)
                .HasForeignKey(t => t.UserId);

            //Stuffing Track in separate method
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Track>()
                .Property(t => t.ArtistName).HasColumnName("Band").HasMaxLength(20);
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackName).HasColumnName("Song").HasMaxLength(100);
            modelBuilder.Entity<Track>()
                .Property(t => t.Grade).HasColumnName("Grade");
            modelBuilder.Entity<Track>()
                .Property(t => t.Album).HasColumnName("AlbumName");
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackUrl).HasColumnName("Link").HasMaxLength(250).IsRequired();
           
            //modelBuilder.Entity<Track>()
            //.Property(t => t.UserName).HasColumnName("UserName").HasMaxLength(250);

            //Stuffing Album in separate method


            base.OnModelCreating(modelBuilder);
        }
    }

}
