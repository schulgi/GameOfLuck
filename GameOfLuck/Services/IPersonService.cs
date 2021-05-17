using GameOfLuck.Entities;
using GameOfLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Services
{
   public interface IPersonService
    {
        void Create(NewPersonDTO model);
        int AssignInitialPoints();
        string validateNewUser(NewPersonDTO person);
        PersonDTO GetPersonByEmail(string email);
       void Update(PersonDTO model);
    }
}
