using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository
{
    public interface IPerson_Has_Need_Skill_Repo
    {
        public Task<IdentityResult> Add_Person_Has_Skills_By_Id_Async(int Person_Id, int Skill_Id, CancellationToken cancellationToken);
        //returns the all the skills a person Has
        public Task<IEnumerable<SkillModel>> Get_Person_Has_Skills_By_Id_Async(int Person_Id, CancellationToken cancellationToken);
        public Task<IdentityResult> Add_Person_Need_Skills_By_Id_Async(int Person_Id, int Skill_Id, CancellationToken cancellationToken);   
        //returns all the skills a person Needs
        public Task<IEnumerable<SkillModel>> Get_Person_Need_Skills_By_Id_Async(int Person_Id, CancellationToken cancellationToken);
    }
}
