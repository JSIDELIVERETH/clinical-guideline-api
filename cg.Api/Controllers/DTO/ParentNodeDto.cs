using System;

namespace cg.Api.Controllers.DTO
{
    public class ParentNodeDto
    {
        public int Id { get; set; }
        public int ParentNodeId { get; set; }
        public String ParentNodeName { get; set; }
        public bool IsActive { get; set; }
    }
}