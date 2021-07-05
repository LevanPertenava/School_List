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
        public Commands(string connectionString)
        {
            _connection = connectionString;
            _database = new Database<SqlConnection>(connectionString);
        }

        public void AddPerson(Student student)
        {
            StudentRepository studentRepository = new(_connection);
            studentRepository.Insert(student);
        }

        public void RemovePerson(int id)
        {
            StudentRepository studentRepository = new(_connection);
            studentRepository.Delete(id);
        }

        public void AddSubject(string subjectName)
        {
            SubjectRepository subjectRepository = new(_connection);
            subjectRepository.Insert(new Subject() { SubjectName = subjectName });
        }

        public void AddPersonToSubject(int id, string subjectName)
        {
            SqlParameter[] sqlParameters = { new SqlParameter() { ParameterName = "@PersonalId", SqlDbType = SqlDbType.Int, Value = id },
                                             new SqlParameter() {ParameterName = "@SubjectName", SqlDbType = SqlDbType.NVarChar, Value =subjectName }
                                            };
            _database.ExecuteNonQuery("InsertStudentsSubjects_SP", CommandType.StoredProcedure, sqlParameters);
        }

        public void SetPoint(int id, string subjectName, byte grade)
        {
            SqlParameter[] sqlParameters = { new SqlParameter() { ParameterName = "@PersonalId", SqlDbType = SqlDbType.Int, Value = id },
                                             new SqlParameter() {ParameterName = "@SubjectName", SqlDbType = SqlDbType.NVarChar, Value = subjectName },
                                             new SqlParameter() {ParameterName = @"Grade", SqlDbType = SqlDbType.TinyInt, Value = grade}
                                            };
            _database.ExecuteNonQuery("SetPoint_SP", CommandType.StoredProcedure, sqlParameters);
        }

        public DataTable GetPersonPoint(int id)
        {
            return GetPersonPoint(id, null);
        }

        public DataTable GetPersonPoint(int id, string subject)
        {
            SqlParameter[] sqlParameter = { new SqlParameter() { ParameterName = "@PersonalId", SqlDbType = SqlDbType.Int, Value = id },
                                            new SqlParameter() {ParameterName = "@SubjectName", SqlDbType = SqlDbType.NVarChar, Value = subject}
                                          };
            return _database.GetTable("GetPersonPoint_SP", CommandType.StoredProcedure, sqlParameter);
        }

        public DataTable GetAllPersonsPoints(string subject)
        {
            return _database.GetTable("GetAllPersonsPoints_SP", CommandType.StoredProcedure, new SqlParameter() { ParameterName = "@SubjectName", SqlDbType = SqlDbType.NVarChar, Value = subject });
        }

        public DataTable GetAllPersonsPoints()
        {
            return GetAllPersonsPoints(null);

        }
    }
}
