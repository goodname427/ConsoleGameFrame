using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Render
{
    public interface ICameraRenderPass
    {
        void RenderPass(Camera camera, Image renderCache);
    }
}
