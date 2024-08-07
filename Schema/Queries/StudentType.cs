using HotChocolate;
using System;

namespace GraphQLDemo.Schema.Queries
{
    public class StudentType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [GraphQLName("gPA")]
        public double GPA { get; set; }
    }
}
