/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppTry3.DBEntities;

namespace WebAppTry3.Models.DBConfigurations

    internal class TrackEntityConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        //builder.ToTable("Playlist");
        //builder.HasKey(x => x.ID);

        ModelBuilder.Entity<Track>()
            .HasKey(t => new { t.ID });
                .Map(t =>
            {
                m.Properties(pt => new { pt.Track })
            }
                );
                .HasForeignKey(t => t.UserID);

        //builder.Property(x => x.ArtistName).HasColumnName("Band").HasMaxLength(20).IsRequired();
        //builder.Property(x => x.TrackName).HasColumnName("Song").HasMaxLength(30).IsRequired();
        //builder.Property(x => x.Grade).HasColumnName("Grade").IsRequired();
        //builder.Property(x => x.TrackUrl).HasColumnName("Link").HasMaxLength(250).IsRequired();

        //builder.Metadata.FindNavigation(nameof(Track.User)).SetPropertyAccessMode(PropertyAccessMode.Field);
        //ModelBuilder.Entity<Track>(a => a.HasMany(typeof(Track)).WithOne(typeof(User)).HasForeignKey("UserID");
        //builder.HasOne(x => x.User).WithMany(b => b.Tracks).HasForeignKey(b => b.UserID);
        //builder.HasOne(x => x.Albums).WithMany(c => c.Tracks).HasForeignKey(b => b.UserID);
        //builder.HasMany(d => d.Albums).WithMany(c => c.Tracks).HasForeignKey(x => x.AlbumID);

    }
}*/


