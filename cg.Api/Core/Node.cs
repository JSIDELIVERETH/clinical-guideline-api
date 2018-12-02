using System;
using System.Collections.Generic;

namespace cg.Api.Core
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NodeTypeId { get; set; }
        public decimal Page { get; set; }
        public bool IsActive { get; set; }
        public Guid rowguid { get; set; }

        public NodeType NodeType { get; set; }
        public List<NodeRelation> NodeRelation { get; set; }
    }
}
