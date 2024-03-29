﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudentProject.Data;
using StudentProject.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentProject.Data.Repository;
using Microsoft.AspNetCore.Cors;

namespace StudentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        //private readonly IStudentRepository _studentRepository;
        private readonly ICollegeRepository<Student> _studentRepository;
        public StudentController(IMapper mapper, ICollegeRepository<Student> studentRepository) 
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            
        }

        [HttpGet]
        [Route("All")]
        [EnableCors(PolicyName = "AllowOnlyLocalHost")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudentAsync() {
            var q = await _studentRepository.GetAllAsync();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(q);
            return Ok(studentDTOs);
        }
        [HttpGet]
        [Route("{id:int}", Name = "GetId")]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Vui long nhap id > 1");
            }
            var q = await _studentRepository.GetStudentByIdAsync(student => student.Id == id);
            if (q == null)
            {
                return NotFound($"Khong tim thay sinh vien co id la {id} trong Database");
            }
            var c = _mapper.Map<StudentDTO>(q);
            return Ok(c);
        }
        [HttpPost]
        [Route("Create", Name = "CreateS")]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest("Khong duoc de thong tin trong");
            }
            Student S = _mapper.Map<Student>(studentDTO);
            var studentAfterCreation =  await _studentRepository.CreateStudentAsync(S);

            studentDTO.Id = studentAfterCreation.Id;
            
            return CreatedAtRoute("GetId", new { Id = studentDTO.Id }, S);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<Student>> UpdateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null || studentDTO.Id <= 0)
            {
                return BadRequest();
            }
            var student = await _studentRepository.GetStudentByIdAsync(student => student.Id == studentDTO.Id, true);
            if (student == null)
            {
                return NotFound("Khong tim thay du lieu sinh vien");
            }
            var Q = _mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateStudentAsync(Q);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStudentById")]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Vui long nhap Id >= 1");
            }
            var q = await _studentRepository.GetStudentByIdAsync(student => student.Id == id);
            if (q == null)
            {
                return NotFound($"Khong tim thay sinh vien co id la {id} trong Database");
            }
            await _studentRepository.DeleteStudentAsync(q);
            return Ok(true);
        }
    }
}
