using AutoMapper;
using GameOfLuck.Context;
using GameOfLuck.Entities;
using GameOfLuck.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLuck.Services
{
    public class PersonService : IPersonService
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PersonService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public void Create(PersonDTO model)
        {
            try
            {
                Person person = new Person();
                person =  mapper.Map<Person>(model);

                context.Persons.Add(person);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Update(PersonDTO model)
        {
            try
            {
                Person person = new Person();
                person = mapper.Map<Person>(model);

                context.Persons.Update(person);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public int AssignInitialPoints()
        {

            return 10000;
        }

        public string validateNewUser(PersonDTO person)
        {
            string ret = "";
            if (string.IsNullOrEmpty(person.Name))
            {
                ret = "Name must not be empty";
            }
            if (string.IsNullOrEmpty(person.Surname))
            {
                ret = "Surname must not be empty";
            }

            return ret;
        }

        public PersonDTO GetPersonByEmail(string email)
        {
            PersonDTO ret = new PersonDTO();
            Person person = context.Persons.FirstOrDefault(x => x.Email == email);

            context.Entry(person).State = EntityState.Detached;

            if (person != null)
            {
                ret = mapper.Map<PersonDTO>(person);
            }

            return ret;
        }
    }
}
