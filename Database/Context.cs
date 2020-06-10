using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Travel_Express.Database
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Deviation> Deviation { get; set; }
        public virtual DbSet<Travel> Travel { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IdAddress)
                    .HasName("prk_constraint_address");

                entity.ToTable("address");

                entity.Property(e => e.IdAddress).HasColumnName("id_address");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(25);

                entity.Property(e => e.Complement)
                    .HasColumnName("complement")
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(25);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(25);

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.IdTravel, e.Author })
                    .HasName("prk_constraint_booking");

                entity.ToTable("booking");

                entity.Property(e => e.IdTravel).HasColumnName("id_travel");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(50);

                entity.Property(e => e.Pending).HasColumnName("pending");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.Author)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_booking_user");

                entity.HasOne(d => d.IdTravelNavigation)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.IdTravel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_booking_travel");
            });

            modelBuilder.Entity<Deviation>(entity =>
            {
                entity.HasKey(e => new { e.IdTravel, e.DeviationOrder })
                    .HasName("prk_constraint_deviation");

                entity.ToTable("deviation");

                entity.Property(e => e.IdTravel).HasColumnName("id_travel");

                entity.Property(e => e.DeviationOrder).HasColumnName("deviation_order");

                entity.Property(e => e.Addr).HasColumnName("addr");

                entity.HasOne(d => d.IdTravelNavigation)
                    .WithMany(p => p.Deviation)
                    .HasForeignKey(d => d.IdTravel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_deviation_travel");
            });

            modelBuilder.Entity<Travel>(entity =>
            {
                entity.HasKey(e => e.IdTravel)
                    .HasName("prk_constraint_travel");

                entity.ToTable("travel");

                entity.Property(e => e.IdTravel).HasColumnName("id_travel");

                entity.Property(e => e.Driver)
                    .HasColumnName("driver")
                    .HasMaxLength(50);

                entity.Property(e => e.From).HasColumnName("from_");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.To).HasColumnName("to_");

                entity.HasOne(d => d.DriverNavigation)
                    .WithMany(p => p.Travel)
                    .HasForeignKey(d => d.Driver)
                    .HasConstraintName("fk_travel_user");

                entity.HasOne(d => d.FromNavigation)
                    .WithMany(p => p.TravelFromNavigation)
                    .HasForeignKey(d => d.From)
                    .HasConstraintName("fk_travel_from");

                entity.HasOne(d => d.ToNavigation)
                    .WithMany(p => p.TravelToNavigation)
                    .HasForeignKey(d => d.To)
                    .HasConstraintName("fk_travel_to");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Mail)
                    .HasName("prk_constraint_users");

                entity.ToTable("users");

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasMaxLength(50);

                entity.Property(e => e.AcceptDeviation).HasColumnName("accept_deviation");

                entity.Property(e => e.AcceptEveryone).HasColumnName("accept_everyone");

                entity.Property(e => e.AcceptMusic).HasColumnName("accept_music");

                entity.Property(e => e.AcceptPet).HasColumnName("accept_pet");

                entity.Property(e => e.AcceptSmoke).HasColumnName("accept_smoke");

                entity.Property(e => e.AcceptTalking).HasColumnName("accept_talking");

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
