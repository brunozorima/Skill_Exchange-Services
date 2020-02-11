using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Repository;

namespace SkillExchange.AccessService.Services.SkillService
{
    public class Person_Has_Need_Skill_Service : IPerson_Has_Need_Skill_Service
    {
        private readonly IPerson_Has_Need_Skill_Repo _person_Has_Need_Skill_Repo;
        public Person_Has_Need_Skill_Service(IPerson_Has_Need_Skill_Repo person_Has_Need_Skill_Repo)
        {
            this._person_Has_Need_Skill_Repo = person_Has_Need_Skill_Repo;
        }
        public async Task<IdentityResult> AddPersonHasSkillById(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            return await this._person_Has_Need_Skill_Repo.Add_Person_Has_Skills_By_Id_Async(Person_Id, Skill_Id, cancellationToken);
        }
        public async Task<IEnumerable<SkillModel>> GetPersonHasSkillById(int Person_Id, CancellationToken cancellationToken)
        {
            return await this._person_Has_Need_Skill_Repo.Get_Person_Has_Skills_By_Id_Async(Person_Id, cancellationToken);
        }
    }
}
