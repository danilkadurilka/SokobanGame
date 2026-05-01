using Microsoft.EntityFrameworkCore;
using SokobanGame.Models;

namespace SokobanGame.Database
{
    public class SokobanDbContext : DbContext
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<Record> Records { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=DBSRV\vip2025;Database=KursDS_SokobanDB;Integrated Security=True;TrustServerCertificate=True;");
        //    }
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=SokobanDB;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Level>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Width).IsRequired();
                entity.Property(e => e.Height).IsRequired();
                entity.Property(e => e.MapData).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.IsDefault).HasDefaultValue(false);
                entity.HasMany(e => e.Records)
                      .WithOne(e => e.Level)
                      .HasForeignKey(e => e.LevelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlayerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CountMoves).IsRequired();
                entity.Property(e => e.Time).IsRequired();
                entity.Property(e => e.CompletedAt).IsRequired();
                entity.HasOne(e => e.Level)
                      .WithMany(e => e.Records)
                      .HasForeignKey(e => e.LevelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
            SeedDefaultLevels();
        }
        private void SeedDefaultLevels()
        {
            if (!Levels.Any())
            {
                Level[] defaultLevels =
                [
                    new Level
                    {
                        Name = "Уровень 1",
                        Width = 10,
                        Height = 5,
                        MapData = "##########\n#  $    .#\n##   $  .#\n#@       #\n##########",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 2",
                        Width = 10,
                        Height = 10,
                        MapData = "   ###    \n   #.#    \n   # #####\n####$ $ .#\n#.  $@####\n#####$#   \n    # #   \n    # #   \n    #.#   \n    ###   ",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 3",
                        Width = 8,
                        Height = 9,
                        MapData = "########\n#@   ###\n####$###\n##    .#\n#. $. ##\n##  # ##\n##  $ ##\n##  ####\n########",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 4",
                        Width = 19,
                        Height = 9,
                        MapData = "           #######\n           #     #\n############    .#\n#    #    $      #\n#    #     #    .#\n#          #######\n#    #$    #      \n# @        #      \n############      ",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 5",
                        Width = 12,
                        Height = 4,
                        MapData = "##########  \n#@       ###\n## $ $   ..#\n############",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 6",
                        Width = 8,
                        Height = 6,
                        MapData = "####### \n#..   # \n# #$# ##\n# $  $ #\n## .@  #\n #######",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 7",
                        Width = 17,
                        Height = 18,
                        MapData = "          #######\n          #  ...#\n      #####  ...#\n      #      ...#\n      #  ##  ...#\n      ## ##  ...#\n     ### ########\n     # $$$ ##    \n #####  $ $ #####\n##   #$ $   #   #\n#@ $  $    $  $ #\n###### $$ $ #####\n     # $    #    \n     #### ###    \n        #  #     \n        #  #     \n        #  #     \n        ####     ",
                        IsDefault = true
                    },
                    new Level
                    {
                        Name = "Уровень 8",
                        Width = 21,
                        Height = 20,
                        MapData = "              ####   \n         ######  #   \n         #       #   \n         #  #### ### \n ###  ##### ###    # \n##@####   $$$ #    # \n# $$   $$ $   #....##\n#  $$$#    $  #.....#\n# $   # $$ $$ #.....#\n###   #  $    #.....#\n  #   # $ $ $ #.....#\n  # ####### ###.....#\n  #   #  $ $  #.....#\n  ### # $$ $ $#######\n    # #  $      #    \n    # # $$$ $$$ #    \n    # #       # #    \n    # ######### #    \n    #           #    \n    #############    ",
                        IsDefault = true
                    }
                ];
                Levels.AddRange(defaultLevels);
                SaveChanges();
            }
        }
    }
}