using NineskyStudy.ICarModule;
using System;

namespace NineskyStudy.AudiCarModule
{
    public class AudiCar : ICar
    {
        public AudiCar() { }

        public string Owener { get; set; } = "我是奥迪车";

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
