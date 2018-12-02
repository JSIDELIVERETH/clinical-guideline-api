using cg.Api.Core;
using Microsoft.EntityFrameworkCore;

namespace cg.Api.Infrastructure
{
    public static class NodeMapping
    {
        public static ModelBuilder MapNode(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>(e =>
            {
                e.ToTable("node", "cg");
                e.HasKey(f => f.Id);
                e.HasOne(p => p.NodeType)
                    .WithMany()
                    .HasForeignKey(f=>f.NodeTypeId);
                e.HasMany(p => p.NodeRelation)
                    .WithOne(p=>p.ChildNode)
                    .HasForeignKey(f => f.ChildNodeId);
            });

            modelBuilder.Entity<NodeType>(e =>
            {
                e.ToTable("nodetype", "cg");
                e.HasKey(f => f.Id);
            });

            modelBuilder.Entity<NodeRelation>(e =>
            {
                e.ToTable("noderelation", "cg");
                e.HasKey(f => f.Id);
                e.HasOne(p => p.ParentNode)
                    .WithMany()
                    .HasForeignKey(f => f.ParentNodeId);
            });

            modelBuilder.Entity<NodeDescription>(e =>
            {
                e.ToTable("nodedescription", "cg");
                e.HasKey(f => f.Id);
            });
            return modelBuilder;
        }
    }
}