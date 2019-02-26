using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Box
    {
        public Color color;
        public Rectangle rec;

        public Box(int _x, int _y, int _size, Color _color)
        {
            color = _color;
            rec = new Rectangle(_x, _y, _size, _size);
        }

        public void Move(int speed)
        {
            rec.Y += speed;
        }

        public void Move(int speed, string direction)
        {
            if (direction == "left")
            {
                rec.X -= speed;
            }
            if (direction == "right")
            {
                rec.X += speed;
            }
        }

        public Boolean Collision (Box b)
        {
            if (rec.IntersectsWith(b.rec))
            {
                return true;
            }

            return false;
        }
    }
}
