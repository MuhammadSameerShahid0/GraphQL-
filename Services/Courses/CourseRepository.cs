using GraphQLDemo.DTOs;
using GraphQLDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.Services.Courses
{
    public class CourseRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _ContextFactory;
        public CourseRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            _ContextFactory = contextFactory;
        }

        public async Task<IEnumerable<CourseDTO>> GetAll()
        {
            using (SchoolDbContext context = _ContextFactory.CreateDbContext())
            {
                return await context.Courses
                    //.Include(c => c.Instructor)
                    //.Include(c => c.Students)
                    .ToListAsync();
            }
        }
        public async Task<CourseDTO> GetById(Guid courseId)
        {
            using (SchoolDbContext Context = _ContextFactory.CreateDbContext())
            {
                return await Context.Courses
                    //.Include(c => c.Instructor)
                    //.Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == courseId);
            }
        }
        public async Task<CourseDTO> Create(CourseDTO course)
        {
            using (SchoolDbContext context = _ContextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }
        public async Task<CourseDTO> Update(CourseDTO course)
        {
            using (SchoolDbContext Context = _ContextFactory.CreateDbContext())
            {
                Context.Courses.Update(course);
                await Context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (SchoolDbContext Context = _ContextFactory.CreateDbContext())
            {
                CourseDTO course = new CourseDTO();
                { course.Id = id; }
                Context.Courses.Remove(course);
                return await Context.SaveChangesAsync() > 0;

            }
        }

    }
}
