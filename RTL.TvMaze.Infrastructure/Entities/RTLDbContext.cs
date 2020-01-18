using Microsoft.EntityFrameworkCore;

namespace RTL.TvMaze.Infrastructure.Entities
{
    public partial class RTLDbContext : DbContext
    {
        public RTLDbContext()
        {
        }

        public RTLDbContext(DbContextOptions<RTLDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TvMazePerson> TvMazePerson { get; set; }
        public virtual DbSet<TvMazeShow> TvMazeShow { get; set; }
        public virtual DbSet<TvMazeShowCast> TvMazeShowCast { get; set; }
        public virtual DbSet<TvMazeShowIndex> TvMazeShowIndex { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<TvMazePerson>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.TvMazeId })
                    .HasName("PK_TV_MAZE_PERSON");

                entity.ToTable("tv_maze_person");

                entity.HasIndex(e => e.TvMazeId)
                    .HasName("UQ__tv_maze___687565F579C1B367")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TvMazeId).HasColumnName("tv_maze_id");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TvMazeShow>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TV_MAZE_SHOW")
                    .IsClustered(false);

                entity.ToTable("tv_maze_show");

                entity.HasIndex(e => e.TvMazeId)
                    .HasName("UQ__tv_maze___687565F56C43D7CC")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.TvMazeId).HasColumnName("tv_maze_id");
            });

            modelBuilder.Entity<TvMazeShowCast>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TV_MAZE_SHOW_CAST")
                    .IsClustered(false);

                entity.ToTable("tv_maze_show_cast");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TvMazePersonId).HasColumnName("tv_maze_person_id");

                entity.Property(e => e.TvMazeShowId).HasColumnName("tv_maze_show_id");

                entity.HasOne(d => d.TvMazePerson)
                    .WithMany(p => p.TvMazeShowCast)
                    .HasPrincipalKey(p => p.TvMazeId)
                    .HasForeignKey(d => d.TvMazePersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TV_MAZE_SHOW_CAST_TVMAZE_PEOPLE");

                entity.HasOne(d => d.TvMazeShow)
                    .WithMany(p => p.TvMazeShowCast)
                    .HasPrincipalKey(p => p.TvMazeId)
                    .HasForeignKey(d => d.TvMazeShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TV_MAZE_SHOW_CAST_TVMAZE_SHOW");
            });

            modelBuilder.Entity<TvMazeShowIndex>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TV_MAZE_SHOW_INDEX")
                    .IsClustered(false);

                entity.ToTable("tv_maze_show_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.InProgress).HasColumnName("in_progress");

                entity.Property(e => e.RecordsCreated).HasColumnName("records_created");

                entity.Property(e => e.RecordsUpdated).HasColumnName("records_updated");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime");
            });
        }
    }
}
