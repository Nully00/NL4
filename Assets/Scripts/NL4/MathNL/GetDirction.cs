using UnityEngine;

namespace NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// 角度を指定してVector2を返す
        /// </summary>
        public static Vector2 GetDirection(float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }
        /// <summary>
        /// スタートとゴールを指定して方向を返す。
        /// </summary>
        public static Vector2 GetDirection(Vector2 from, Vector2 to)
        {
            return GetDirection(GetAngle(from, to));
        }
    }
}
