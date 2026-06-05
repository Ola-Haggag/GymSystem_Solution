using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(X => X.Id);
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(X => X.Session)
                   .WithMany(X => X.Bookings)
                   .HasForeignKey(X => X.SessionId);

            builder.HasOne(X => X.Member)
                   .WithMany(X => X.Bookings)
                   .HasForeignKey(X => X.MemberId);

            builder.HasKey(X => new { X.SessionId, X.MemberId });
        }
    }
}
