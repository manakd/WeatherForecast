using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WeatherForecast.Models;

public partial class WeatherForecastDbContext : DbContext
{
    //private readonly string _connectionString;
    //public WeatherForecastDbContext(string connectionString)
    //{
    //    _connectionString = connectionString;
    //}

    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ForecastLog> ForecastLogs { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForecastLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Forecast__5E548648A0F9AF88");

            entity.ToTable("ForecastLog");

            entity.Property(e => e.LogId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ForecastDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ForecastDT");
            entity.Property(e => e.ForecastLatitude).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ForecastLongitude).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ForecastTempCurrent).HasColumnType("int");
            entity.Property(e => e.ForecastJson).HasColumnType("varchar(max)");
            entity.Property(e => e.ForecastUrl).HasColumnType("varchar(256)");
            entity.Property(e => e.ForecastSuccess).HasColumnType("bit");
            entity.Property(e => e.ForecastStatus).HasColumnType("varchar(max)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
