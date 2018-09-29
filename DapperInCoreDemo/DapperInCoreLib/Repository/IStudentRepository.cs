using System.Collections.Generic;

namespace DapperInCoreLib
{
    public interface IStudentRepository
    {
        /// <summary>
        /// 取所有学生
        /// </summary>
        /// <returns></returns>
        List<Student> GetList();

        /// <summary>
        /// 获取某学生信息
        /// </summary>
        /// <returns></returns>
        Student GetStudent(int id);

        /// <summary>
        /// 写入学生信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Insert(List<Student> list);

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        bool Update(Student student);

        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
    }
}