using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// 指定した範囲に入っているかどうか
        /// </summary>
        public static bool WithinRange(this float self, float from, float to)
        {
            var (min, max) = (from > to) ? (to, from) : (from, to);
            return min <= self && self <= max;
        }
        /// <summary>
        /// 指定した角度に入っているかどうか
        /// </summary>
        /// <param name="dir">0-180の範囲で指定(yがマイナスの象限の場合はマイナスをつける)</param>
        /// <param name="range"></param>
        public static bool WithinAngle(float angle, Vector2 dir, float range)
        {
            float diff = Mathf.Abs(GetAngle(Vector2.zero, dir) - angle);
            if (diff < range / 2)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 指定した角度に入っているかどうか
        /// </summary>
        /// <param name="dir">0-180の範囲で指定(yがマイナスの象限の場合はマイナスをつける)</param>
        /// <param name="range"></param>
        public static bool WithinAngle(Vector2 from, Vector2 to, Vector2 dir, float range)
        {
            float angle = GetAngle(from, to);

            return WithinAngle(angle, dir, range);
        }
        /// <summary>
        /// 指定した角度に入っているかどうか
        /// </summary>
        public static bool WithinAngle(Transform from, Transform to, Vector2 dir, float range)
        {
            return WithinAngle(from.position, to.position, dir, range);
        }
        /// <summary>
        /// 指定した四角の範囲内に入っているかどうか
        /// </summary>
        /// <param name="self">現在の座標</param>
        /// <param name="from">一つ目の座標</param>
        /// <param name="to">二つ目の座標</param>
        /// <returns>入ってたらtrue</returns>
        public static bool WithinSquare(this Vector2 self, Vector2 from, Vector2 to)
        {
            float maxX, minX, maxY, minY;
            if (from.x > to.x)
            {
                maxX = from.x;
                minX = to.x;
            }
            else
            {
                maxX = to.x;
                minX = from.x;
            }

            if (from.y > to.y)
            {
                maxY = from.y;
                minY = to.y;
            }
            else
            {
                maxY = to.y;
                minY = from.y;
            }
            return WithinSquareLeftUpRightDown(self, new Vector2(minX, maxY), new Vector2(maxX, minY));
        }

        /// <summary>
        /// 指定した四角の範囲内に入っているかどうか
        /// </summary>
        /// <param name="self">現在の座標</param>
        /// <param name="from">一つ目の座標</param>
        /// <param name="to">二つ目の座標</param>
        /// <returns>入ってたら true</returns>
        public static bool WithinSquare(this Vector3 self, Vector2 from, Vector2 to)
        {
            float maxX, minX, maxY, minY;
            if (from.x > to.x)
            {
                maxX = from.x;
                minX = to.x;
            }
            else
            {
                maxX = to.x;
                minX = from.x;
            }

            if (from.y > to.y)
            {
                maxY = from.y;
                minY = to.y;
            }
            else
            {
                maxY = to.y;
                minY = from.y;
            }
            return WithinSquareLeftUpRightDown(self, new Vector2(minX, maxY), new Vector2(maxX, minY));
        }

        /// <summary>
        /// 左上と左下を指定して座標が四角範囲内に入っているかどうか
        /// </summary>
        /// <param name="self">指定した座標</param>
        /// <param name="leftUp">左上</param>
        /// <param name="rightDown">左下</param>
        /// <returns>入っていたら true</returns>
        private static bool WithinSquareLeftUpRightDown(this Vector2 self, Vector2 leftUp, Vector2 rightDown)
        {
            if (self.x < leftUp.x || rightDown.x < self.x) return false;
            if (self.y > leftUp.y || rightDown.y > self.y) return false;
            return true;
        }
        /// <summary>
        /// 指定した扇形に入っているかどうか
        /// </summary>
        public static bool WithinSharpFun(this Vector2 self, Vector2 target, Vector2 dir, float angle, float r)
        {
            return WithinCircle(self, target, r) && WithinAngle(self, target, dir, angle);
        }
        /// <summary>
        /// 指定した扇形に入っているかどうか
        /// </summary>
        public static bool WithinSharpFun(this Transform self, Transform target, Vector2 dir, float angle, float r)
        {
            return WithinSharpFun(self.position, target.position, dir, angle, r);
        }
        /// <summary>
        /// 指定した円形に入っているかどうか
        /// </summary>
        public static bool WithinCircle(this Vector2 self, Vector2 target, float r)
        {
            return Vector2.Distance(self, target) <= r;
        }
        /// <summary>
        /// 指定した円形に入っているかどうか
        /// </summary>
        public static bool WithinCircle(this Transform self, Transform target, float r)
        {
            return WithinCircle(self.position, target.position, r);
        }
    }
}
