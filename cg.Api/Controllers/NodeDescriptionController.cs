using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using cg.Api.Controllers.DTO;
using cg.Api.Core;
using cg.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cg.Api.Controllers
{
  
    [Route("api/NodeDescriptions")]
    public class NodeDescriptionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly cgDbContext _cgDbContext;

        public NodeDescriptionController(IMapper mapper,cgDbContext cgDbContext)
        {
            _mapper = mapper;
            _cgDbContext = cgDbContext;
        }
       
        [HttpPost]
        [ProducesResponseType(typeof(NodeDescription), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult PostDescription([FromBody]NodeDescription value)
        {
            try
            {
                if (value.Id == 0)
                {
                    BadRequest("Id cannot be 0(Zero)");
                }
                var nodeDescription = _cgDbContext.NodeDescriptions.FirstOrDefault(s => s.Id == value.Id);
                if (nodeDescription == null)
                {
                    nodeDescription = new NodeDescription()
                    {
                        Id = value.Id,
                    };
                    _cgDbContext.NodeDescriptions.Add(nodeDescription);
                }

                nodeDescription.Title = value.Title;
                nodeDescription.Description = value.Description;
                nodeDescription.IsCondition = value.IsCondition;


                _cgDbContext.SaveChanges();
                return Ok(nodeDescription);
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot save data");
            }

        }

        [HttpGet("{nodeId}")]
        [ProducesResponseType(typeof(NodeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetNodeDescription(int nodeid)
        {
            try
            {

                return Ok(_cgDbContext.NodeDescriptions.FirstOrDefault(s => s.Id == nodeid));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<NodeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetNodes(string code, int? page)
        {
            try
            {
                if (String.IsNullOrEmpty(code))
                {
                    return Ok(_mapper.Map<List<NodeDto>>(_cgDbContext.Nodes.Include(i => i.NodeType)).Where(s => !page.HasValue || s.Page == page.Value));
                }
                return Ok(_mapper.Map<List<NodeDto>>(_cgDbContext.Nodes.Include(i => i.NodeType).Where(s => s.NodeType.NodeTypeCode == code && (!page.HasValue || s.Page == page.Value))));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

    }
}
