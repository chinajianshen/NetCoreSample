using System;

namespace NineskyStudy.ICarModule
{
    public interface ICar
    {
        void Run();
        void Turn(Direction direction );
        string Owener { get; set; }
    }


}
