using GameFrame.Core;

namespace GameFrame.UI
{
    /// <summary>
    /// 转发玩家的输入用于控制UI
    /// </summary>
    public class GridGroupElementSelectHelper(GridGroupElement bindCanvasElement)
    {
        public GridGroupElement BindElement { get; private set; } = bindCanvasElement;

        private Vector _gridPosition;

        private static int Recycle(int comp, int max)
        {
            return (comp + max) % max;
        }

        public virtual void Update()
        {
            var cur = BindElement[_gridPosition.X, _gridPosition.Y];

            if (cur != null && !cur.IsSelected)
            {
                cur.Select();
            }

            if (Input.GetButton("Confirm"))
            {
                cur?.Confirm();
            }

            var dir = Input.GetDirection();

            if (dir == Vector.Zero)
            {
                return;
            }
            dir.X *= Math.Sign(BindElement.GridSpacing.X);
            dir.Y *= Math.Sign(BindElement.GridSpacing.Y);

            var nextPos = _gridPosition + dir;


            // 调整Pos
            if (dir.X != 0)
            {
                nextPos.X = Recycle(nextPos.X, BindElement.GetMaxGridColumnCount());
                nextPos.Y = Math.Clamp(nextPos.Y, 0, BindElement.GetGridRowCount(nextPos.X) - 1);
            }
            else if (dir.Y != 0)
            {
                nextPos.Y = Recycle(nextPos.Y, BindElement.GetMaxGridRowCount());
                nextPos.X = Math.Clamp(nextPos.X, 0, BindElement.GetGridColumnCount(nextPos.Y) - 1);
            }


            var next = BindElement[nextPos.X, nextPos.Y];
            if (next != null)
            {
                next.Select();
                cur?.Unselect();
                _gridPosition = nextPos;
            }
        }
    }
}
