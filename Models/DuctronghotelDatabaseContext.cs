using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class DuctronghotelDatabaseContext : DbContext
{
    public DuctronghotelDatabaseContext()
    {
    }

    public DuctronghotelDatabaseContext(DbContextOptions<DuctronghotelDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Khachsan> Khachsans { get; set; }

    public virtual DbSet<Loaiphong> Loaiphongs { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Reserved> Reserveds { get; set; }

    public virtual DbSet<Thanhpho> Thanhphos { get; set; }

    public virtual DbSet<Tinhtien> Tinhtiens { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:ductronghotel-server.database.windows.net,1433;Initial Catalog=ductronghotel-database;Persist Security Info=False;User ID=ductronghotel-server-admin;Password=196196@Sweet;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Idadmin).HasName("PK__admin__5B93BD6E91AB68DD");

            entity.ToTable("admin");

            entity.Property(e => e.Idadmin).HasColumnName("idadmin");
            entity.Property(e => e.Emailadmin)
                .HasMaxLength(100)
                .HasColumnName("emailadmin");
            entity.Property(e => e.Passwordadmin)
                .HasMaxLength(100)
                .HasColumnName("passwordadmin");
        });

        modelBuilder.Entity<Khachsan>(entity =>
        {
            entity.HasKey(e => e.Idkhachsan).HasName("PK__khachsan__733B502617EEF2FD");

            entity.ToTable("khachsan");

            entity.Property(e => e.Idkhachsan)
                .ValueGeneratedNever()
                .HasColumnName("idkhachsan");
            entity.Property(e => e.Diachiks)
                .HasMaxLength(100)
                .HasColumnName("diachiks");
            entity.Property(e => e.Idthanhpho).HasColumnName("idthanhpho");
            entity.Property(e => e.Img)
                .HasMaxLength(100)
                .HasColumnName("img");
            entity.Property(e => e.Sdtks)
                .HasMaxLength(10)
                .HasColumnName("sdtks");
            entity.Property(e => e.Tenchinhanh)
                .HasMaxLength(50)
                .HasColumnName("tenchinhanh");
        });

        modelBuilder.Entity<Loaiphong>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("loaiphong");

            entity.Property(e => e.Bed).HasColumnName("bed");
            entity.Property(e => e.Dientich).HasColumnName("dientich");
            entity.Property(e => e.Gia).HasColumnName("gia");
            entity.Property(e => e.Idtype).HasColumnName("idtype");
            entity.Property(e => e.Loaiphong1)
                .HasMaxLength(20)
                .HasColumnName("loaiphong");
            entity.Property(e => e.Size).HasColumnName("size");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Idroom).HasName("PK__phong__F3249856213AFD51");

            entity.ToTable("phong");

            entity.Property(e => e.Idroom).HasColumnName("idroom");
            entity.Property(e => e.Idksan).HasColumnName("idksan");
            entity.Property(e => e.Roomtypeid).HasColumnName("roomtypeid");
            entity.Property(e => e.Sophong)
                .HasMaxLength(15)
                .HasColumnName("sophong");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(20)
                .HasColumnName("trangthai");
        });

        modelBuilder.Entity<Reserved>(entity =>
        {
            entity.HasKey(e => e.Reservedid).HasName("PK__reserved__B1DA827BBAB1930B");

            entity.ToTable("reserved");

            entity.Property(e => e.Reservedid).HasColumnName("reservedid");
            entity.Property(e => e.Checkin).HasColumnName("checkin");
            entity.Property(e => e.Checkout).HasColumnName("checkout");
            entity.Property(e => e.Idksan).HasColumnName("idksan");
            entity.Property(e => e.Idroom).HasColumnName("idroom");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Thanhtoanchua)
                .HasMaxLength(50)
                .HasColumnName("thanhtoanchua");
        });

        modelBuilder.Entity<Thanhpho>(entity =>
        {
            entity.HasKey(e => e.Idthanhpho).HasName("PK__thanhpho__21B73AA04380B42A");

            entity.ToTable("thanhpho");

            entity.Property(e => e.Idthanhpho)
                .ValueGeneratedNever()
                .HasColumnName("IDTHANHPHO");
            entity.Property(e => e.Tenthanhpho)
                .HasMaxLength(50)
                .HasColumnName("TENTHANHPHO");
        });

        modelBuilder.Entity<Tinhtien>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("PK__tinhtien__C2BEC91069FD7627");

            entity.ToTable("tinhtien");

            entity.Property(e => e.RId)
                .ValueGeneratedNever()
                .HasColumnName("rID");
            entity.Property(e => e.Checkin).HasColumnName("checkin");
            entity.Property(e => e.Checkout).HasColumnName("checkout");
            entity.Property(e => e.Gia).HasColumnName("gia");
            entity.Property(e => e.Sophong)
                .HasMaxLength(20)
                .HasColumnName("sophong");
            entity.Property(e => e.Tenchinhanh)
                .HasMaxLength(50)
                .HasColumnName("tenchinhanh");
            entity.Property(e => e.Tongngay).HasColumnName("tongngay");
            entity.Property(e => e.Tongtien).HasColumnName("tongtien");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("trangthai");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__userr__CBA1B2576525FF5D");

            entity.ToTable("userr");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Emailuser)
                .HasMaxLength(50)
                .HasColumnName("emailuser");
            entity.Property(e => e.Sdtuser)
                .HasMaxLength(10)
                .HasColumnName("sdtuser");
            entity.Property(e => e.Tenuser)
                .HasMaxLength(100)
                .HasColumnName("tenuser");
            entity.Property(e => e.Userpassword)
                .HasMaxLength(100)
                .HasColumnName("userpassword");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
