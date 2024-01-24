using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EliteBookings.Models
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Club> Clubs { get; set; } = null!;
        public virtual DbSet<Condition> Conditions { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User Id=postgres;Password=4qJe7pfdsKG2WF3P;Server=db.shbyxunuccvdqivuqoqk.supabase.co;Port=5432;Database=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
                .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
                .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
                .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
                .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
                .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
                .HasPostgresExtension("extensions", "pg_stat_statements")
                .HasPostgresExtension("extensions", "pgcrypto")
                .HasPostgresExtension("extensions", "pgjwt")
                .HasPostgresExtension("extensions", "uuid-ossp")
                .HasPostgresExtension("graphql", "pg_graphql")
                .HasPostgresExtension("pgsodium", "pgsodium")
                .HasPostgresExtension("vault", "supabase_vault");

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Users_pkey");

                entity.HasComment("users table");
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.HasComment("clubs table");

                entity.HasOne(d => d.AdminUser)
                    .WithMany(p => p.Clubs)
                    .HasForeignKey(d => d.AdminUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Clubs_admin_user_id_fkey");
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasComment("conditions table");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Conditions)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Conditions_section_id_fkey");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasComment("reservations table");

                entity.Property(e => e.Status).HasDefaultValueSql("'pending'::character varying");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Reservations_club_id_fkey");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Reservations_section_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Reservations_user_id_fkey");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasComment("sections table");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Sections_club_id_fkey");
            });

            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
