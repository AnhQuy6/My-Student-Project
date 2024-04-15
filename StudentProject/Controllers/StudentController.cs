using AutoMapper;
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
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Linq.Expressions;

namespace StudentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowAll")]
    [Authorize(AuthenticationSchemes = "LoginForLocalUsers", Roles = "Superadmin,Admin")]
    //[AllowAnonymous]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        //private readonly IStudentRepository _studentRepository;
        private readonly ICollegeRepository<Student> _studentRepository;
        private APIResponse _apiResponse;

        public StudentController(IMapper mapper, ICollegeRepository<Student> studentRepository, APIResponse apiResponse) 
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse = new(); // new APIResponse();
        }
        
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[EnableCors(PolicyName = "AllowOnlyLocalHost")]
        public async Task<ActionResult<APIResponse>> GetAllStudentAsync() 
        {
            try
            {
                var q = await _studentRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(q);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
            
        }
        [HttpGet]
        [Route("{id:int}", Name = "GetId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentById(int id)
        {
            try
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
                _apiResponse.Data = _mapper.Map<StudentDTO>(q);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
            
        }
        [HttpPost]
        [Route("Create", Name = "CreateS")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            try
            {
                if (studentDTO == null)
                {
                    return BadRequest("Khong duoc de thong tin trong");
                }
                Student S = _mapper.Map<Student>(studentDTO);
                var studentAfterCreation = await _studentRepository.CreateStudentAsync(S);

                studentDTO.Id = studentAfterCreation.Id;

                _apiResponse.Data = studentDTO;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetId", new { Id = studentDTO.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
            
        }
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateStudent([FromBody] StudentDTO studentDTO)
        {
            try
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
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
            
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteStudent(int id)
        {
            try
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
                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }
    }
}
