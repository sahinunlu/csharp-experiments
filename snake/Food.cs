using System.Drawing;

namespace snakegame
{
    public interface IFood
    { }

    public class Food : IFood
    {
        public Point Location;
    }
}
