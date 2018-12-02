using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cg.Api.Core
{
    public class NodeDescription
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public bool IsCondition { get; set; }
    }
}
