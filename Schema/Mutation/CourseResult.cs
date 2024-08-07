using GraphQLDemo.Models;
using GraphQLDemo.Schema.Queries;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.Schema.Mutation
{
    public class CourseResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }

    }
}
