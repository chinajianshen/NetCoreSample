using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperInCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var studentList = new List<Student>()
            {
                new Student(){ Name = "Jimmy.you", Age = 30 },
                new Student(){ Name = "Mark.zhou", Age = 32 },
            };

            // 写入
            var addResultFlag = StudentDao.Insert(studentList);
            if (addResultFlag)
            {
                Console.WriteLine("Dapper批量数据写入成功！");
            }

            // 查询
            var selectResList = StudentDao.GetList();
            Console.WriteLine("当前学生数据：{0}", JsonConvert.SerializeObject(selectResList));

            // 更新
            var student = selectResList.FirstOrDefault(s => s.Name == "Jimmy.you");
            if(null != student)
            {
                student.Age = 31;
            }
            var updateResultFlag = StudentDao.Update(student);
            if (updateResultFlag)
            {
                Console.WriteLine("Dapper更新数据成功!");
            }

            // 测试删除
            var deleteResultFlag = StudentDao.Delete(student);
            if (deleteResultFlag)
            {
                Console.WriteLine("Dapper删除数据成功！");
            }

            Console.ReadKey();
        }
    }
}