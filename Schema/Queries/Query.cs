using HotChocolate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HotChocolate.Types;
using HotChocolate.Data;
using GraphQLDemo.Services;
using GraphQLDemo.Schema.Sorting;
using GraphQLDemo.Services.Courses;
using GraphQLDemo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Schema.Queries
{

    public class Query
    {
        //private readonly Faker<InstructorType> _InstructorFaker;
        //private readonly Faker<StudentType> _StudentFaker;
        //private readonly Faker<CourseType> _CourseFaker;

        //public Query()
        //{
        //    _InstructorFaker = new Faker<InstructorType>()
        //         .RuleFor(c => c.Id, f => Guid.NewGuid())
        //         .RuleFor(c => c.FirstName, f => f.Name.FirstName())
        //         .RuleFor(c => c.LastName, f => f.Name.LastName())
        //         .RuleFor(c => c.Salary, f => f.Random.Double(0, 1000));

        //    _StudentFaker = new Faker<StudentType>()
        //        .RuleFor(c => c.Id, f => Guid.NewGuid())
        //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
        //        .RuleFor(c => c.LastName, f => f.Name.LastName())
        //        .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));

        //    _CourseFaker = new Faker<CourseType>()
        //        .RuleFor(c => c.Id, f => Guid.NewGuid())
        //        .RuleFor(c => c.Name, f => f.Name.JobTitle())
        //        .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
        //        .RuleFor(c => c.Instructor, f => _InstructorFaker.Generate())
        //        .RuleFor(c => c.Students, f => _StudentFaker.Generate(3));

        //}

        private readonly CourseRepository _courseRepository;

        public Query(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            IEnumerable<CourseDTO> courseDTOs = await _courseRepository.GetAll();
            return courseDTOs.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = c.Instructor.Id,
                //    FirstName = c.Instructor.FirstName,
                //    LastName = c.Instructor.LastName,
                //    Salary = c.Instructor.Salary,
                //}
            });
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [UseFiltering]
        [UseSorting(typeof(CourseSort))]
        public IQueryable<CourseType> GetPaginatedCourses([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = c.Instructor.Id,
                //    FirstName = c.Instructor.FirstName,
                //    LastName = c.Instructor.LastName,
                //    Salary = c.Instructor.Salary,
                //}
            });
        }

        [UseOffsetPaging(IncludeTotalCount = true)]
        public async Task<IEnumerable<CourseType>> GetOffsetCourses()
        {
            IEnumerable<CourseDTO> courseDTOs = await _courseRepository.GetAll();
            return courseDTOs.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = c.Instructor.Id,
                //    FirstName = c.Instructor.FirstName,
                //    LastName = c.Instructor.LastName,
                //    Salary = c.Instructor.Salary,
                //}
            });
        }

        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            //await Task.Delay(1000);
            //CourseType course = _CourseFaker.Generate();
            //course.Id = id;
            //return course;

            CourseDTO courseDTO = await _courseRepository.GetById(id);
            return new CourseType()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId= courseDTO.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = courseDTO.Instructor.Id,
                //    FirstName = courseDTO.Instructor.FirstName,
                //    LastName = courseDTO.Instructor.LastName,
                //    Salary = courseDTO.Instructor.Salary,
                //}
            };
        }
        
        [GraphQLDeprecated("This Query is Deprecated")]
        public string Instructions => "Try to learn GraphQL";

    }
}
