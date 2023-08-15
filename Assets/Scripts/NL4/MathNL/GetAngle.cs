using UnityEngine;

namespace NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// Vector2を指定して角度を返す
        /// </summary>
        /// <returns>(0～179～180～-179～0)</returns>
        public static float GetAngle(Vector2 from, Vector2 to)
        {
            var dx = to.x - from.x;
            var dy = to.y - from.y;
            var rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }
        /// <summary>
        /// Vector2を指定して角度を返す
        /// </summary>
        /// <returns>(0～179～180～-179～0</returns>
        public static float GetAngle(Transform from, Transform to)
        {
            return GetAngle(from.position, to.position);
        }
        /// <summary>
        /// Vector2.zeroからtoの角度を返す
        /// </summary>
        public static float GetAngle(Vector2 to)
        {
            return GetAngle(Vector2.zero, to);
        }
        /// <summary>
        /// Vector2を指定して角度を返す
        /// </summary>
        /// <returns>0～359</returns>
        public static float GetAngle360(Vector2 from, Vector2 to)
        {
            var dx = to.x - from.x;
            var dy = to.y - from.y;
            var rad = Mathf.Atan2(dy, dx);
            var angle = rad * Mathf.Rad2Deg;

            if (angle >= 0) return angle;
            return (180 + angle) + 180;
        }
        /// <summary>
        /// Vector2を指定して角度を返す
        /// </summary>
        /// <returns>0～359</returns>
        public static float GetAngle360(Transform from, Transform to)
        {
            return GetAngle360(from.position, to.position);
        }

    }
}
