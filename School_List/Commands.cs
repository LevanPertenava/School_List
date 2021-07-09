using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Models;
using School.Repository;
using System.Data.SqlClient;
using DatabaseHelper;
using System.Data;

namespace School_List
{
    class Commands
    {
        private string _connection;
        private Database<SqlConnection> _database;
        private DataSet _dataSet;
        public Commands(string connectionString)
        {
            _connection = connectionString;
            _database = new Database<SqlConnection>(connectionString);
            _dataSet = _database.GetTables("GetAllTables_SP", CommandType.StoredProcedure);
            _dataSet.Tables[0].TableName = "Students";
            _dataSet.Tables[1].TableName = "Subjects";
            _dataSet.Tables[2].TableName = "StudentsToSubjects";
        }

        public void AddPerson(Student student)
        {
            if (student.Age > 100)
            {
                Console.WriteLine("Invalid Age");
                return;
            }
            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.LastName))
            {
                Console.WriteLine("Name or Lastname must be filled");
                return;
            }
            if (student.PersonalId < 4)
            {
                Console.WriteLine("Invalid Personal ID");
                return;
            }
            if (IsStudentAvailable(student.PersonalId))
            {
                Console.WriteLine("this Id is already registered.");
                return;
            }

            StudentRepository studentRepository = new(_connection);
            studentRepository.Insert(student);
        }

        public void RemovePerson(int personalId)
        {
            var id = GetStudentId(personalId);
            if (id == 0)
            {
                Console.WriteLine("Student ID was not found");
                return;
            }

            StudentRepository studentRepository = new(_connection);
            studentRepository.Delete(personalId);
        }

        public void AddSubject(string subjectName)
        {
            if (string.IsNullOrEmpty(subjectName))
            {
                Console.WriteLine("Subject name must be filled");
                return;
            }
            if (isSubjectAvailable(subjectName))
            {
                Console.WriteLine("This subject is already registered");
                return;
            }

            SubjectRepository subjectRepository = new(_connection);
            subjectRepository.Insert(new Subject() { SubjectName = subjectName });
        }

        public void AddPersonToSubject(int personalId, string subjectName)
        {
            int studentId = GetStudentId(personalId);
            int subjectId = GetSubjectId(subjectName);

            if (isStudentToSubjectAvailable(studentId, subjectId))
            {
                Console.WriteLine("This record already exists");
                return;
            }
            SqlParameter[] sqlParameters = { new SqlParameter() { ParameterName = "@StudentId", SqlDbType = SqlDbType.Int, Value = studentId },
                                             new SqlParameter() {ParameterName = "@SubjectId", SqlDbType = SqlDbType.Int, Value = subjectId }
                                            };
            _database.ExecuteNonQuery("InsertStudentsSubjects_SP", CommandType.StoredProcedure, sqlParameters);
        }

        public void SetPoint(int personalId, string subjectName, byte grade)
        {
            int studentId = GetStudentId(personalId);
            int subjectId = GetSubjectId(subjectName);

            if (grade > 10)
            {
                Console.WriteLine("Can't be evaluated by more than 10 points ");
                return;
            }
            if (!isStudentToSubjectAvailable(studentId, subjectId))
            {
                Console.WriteLine("Record was not found");
                return;
            }

            SqlParameter[] sqlParameters = { new SqlParameter() { ParameterName = "@studentId", SqlDbType = SqlDbType.Int, Value = studentId },
                                             new SqlParameter() {ParameterName = "@subjectId", SqlDbType = SqlDbType.Int, Value = subjectId },
                                             new SqlParameter() {ParameterName = @"Grade", SqlDbType = SqlDbType.TinyInt, Value = grade}
                                            };
            _database.ExecuteNonQuery("SetPoint_SP", CommandType.StoredProcedure, sqlParameters);
        }

        public void GetPersonPoint(int personalId)
        {
            var studentToSubjects = _dataSet.Tables["StudentsToSubjects"];
            var subjects = _dataSet.Tables["Subjects"];
            var students = _dataSet.Tables["Students"];

            var sub = (from subject in subjects.AsEnumerable()
                       join sts in studentToSubjects.AsEnumerable()
                       on subject.Field<int>("id") equals sts.Field<int>("SubjectId") 
                       join student in students.AsEnumerable()
                       on sts.Field<int>("StudentId") equals GetStudentId(personalId)
                       select new
                       {
                           SubjectName = subject.Field<string>("SubjectName"),
                           Grade = sts.Field<byte>("Grade")
                       }).Distinct();

            foreach (var item in sub)
            {
                Console.WriteLine($"{item.SubjectName} : {item.Grade}");
            }
        }

        public void GetPersonPoint(int personalId, string subjectName)
        {
            var studentToSubjects = _dataSet.Tables["StudentsToSubjects"];
            var subjects = _dataSet.Tables["Subjects"];
            var students = _dataSet.Tables["Students"];

            var sub = (from sts in studentToSubjects.AsEnumerable() 
                       join subject in subjects.AsEnumerable()
                       on sts.Field<int>("SubjectId") equals GetSubjectId(subjectName)
                       join student in students.AsEnumerable()
                       on sts.Field<int>("StudentId") equals GetStudentId(personalId)
                       select new
                       {
                           SubjectName = subjectName,
                           Grade = sts.Field<byte>("Grade")
                       }).Distinct();

            foreach (var item in sub)
            {
                Console.WriteLine($"{item.SubjectName} : {item.Grade}");
            }
        }

        public void GetAllPersonsPoints(string subjectName)
        {
            var studentToSubjects = _dataSet.Tables["StudentsToSubjects"];
            var subjects = _dataSet.Tables["Subjects"];
            var students = _dataSet.Tables["Students"];

            var sub = (from sts in studentToSubjects.AsEnumerable()
                       join subject in subjects.AsEnumerable()
                       on sts.Field<int>("SubjectId") equals GetSubjectId(subjectName)
                       join student in students.AsEnumerable()
                       on sts.Field<int>("StudentId") equals student.Field<int>("Id")
                       select new
                       {
                           Name =student.Field<string>("Name"),
                           LastName = student.Field<string>("LastName"),
                           Grade = sts.Field<byte>("Grade")
                       }).Distinct();

            foreach (var item in sub)
            {
                Console.WriteLine($"{item.Name} {item.LastName} : {item.Grade}");
            }
        }

        public void GetAllPersonsPoints()
        {
            var studentToSubjects = _dataSet.Tables["StudentsToSubjects"];
            var subjects = _dataSet.Tables["Subjects"];
            var students = _dataSet.Tables["Students"];

            var sub = (from sts in studentToSubjects.AsEnumerable()
                       join subject in subjects.AsEnumerable()
                       on sts.Field<int>("SubjectId") equals subject.Field<int>("Id")
                       join student in students.AsEnumerable()
                       on sts.Field<int>("StudentId") equals student.Field<int>("Id")
                       select new
                       {
                           Name = student.Field<string>("Name"),
                           LastName = student.Field<string>("LastName"),
                           Subject = subject.Field<string>("SubjectName"),
                           Grade = sts.Field<byte>("Grade")
                       }).Distinct();

            var grouping = from gt in sub
                           group gt by gt.Subject into g
                           select new
                           {
                               Subject = g.Key,
                               Data = g
                           };

            foreach (var item in grouping)
            {
                Console.WriteLine(item.Subject);
                foreach (var data in item.Data)
                {
                    Console.WriteLine($"\t{data.Name} {data.LastName} : {data.Grade}");
                }
                Console.WriteLine();
            }
        }

        private int GetStudentId(int personalId)
        {
            DataTable students = _dataSet.Tables["Students"];
            var studentId = (from student in students.AsEnumerable()
                             where student.Field<int>("PersonalId") == personalId
                             select student.Field<int>("Id")).FirstOrDefault();

            return studentId;
        }

        private int GetSubjectId(string subjectName)
        {
            DataTable subjects = _dataSet.Tables["Subjects"];
            var subjectId = (from subject in subjects.AsEnumerable()
                             where subject.Field<string>("SubjectName").ToLower() == subjectName.ToLower()
                             select subject.Field<int>("Id")).FirstOrDefault();

            return subjectId;
        }

        private bool IsStudentAvailable(int personalId)
        {
            DataTable table = _dataSet.Tables["Students"];
            return table.AsEnumerable().Any(row => row.Field<int>("PersonalId") == personalId);
        }

        private bool isSubjectAvailable(string subjectName)
        {
            DataTable table = _dataSet.Tables["Subjects"];
            return table.AsEnumerable().Any(row => row.Field<string>("SubjectName").ToLower() == subjectName.ToLower());
        }

        private bool isStudentToSubjectAvailable(int studentId, int subjectId)
        {
            DataTable table = _dataSet.Tables["StudentsToSubjects"];

            var sub = (from query in table.AsEnumerable()
                       where query.Field<int>("StudentId") == studentId && query.Field<int>("SubjectId") == subjectId
                       select new { StudentId = studentId, SubjectId = subjectId }).FirstOrDefault();
            return sub is not null;
        }
    }
}
