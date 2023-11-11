using UnityEngine;

namespace NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// selfからtargetがどちら側にいるかどうか
        /// </summary>
        public static float GetSide(Vector2 self, Vector2 target, Vector2 dir)
        {
            Vector2 diff = target - self;
            float cross = Cross(dir, diff);
            return Vector2.Angle(dir, diff) * ((cross > 0) ? 1 : -1);

            float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;
        }
        /// <summary>
        /// selfからtargetがどちら側にいるかどうか
        /// </summary>
        public static float GetSide(Transform self, Transform target, Vector2 dir)
        {
            return GetSide(self.position, target.position, dir);
        }
    }
}
