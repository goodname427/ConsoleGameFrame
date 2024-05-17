namespace GameFrame.Core.Physics
{
    public interface IPrimitive
    {
        public IEnumerable<Vector> Vertexes { get; }

        bool IsInside(Vector position);
    }
}
