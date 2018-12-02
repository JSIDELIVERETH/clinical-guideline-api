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
  
    [Route("api/Nodes")]
    public class NodeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly cgDbContext _cgDbContext;

        public NodeController(IMapper mapper,cgDbContext cgDbContext)
        {
            _mapper = mapper;
            _cgDbContext = cgDbContext;
        }
        [HttpGet("NodeTypes")]
        [ProducesResponseType(typeof(IList<NodeType>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetNodeType()
        {
            try
            {
                return Ok(_cgDbContext.NodeTypes.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

   
        [HttpGet("{nodeId}")]
        [ProducesResponseType(typeof(NodeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetNode(int nodeid)
        {
            try
            {
                
                    return Ok(_mapper.Map<NodeDto>(_cgDbContext.Nodes.Include(i => i.NodeType).Include(i => i.NodeRelation).ThenInclude(i => i.ParentNode).FirstOrDefault(s=>s.Id ==nodeid)));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

        [HttpGet("{nodeId}/RelatedNodes")]
        [ProducesResponseType(typeof(IList<NodeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetNodeByParent(int nodeId,bool? isActive)
        {
            try
            {
                var parent = _cgDbContext.Nodes.FirstOrDefault(s => s.Id == nodeId);
                return Ok(_mapper.Map<List<NodeDto>>(_cgDbContext.Nodes.Include(s=>s.NodeType).Where(s => s.Page == parent.Page && (!isActive.HasValue ||s.IsActive))));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(NodeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult Post(int? parentId, [FromBody]NodeDto value)
        {
            try
            {
                var node = _cgDbContext.Nodes.Include(s=>s.NodeRelation).FirstOrDefault(s => s.Id == value.Id);
                if(node==null)
                {
                    node = new Node();
                    node.rowguid = new Guid();
                    node.IsActive = true;
                    _cgDbContext.Nodes.Add(node);
                }

                node.Page = value.Page;
                node.NodeTypeId = value.NodeTypeId;
                node.Name = value.Name;
                //node.IsActive = value.IsActive;

                if (node.NodeRelation == null)
                {
                    node.NodeRelation = new List<NodeRelation>();
                }

                if (value.ParentNodes == null)
                {
                    value.ParentNodes = new List<int>();
                }
                foreach (var pr in value.ParentNodes)
                {
                    var parentNode = node.NodeRelation.FirstOrDefault(s => s.ParentNodeId == pr);
                    if (parentNode == null)
                    {
                        parentNode = new NodeRelation()
                        {
                            ParentNodeId = pr


                        };

                        node.NodeRelation.Add(parentNode);
                    }
                    parentNode.IsActive = true;

                }

                foreach (var parent in node.NodeRelation)
                {
                    if (!value.ParentNodes.Contains(parent.ParentNodeId))
                    {
                        parent.IsActive = false;
                    }
                }
                _cgDbContext.SaveChanges();
                return Ok(_mapper.Map<NodeDto>(node));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot save data");
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

        #region Sync

        [HttpGet("sync/pages")]
        [ProducesResponseType(typeof(int[]), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult Getsyncpage()
        {
            try
            {

                return Ok(_cgDbContext.Nodes.Select(s => s.Page).Distinct().ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }
         
        [HttpGet("sync/nodes")]
        [ProducesResponseType(typeof(IList<Node>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetsyncNodes(int page)
        {
            try
            {

                return Ok(_cgDbContext.Nodes.Include(i => i.NodeType).Where(s => s.Page == page));
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

        [HttpGet("/sync/NodeTypes")]
        [ProducesResponseType(typeof(IList<NodeType>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetsyncNodeTypes()
        {
            try
            {
                return Ok(_cgDbContext.NodeTypes.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }

        [HttpGet("/sync/relation")]
        [ProducesResponseType(typeof(IList<NodeRelation>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult Getsyncrelation(int page)
        {
            try
            {

                return Ok(_cgDbContext.NodeRelations.Where(p => p.ParentNode.Page == page).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured cannot fetch data");
            }
        }
        #endregion

    }
}
