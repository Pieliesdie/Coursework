using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace Model
{
    public partial class NotConsultantv2Context : DbContext
    {
        public NotConsultantv2Context(){ }

        public NotConsultantv2Context(DbContextOptions<NotConsultantv2Context> options) : base(options) { }

        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SecurityLabels> SecurityLabels { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        private DbSet<UserReadPermission> _userReadPermissions { get; set; }
        private DbSet<UserWritePermission> _userWritePermissions { get; set; }


        public IQueryable<UserReadPermission> UserReadPermissions(int id) => this._userReadPermissions.FromSqlInterpolated($"select * from GetUserReadPermissions({id})");

        public IQueryable<UserWritePermission> UserWritePermissions(int id) => this._userWritePermissions.FromSqlInterpolated($"select * from GetUserWritePermissions({id})");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {          
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MK32EF2;Database=NotConsultantv2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Decription).HasMaxLength(150);

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Articles__Author__5BE2A6F2");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Articles__Docume__5CD6CB2B");

                entity.HasOne(d => d.SecurityLabelNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.SecurityLabel)
                    .HasConstraintName("FK__Articles__Securi__5DCAEF64");
            });

            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Surname, e.MiddleName, e.Email })
                    .HasName("UQ__Authors__3C3DEDF63452B2BF")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.SecurityLabelNavigation)
                    .WithMany(p => p.Authors)
                    .HasForeignKey(d => d.SecurityLabel)
                    .HasConstraintName("FK__Authors__Securit__59063A47");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK__Comments__Articl__619B8048");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Comments__Author__60A75C0F");

                entity.HasOne(d => d.SecurityLabelNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.SecurityLabel)
                    .HasConstraintName("FK__Comments__Securi__628FA481");
            });

            modelBuilder.Entity<DocumentTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.SecurityLabelNavigation)
                    .WithMany(p => p.DocumentTypes)
                    .HasForeignKey(d => d.SecurityLabel)
                    .HasConstraintName("FK__DocumentT__Secur__534D60F1");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Roles__737584F7698B6A88");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SecurityLabels>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Security__737584F6CEEC7C4E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(150);

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasComputedColumnSql("([Label_hid].[GetLevel]())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Users__5E55825B84415D4D")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.HasOne(d => d.PermissionNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Permission)
                    .HasConstraintName("FK__Users__Permissio__07C12930");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__Users__Role__06CD04F7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
