using NineskyStudy.ICarModule;
using System;

namespace NineskyStudy.BenzCarModule
{
    public class BenzCar : ICar
    {
        public BenzCar() { }

        public string Owener { get; set; } = "我是奔驰车";

        public void Run()
        {
            Console.WriteLine("AudiCar Run");
        }

        public void Turn(Direction direction)
        {
            Console.WriteLine("AudiCar Turn: " + direction.ToString());
        }
    }
}
