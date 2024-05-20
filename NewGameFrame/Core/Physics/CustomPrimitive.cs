using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Core.Physics
{
    public class CustomPrimitive : IPrimitive
    {
        private readonly List<Vector> _vertexes = [];
        public IEnumerable<Vector> Vertexes => _vertexes;

        public void AddVertex(Vector vertex)
        {
            _vertexes.Add(vertex);
        }
        public void RemoveVertex(Vector vertex)
        {
            _vertexes.Remove(vertex);
        }

        public bool IsInside(Vector position)
        {
            foreach (var vertex in _vertexes)
            {
                if (vertex == position)
                    return true;
            }

            return false;
        }
    }
}
