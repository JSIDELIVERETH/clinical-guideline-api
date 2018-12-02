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
  
    [Route("api/Sync")]
    public class SyncController : Controller
    {
        private readonly IMapper _mapper;
        private readonly cgDbContext _cgDbContext;

        public SyncController(IMapper mapper,cgDbContext cgDbContext)
        {
            _mapper = mapper;
            _cgDbContext = cgDbContext;
        }

        #region Sync

        [HttpGet("pages")]
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
         
        [HttpGet("nodes")]
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

        [HttpGet("NodeTypes")]
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

        [HttpGet("relation")]
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
