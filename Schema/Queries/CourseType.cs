using GraphQLDemo.DataLoader;
using GraphQLDemo.DTOs;
using GraphQLDemo.Models;
using GraphQLDemo.Services.Instructor;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Schema.Queries
{
    public class CourseType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorRepository)
        {
            InstructorDTO instructorDTO = await instructorRepository.LoadAsync(InstructorId , CancellationToken.None);
            return new InstructorType
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };
        }

        public IEnumerable<StudentType> Students { get; set; }

        public string Description()
        {
            return $"{Name} : This is a course";

        }
    }
}
