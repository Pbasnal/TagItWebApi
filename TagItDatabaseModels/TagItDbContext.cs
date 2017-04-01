using System.Data.Entity;
using TagItDatabaseModels.Tables;

namespace TagItDatabaseModels
{
    public class TagItDbContext : DbContext
    {
        public TagItDbContext() : base("name=tagItDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TagItDbContext, TagItDatabaseModels.Migrations.Configuration>("tagItDBConnectionString"));
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentType> CommentTypes { get; set; }
        public DbSet<Hotspot> Hotspots { get; set; }
        public DbSet<HotspotTag> HotspotTags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
