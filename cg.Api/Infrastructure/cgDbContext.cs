using cg.Api.Core;
using Microsoft.EntityFrameworkCore;

namespace cg.Api.Infrastructure
{
    public class cgDbContext : DbContext
    {
        public cgDbContext(DbContextOptions options) : base(options)
        {

        }

        #region NodeModule

        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeType> NodeTypes { get; set; }
        public DbSet<NodeRelation> NodeRelations { get; set; }
        public DbSet<NodeDescription> NodeDescriptions { get; set; }


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.ToLower();
                }
                entity.Relational().TableName = entity.Name.ToLower();
            }


            modelBuilder
                .MapNode();
        }
    }
}