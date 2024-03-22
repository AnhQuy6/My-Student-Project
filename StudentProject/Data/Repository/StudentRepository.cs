
using Microsoft.EntityFrameworkCore;

namespace StudentProject.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDB _dbContext;
        public StudentRepository(StudentDB dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task<int> CreateStudentAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(Student student)
        {
            
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id, bool useNoTracking = false)
        {
            if (useNoTracking)
                return await _dbContext.Students.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
            else
                return await _dbContext.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
