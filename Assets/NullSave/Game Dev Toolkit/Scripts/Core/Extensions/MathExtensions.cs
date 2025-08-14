using UnityEngine;

namespace NullSave.GDTK
{
    public static class MathExtensions
    {

        #region Public Methods

        /// <summary>
        /// Convert any angle to range of -180 to +180
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Convert360to180(this float angle)
        {
            angle = FixAngle(angle);
            if (angle < -180) return angle + 360;
            if (angle > 180) return angle - 360;
            return angle;
        }

        /// <summary>
        /// Reduce any angle to range of -360 to 360
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float FixAngle(this float angle)
        {
            do
            {
                if (angle < -360) angle += 360;
                if (angle > 360) angle -= 360;
            } while (angle < -360 || angle > 360);

            return angle;
        }

        /// <summary>
        /// Normalize the angle between -180 and 180 degrees
        /// </summary>
        /// <param Name="eulerAngle">Euler angle.</param>
        public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
        {
            var delta = eulerAngle;

            delta.x = Convert360to180(delta.x);
            delta.y = Convert360to180(delta.y);
            delta.z = Convert360to180(delta.z);

            return new Vector3(delta.x, delta.y, delta.z);
        }

        /// <summary>
        /// Check if a point is inside of a quad
        /// </summary>
        /// <param name="point"></param>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="bottomLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        public static bool PointInQuad(this Vector2 point, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            if (PointInTriangle(point, topLeft, topRight, bottomLeft)) return true;
            if (PointInTriangle(point, topRight, bottomRight, bottomLeft)) return true;
            return false;
        }

        /// <summary>
        /// Check if a point is inside a triangle
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool PointInTriangle(this Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            float area = 0.5f * (-b.y * c.x + a.y * (-b.x + c.x) + a.x * (b.y - c.y) + b.x * c.y);
            float s = 1 / (2 * area) * (a.y * c.x - a.x * c.y + (c.y - a.y) * p.x + (a.x - c.x) * p.y);
            float t = 1 / (2 * area) * (a.x * b.y - a.y * b.x + (a.y - b.y) * p.x + (b.x - a.x) * p.y);
            return s >= 0 && t >= 0 && (s + t) <= 1;
        }

        #endregion

    }
}