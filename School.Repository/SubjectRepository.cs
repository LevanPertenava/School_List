using School.Models;

namespace School.Repository
{
    public class SubjectRepository : RepositoryBase<Subject>
    {
        public SubjectRepository(string connectionString) :
            base(connectionString)
        {
        }
    }
}
