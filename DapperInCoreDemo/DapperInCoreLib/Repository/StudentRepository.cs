using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DapperInCoreLib
{
    public class StudentRepository : IStudentRepository
    {
        private static readonly string SELECT_SQL_STRING = @"select * from TB_Student";
        private static readonly string INSERT_SQL_STRING = @"insert into TB_Student(Name, Age) values(@Name, @Age)";
        private static readonly string UPDATE_SQL_STRING = @"update TB_Student set Name=@Name, Age=@Age where Id = @Id";
        private static readonly string DELETE_SQL_STRING = @"delete TB_Student where Id = @Id";

        private IConfiguration _configuration;
        private DapperFactory _factory;
        private IDapper _dapper;

        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = DapperFactory.GetInstance(configuration);
            _dapper = _factory.GetDapper();
        }

        public List<Student> GetList()
        {
            return _dapper.GetList<Student>(SELECT_SQL_STRING);
        }

        public Student GetStudent(int id)
        {
            return GetList().FirstOrDefault(f => f.Id == id);
        }

        public bool Insert(List<Student> list)
        {
            return _dapper.Insert(INSERT_SQL_STRING, list);
        }

        public bool Update(Student student)
        {
            return _dapper.Update(UPDATE_SQL_STRING, student);
        }

        public bool Delete(int id)
        {
            return _dapper.Delete(DELETE_SQL_STRING, id);
        }
    }
}