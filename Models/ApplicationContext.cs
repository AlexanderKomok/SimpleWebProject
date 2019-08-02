using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppTry3.DBEntities;

namespace WebAppTry3.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Track_Album> Track_Albums { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<User> DBUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Realization relationship many to many between Track and Album in EF Core

            modelBuilder.Entity<Track_Album>().HasKey(ce =>  new{ce.TrackId, ce.AlbumId });

            modelBuilder.Entity<Track_Album>()
                .HasOne<Track>(ce => ce.Track)
                .WithMany(t => t.ConnectEntities)
                .HasForeignKey(ce => ce.TrackId);

            modelBuilder.Entity<Track_Album>()
                .HasOne<Album>(ce => ce.Album)
                .WithMany(a => a.ConnectEntities)
                .HasForeignKey(ce => ce.AlbumId);


            //Realization relationship one to many between User and Album in EF Core

            modelBuilder.Entity<Album>()
                .HasOne<User>(a => a.User)
                .WithMany(dbu => dbu.Albums)
                .HasForeignKey(a => a.UserId);

            //Realization relationship one to many between User and Track in EF Core
            modelBuilder.Entity<Track>()
                .HasOne<User>(t => t.User)
                .WithMany(dbu => dbu.Tracks)
                .HasForeignKey(t => t.UserId);

            //Stuffing Track in separate method
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackName).HasColumnName("Song").HasMaxLength(100);
            modelBuilder.Entity<Track>()
                .Property(t => t.PlayerState).HasColumnName("PlayerState");
            modelBuilder.Entity<Track>()
                .Property(t => t.TrackUrl).HasColumnName("Link").HasMaxLength(250).IsRequired();

            //Stuffing Album in separate method
            modelBuilder.Entity<Album>()
                .Property(a => a.AlbumId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Album>()
                .Property(a => a.AlbumName).HasColumnName("AlbumName");

            //Stuffing Track_Album inseparate method
            modelBuilder.Entity<Track_Album>()
                .Property(ta => ta.Track_AlbumId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Track_Album>()
                .Property(ta => ta.TrackId).HasColumnName("TrackId");
            modelBuilder.Entity<Track_Album>()
                .Property(ta => ta.AlbumId).HasColumnName("AlbumId");

            base.OnModelCreating(modelBuilder);
        }

        
    }

}
