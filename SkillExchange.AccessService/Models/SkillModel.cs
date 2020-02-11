using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class SkillModel
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
