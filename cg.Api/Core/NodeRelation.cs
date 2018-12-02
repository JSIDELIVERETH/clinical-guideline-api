namespace cg.Api.Core
{
    public class NodeRelation
    {
        public int Id { get; set; }
        public int ChildNodeId { get; set; }
        public int ParentNodeId { get; set; }
        public Node ChildNode { get; set; }
        public Node ParentNode { get; set; }
        public bool IsActive { get; set; }
    }
}