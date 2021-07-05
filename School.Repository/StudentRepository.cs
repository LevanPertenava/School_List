using School.Models;

namespace School.Repository
{
    public class StudentRepository : RepositoryBase<Student>
    {
        public StudentRepository(string connectionString) :
            base(connectionString)
        {
        }
    }
}
