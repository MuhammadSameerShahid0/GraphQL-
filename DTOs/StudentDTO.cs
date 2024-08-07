using GraphQLDemo.DTOs;
using HotChocolate;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double GPA { get; set; }

        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
