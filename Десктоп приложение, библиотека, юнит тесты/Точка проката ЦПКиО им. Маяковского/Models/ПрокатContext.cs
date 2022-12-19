using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ПрокатContext : DbContext
    {
        private string? connectionString;

        public ПрокатContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ПрокатContext(DbContextOptions<ПрокатContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ДвиженияЗаказов> ДвиженияЗаказовs { get; set; } = null!;
        public virtual DbSet<Должности> Должностиs { get; set; } = null!;
        public virtual DbSet<Заказы> Заказыs { get; set; } = null!;
        public virtual DbSet<ИсторияВходов> ИсторияВходовs { get; set; } = null!;
        public virtual DbSet<Клиенты> Клиентыs { get; set; } = null!;
        public virtual DbSet<Оборудование> Оборудованиеs { get; set; } = null!;
        public virtual DbSet<Пользователи> Пользователиs { get; set; } = null!;
        public virtual DbSet<ПредоставлениеУслуг> ПредоставлениеУслугs { get; set; } = null!;
        public virtual DbSet<ПрокатОборудования> ПрокатОборудованияs { get; set; } = null!;
        public virtual DbSet<Сотрудники> Сотрудникиs { get; set; } = null!;
        public virtual DbSet<СтатусыЗаказов> СтатусыЗаказовs { get; set; } = null!;
        public virtual DbSet<СтатусыПроката> СтатусыПрокатаs { get; set; } = null!;
        public virtual DbSet<ТипыВходов> ТипыВходовs { get; set; } = null!;
        public virtual DbSet<ТипыОборудования> ТипыОборудованияs { get; set; } = null!;
        public virtual DbSet<ТипыУслуг> ТипыУслугs { get; set; } = null!;
        public virtual DbSet<Услуги> Услугиs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (connectionString != null)
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ДвиженияЗаказов>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Движения заказов");

                entity.Property(e => e.Дата).HasColumnType("date");

                entity.HasOne(d => d.ЗаказNavigation)
                    .WithMany(p => p.ДвиженияЗаказовs)
                    .HasForeignKey(d => d.Заказ)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Движения заказов_Заказы");

                entity.HasOne(d => d.ПользовательNavigation)
                    .WithMany(p => p.ДвиженияЗаказовs)
                    .HasForeignKey(d => d.Пользователь)
                    .HasConstraintName("FK_Движения заказов_Пользователи");

                entity.HasOne(d => d.СтатусNavigation)
                    .WithMany(p => p.ДвиженияЗаказовs)
                    .HasForeignKey(d => d.Статус)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Движения заказов_Статусы заказов");
            });

            modelBuilder.Entity<Должности>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Должности");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<Заказы>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Заказы");

                entity.Property(e => e.ВремяПрокатаЧасов).HasColumnName("Время проката (часов)");

                entity.Property(e => e.КодЗаказа)
                    .HasMaxLength(50)
                    .HasColumnName("Код заказа");

                entity.HasOne(d => d.КлиентNavigation)
                    .WithMany(p => p.Заказыs)
                    .HasForeignKey(d => d.Клиент)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Заказы_Клиенты");
            });

            modelBuilder.Entity<ИсторияВходов>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("История входов");

                entity.Property(e => e.Дата).HasColumnType("date");

                entity.HasOne(d => d.ПользовательNavigation)
                    .WithMany(p => p.ИсторияВходовs)
                    .HasForeignKey(d => d.Пользователь)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_История входов_Пользователи");

                entity.HasOne(d => d.ТипNavigation)
                    .WithMany(p => p.ИсторияВходовs)
                    .HasForeignKey(d => d.Тип)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_История входов_Типы входов");
            });

            modelBuilder.Entity<Клиенты>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Клиенты");

                entity.Property(e => e.Адрес).HasMaxLength(100);

                entity.Property(e => e.ДатаРождения)
                    .HasColumnType("datetime")
                    .HasColumnName("Дата рождения");

                entity.Property(e => e.Имя).HasMaxLength(50);

                entity.Property(e => e.НомерПаспорта)
                    .HasMaxLength(6)
                    .HasColumnName("Номер паспорта");

                entity.Property(e => e.Отчество).HasMaxLength(50);

                entity.Property(e => e.СерияПаспорта)
                    .HasMaxLength(4)
                    .HasColumnName("Серия паспорта");

                entity.Property(e => e.Фамилия).HasMaxLength(50);

                entity.Property(e => e.ЭлектроннаяПочта)
                    .HasMaxLength(50)
                    .HasColumnName("Электронная почта");
            });

            modelBuilder.Entity<Оборудование>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Оборудование");

                entity.Property(e => e.КодОборудования)
                    .HasMaxLength(50)
                    .HasColumnName("Код оборудования");

                entity.HasOne(d => d.ТипNavigation)
                    .WithMany(p => p.Оборудованиеs)
                    .HasForeignKey(d => d.Тип)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Оборудование_Типы оборудования");
            });

            modelBuilder.Entity<Пользователи>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Пользователи");

                entity.Property(e => e.Логин).HasMaxLength(50);

                entity.Property(e => e.Пароль).HasMaxLength(50);

                entity.HasOne(d => d.СотрудникNavigation)
                    .WithMany(p => p.Пользователиs)
                    .HasForeignKey(d => d.Сотрудник)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Пользователи_Сотрудники");
            });

            modelBuilder.Entity<ПредоставлениеУслуг>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Предоставление услуг");

                entity.HasOne(d => d.ЗаказNavigation)
                    .WithMany(p => p.ПредоставлениеУслугs)
                    .HasForeignKey(d => d.Заказ)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Предоставление услуг_Заказы");

                entity.HasOne(d => d.УслугаNavigation)
                    .WithMany(p => p.ПредоставлениеУслугs)
                    .HasForeignKey(d => d.Услуга)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Предоставление услуг_Услуги");
            });

            modelBuilder.Entity<ПрокатОборудования>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Прокат оборудования");

                entity.HasOne(d => d.ЗаказNavigation)
                    .WithMany(p => p.ПрокатОборудованияs)
                    .HasForeignKey(d => d.Заказ)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Прокат оборудования_Заказы");

                entity.HasOne(d => d.ОборудованиеNavigation)
                    .WithMany(p => p.ПрокатОборудованияs)
                    .HasForeignKey(d => d.Оборудование)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Прокат оборудования_Оборудование");

                entity.HasOne(d => d.СтатусNavigation)
                    .WithMany(p => p.ПрокатОборудованияs)
                    .HasForeignKey(d => d.Статус)
                    .HasConstraintName("FK_Прокат оборудования_Статусы проката");
            });

            modelBuilder.Entity<Сотрудники>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Сотрудники");

                entity.Property(e => e.Имя).HasMaxLength(50);

                entity.Property(e => e.Отчество).HasMaxLength(50);

                entity.Property(e => e.Фамилия).HasMaxLength(50);

                entity.HasOne(d => d.ДолжностьNavigation)
                    .WithMany(p => p.Сотрудникиs)
                    .HasForeignKey(d => d.Должность)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Сотрудники_Должности");
            });

            modelBuilder.Entity<СтатусыЗаказов>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Статусы заказов");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<СтатусыПроката>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Статусы проката");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<ТипыВходов>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Типы входов");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<ТипыОборудования>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Типы оборудования");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<ТипыУслуг>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Типы услуг");

                entity.Property(e => e.Код).ValueGeneratedNever();

                entity.Property(e => e.Наименование).HasMaxLength(50);
            });

            modelBuilder.Entity<Услуги>(entity =>
            {
                entity.HasKey(e => e.Код);

                entity.ToTable("Услуги");

                entity.Property(e => e.КодУслуги)
                    .HasMaxLength(50)
                    .HasColumnName("Код услуги");

                entity.Property(e => e.Наименование).HasMaxLength(50);

                entity.Property(e => e.СтоимостьРублейЗаЧас).HasColumnName("Стоимость (рублей за час)");

                entity.HasOne(d => d.ТипNavigation)
                    .WithMany(p => p.Услугиs)
                    .HasForeignKey(d => d.Тип)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Услуги_Типы услуг");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
