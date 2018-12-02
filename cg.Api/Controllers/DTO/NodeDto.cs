using System.Collections.Generic;

namespace cg.Api.Controllers.DTO
{
    public class NodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NodeTypeId { get; set; }

        public string NodeTypeName { get; set; }
        public decimal Page { get; set; }
        public bool IsActive { get; set; }
        public List<int> ParentNodes { get; set; }
        //public List<ParentNodeDto> ParentNodes { get; set; }
        
    }
}