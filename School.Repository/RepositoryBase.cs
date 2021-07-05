using DatabaseHelper;
using School.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace School.Repository
{
    public abstract class RepositoryBase<T> where T : new()
    {
        protected readonly Database<SqlConnection> _database;
        protected readonly string _typeName;

        public RepositoryBase(string connectionString)
        {
            _database = new Database<SqlConnection>(connectionString);
            _typeName = typeof(T).Name;
        }

        public int Insert(T model)
        {
            string procedureName = $"Insert{_typeName}_SP";

            var procedureParams = procedureName.GetProcedureParams(_database).ToSqlParams(model);

            return _database.ExecuteNonQuery(procedureName, CommandType.StoredProcedure, procedureParams.ToArray());
        }

        public void Update(T model)
        {
            string procedureName = $"Update{_typeName}_SP";

            var procedureParams = procedureName.GetProcedureParams(_database).ToSqlParams(model);

            _database.ExecuteNonQuery(procedureName, CommandType.StoredProcedure, procedureParams.ToArray());
        }

        public void Delete(int id)
        {
            SqlParameter sqlParameter = new() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = id };

            _database.ExecuteNonQuery($"Delete{_typeName}_SP", CommandType.StoredProcedure, sqlParameter);
        }
    }
}
