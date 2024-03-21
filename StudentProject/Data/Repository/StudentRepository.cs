
using Microsoft.EntityFrameworkCore;

namespace StudentProject.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDB _studentDB;
        public StudentRepository(StudentDB studentDB)
        {

            _studentDB = studentDB;

        }
        public async Task<int> CreateStudentAsync(Student student)
        {
            _studentDB.Students.Add(student);
            await _studentDB.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(Student student)
        {
            
            _studentDB.Students.Remove(student);
            await _studentDB.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentDB.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id, bool useNoTracking = false)
        {
            if (useNoTracking)
                return await _studentDB.Students.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
            else
                return await _studentDB.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            _studentDB.Students.Update(student);
            await _studentDB.SaveChangesAsync();
            return student.Id;
        }
    }
}
