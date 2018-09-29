using DapperInCoreLib;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DapperInCoreWebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentRepository _repository;
        private ILog _log;

        public StudentController(IStudentRepository repository)
        {
            _repository = repository;
            _log = LogManager.GetLogger("NETCoreRepository", "student");
        }

        [HttpGet]
        public ActionResult<List<Student>> Get()
        {
            var list = _repository.GetList();
            _log.DebugFormat("当前学生信息:{0}", JsonConvert.SerializeObject(list));

            return list;
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            return _repository.GetStudent(id);
        }

        [HttpPost]
        public void Post([FromBody] List<Student> students)
        {
            _repository.Insert(students);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Student student)
        {
            _repository.Update(student);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}