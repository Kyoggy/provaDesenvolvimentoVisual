﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using folhaPagamento.Data;

#nullable disable

namespace folhaPagamento.Migrations
{
    [DbContext(typeof(AppDataContext))]
    partial class AppDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.21");

            modelBuilder.Entity("folhaPagamento.Models.Folha", b =>
                {
                    b.Property<int>("FolhaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ano")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FuncionarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("FolhaId");

                    b.HasIndex("FuncionarioId");

                    b.ToTable("Folhas");
                });

            modelBuilder.Entity("folhaPagamento.Models.Funcionario", b =>
                {
                    b.Property<int>("FuncionarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("FuncionarioId");

                    b.ToTable("Funcionarios");
                });

            modelBuilder.Entity("folhaPagamento.Models.Folha", b =>
                {
                    b.HasOne("folhaPagamento.Models.Funcionario", "Funcionario")
                        .WithMany()
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Funcionario");
                });
#pragma warning restore 612, 618
        }
    }
}