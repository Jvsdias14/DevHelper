using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DevHelper.Data.Model;

public partial class DBdevhelperContext : DbContext
{
    public DBdevhelperContext()
    {
    }

    public DBdevhelperContext(DbContextOptions<DBdevhelperContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ArquivoProblema> ArquivoProblemas { get; set; }

    public virtual DbSet<ArquivoSolucao> ArquivoSolucoes { get; set; }

    public virtual DbSet<Problema> Problemas { get; set; }

    public virtual DbSet<Solucao> Solucoes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=devhelper;Trusted_connection=True;TrustServerCertificate=True", options => options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
        }
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArquivoProblema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArquivoP__3213E83FF75EC52C");

            entity.ToTable("ArquivoProblema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProblemaId).HasColumnName("Problema_id");
            entity.Property(e => e.Referencia)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Problema).WithMany(p => p.ArquivoProblemas)
                .HasForeignKey(d => d.ProblemaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArquivoPr__Probl__4E88ABD4");
        });

        modelBuilder.Entity<ArquivoSolucao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArquivoS__3213E83FEA131284");

            entity.ToTable("ArquivoSolucao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Referencia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SolucaoId).HasColumnName("Solucao_id");

            entity.HasOne(d => d.Solucao).WithMany(p => p.ArquivoSolucaos)
                .HasForeignKey(d => d.SolucaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArquivoSo__Soluc__5535A963");
        });

        modelBuilder.Entity<Problema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Problema__3213E83FDB7E4C76");

            entity.ToTable("Problema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Problemas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Problema__Usuari__4BAC3F29");
        });

        modelBuilder.Entity<Solucao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solucao__3213E83F5B93C336");

            entity.ToTable("Solucao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.ProblemaId).HasColumnName("Problema_id");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_id");

            entity.HasOne(d => d.Problema).WithMany(p => p.Solucaos)
                .HasForeignKey(d => d.ProblemaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solucao__Problem__52593CB8");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Solucaos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solucao__Usuario__5165187F");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3213E83FB53E040F");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Biografia)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
