using GraphQLDemo.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace GraphQLDemo.Services.Instructor
{
    public class InstructorRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _ContextFactory;
        public InstructorRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            _ContextFactory = contextFactory;
        }
        public async Task<InstructorDTO> GetById(Guid instructorId)
        {
            using (SchoolDbContext Context = _ContextFactory.CreateDbContext())
            {
                return await Context.Instructors.FirstOrDefaultAsync(c => c.Id == instructorId);
            }
        }
        public async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDbContext Context = _ContextFactory.CreateDbContext())
            {
                return await Context.Instructors
                    .Where(i => instructorIds.Contains(i.Id))
                    .ToListAsync();
            }
        }
    }
}
