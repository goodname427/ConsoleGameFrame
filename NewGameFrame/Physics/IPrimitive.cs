using GameFrame.MathCore;

namespace GameFrame.Physics
{
    public interface IPrimitive
    {
        public IEnumerable<Vector> Vertexes { get; }

        bool IsInside(Vector position);
    }
}
