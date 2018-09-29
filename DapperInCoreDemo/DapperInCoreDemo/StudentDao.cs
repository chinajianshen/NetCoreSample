using System.Collections.Generic;

namespace DapperInCoreDemo
{
    public class StudentDao
    {
        private static readonly string SELECT_SQL_STRING = @"select * from TB_Student";
        private static readonly string INSERT_SQL_STRING = @"insert into TB_Student(Name, Age) values(@Name, @Age)";
        private static readonly string UPDATE_SQL_STRING = @"update TB_Student set Name=@Name, Age=@Age where Id = @Id";
        private static readonly string DELETE_SQL_STRING = @"delete TB_Student where Id = @Id";

        /// <summary>
        /// 获取学生列表
        /// </summary>
        /// <returns></returns>
        public static List<Student> GetList()
        {
            return SELECT_SQL_STRING.GetList<Student>();
        }

        /// <summary>
        /// 批量插入学生信息
        /// </summary>
        /// <param name="studentList"></param>
        /// <returns></returns>
        public static bool Insert(List<Student> studentList)
        {
            return INSERT_SQL_STRING.Insert(studentList);
        }

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public static bool Update(Student student)
        {
            return UPDATE_SQL_STRING.Update(student);
        }

        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public static bool Delete(Student student)
        {
            return DELETE_SQL_STRING.Delete(student);
        }
    }
}