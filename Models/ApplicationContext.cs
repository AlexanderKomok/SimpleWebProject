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
        public DbSet<Album> Albums {get; set;}
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> DBUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Realization relationship many to many between Track and Album in EF Core
            modelBuilder.Entity<ConnectModel>()
                .HasKey(cm => new { cm.TrackID, cm.AlbumID });


            modelBuilder.Entity<ConnectModel>()
                .HasOne<Track>(cm => cm.Track)
                .WithMany(t => t.ConnectModels)
                .HasForeignKey(cm => cm.TrackID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ConnectModel>()
                .HasOne<Album>(cm => cm.Album)
                .WithMany(t => t.ConnectModels)
                .HasForeignKey(cm => cm.AlbumID)
                .OnDelete(DeleteBehavior.Restrict);

            //Realization relationship one to many between User and Album in EF Core
            modelBuilder.Entity<Album>()
                .HasOne<User>(a => a.User)
                .WithMany(dbu => dbu.Albums);

            //Realization relationship one to many between User and Track in EF Core
            modelBuilder.Entity<Track>()
                .HasOne<User>(t => t.User)
                .WithMany(dbu => dbu.Tracks)
                .HasForeignKey(t => t.UserId);

            //Stuffing Track in separate method
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Track>()
                .Property(t => t.ArtistName).HasColumnName("Band").HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackName).HasColumnName("Song").HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Track>()
                .Property(t => t.Grade).HasColumnName("Grade").IsRequired();
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackUrl).HasColumnName("Link").HasMaxLength(250).IsRequired();
            //modelBuilder.Entity<Track>()
                //.Property(t => t.UserName).HasColumnName("UserName").HasMaxLength(250);

            //Stuffing Album in separate method
            modelBuilder.Entity<Album>()
                .Property(a => a.AlbumID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Album>()
                .Property(a => a.AlbumName).HasColumnName("Album").HasMaxLength(20).IsRequired();
            //
            //modelBuilder.Entity<User>()
            //.Property(dbu => dbu.Name).HasColumnName("User Name").IsRequired();
            //Model Relationship

            //modelBuilder.ApplyConfiguration(new AlbumEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new TrackEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new UserEntityConfiguration());            
            base.OnModelCreating(modelBuilder);
        }
    }

}
