using GraphQLDemo.Models;
using GraphQLDemo.Schema.Mutation;
using GraphQLDemo.Schema.Queries;
using GraphQLDemo.Services.Courses;
using HotChocolate;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQLDemo.DTOs;
using HotChocolate.AspNetCore.Authorization;
using GraphQLDemo.Schema.Subscriptions;
using Microsoft.AspNetCore.Components;
using GraphQLDemo.FluentValidation;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation.Results;
using FluentValidation;

namespace GraphQLDemo.Schema.Mutation
{
    public class Mutation
    {
        //private readonly List<CourseResult> _courses;
        //public Mutation()
        //{
        //    _courses = new List<CourseResult>();
        //}

        private readonly CourseRepository _courseRepository;
        //private readonly CourseTypeValidation _validationRules;
        public Mutation(CourseRepository courseRepository/*, CourseTypeValidation validationRules*/)
        {
            _courseRepository = courseRepository;
            //_validationRules = validationRules;
        }

        //private string CreateToken(CourseType courseType)
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, courseType.Name),
        //        new Claim(ClaimTypes.Role, "Admin")
        //    };
        //    SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        //        _configuration.GetSection("Jwt:Key").Value));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    JwtSecurityToken token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds);
        //    string jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwt;
        //}

        //private void Validate(CourseType courseType)
        //{
        //    ValidationResult validationResult = _validationRules.Validate(courseType);
        //    if (!validationResult.IsValid)
        //    {
        //        throw new GraphQLException("Invalid Input");
        //    }            
        //}

        //[Authorize(Roles = new[] { "Admin" })]
        public async Task<CourseResult> CreateCourse(
           [UseFluentValidation , UseValidator(typeof(CourseTypeValidation))]
            string name
           ,Subject subject
           ,Guid instructorid
           ,[Service] ITopicEventSender topicEventSender)
        {
            //CourseType courseType = new CourseType();
            //courseType.Name = name;
            //Validate(courseType);

            CourseDTO courseDTO = new CourseDTO()
            {
                Name = name,
                Subject = subject,
                InstructorId = instructorid,
            };

            courseDTO = await _courseRepository.Create(courseDTO);

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);
            //CourseType courseType = new CourseType();
            //courseType.Name = name;
            //CreateToken(courseType);

            return course;
        }

        public async Task<CourseResult> UpdateCourse(
            [UseFluentValidation , UseValidator(typeof(CourseTypeValidation))]
            string name
            ,Guid id
            ,Subject subject
            ,Guid instructorid
            ,[Service] ITopicEventSender topicEventSender)
        {
            //CourseResult course = _courses.FirstOrDefault(c => c.Id == id);
            //if (course == null)
            //{
            //    throw new GraphQLException(new Error("Course Not Found" , "COURSE_NOT_FOUND"));
            //}
            //course.Name = name;
            //course.Subject = subject;
            //course.InstructorId = instructorid;
            CourseDTO courseDTO = new CourseDTO()
            {
                Id = id,
                Name = name,
                Subject = subject,
                InstructorId = instructorid,
            };

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };
            await _courseRepository.Update(courseDTO);

            string UpdateCourseTopic = $"{course.Id}_{(nameof(Subscription.CourseUpdate), course)}";
            await topicEventSender.SendAsync(UpdateCourseTopic, course);

            return course;
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            //CourseResult course = _courses.FirstOrDefault(c => c.Id==id);

            //if (course == null) { throw new GraphQLException(new Error($"Against this id : {id} , Course Not Found")); }
            //_courses.Remove(course);

            //return course;

            //return _courses.RemoveAll(c => c.Id == id) >= 1;
            try
            {
                return await _courseRepository.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
