using System.Linq;
using AutoMapper;
using cg.Api.Core;

namespace cg.Api.Controllers.DTO.Mapping
{
    public class NodeProfile:Profile
    {
        public NodeProfile()
        {
            CreateMap<Node, NodeDto>()
              .ForMember(m => m.ParentNodes, o => o.MapFrom(s => s.NodeRelation.Where(w=>w.IsActive).Select(t=>t.ParentNodeId)));
            CreateMap<NodeRelation, ParentNodeDto>()
                .ReverseMap();
        }

    }
}
