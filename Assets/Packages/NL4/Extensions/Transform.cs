using UnityEngine;

namespace NL4.Extensions
{
    public static partial class VectorExtensions
    {
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="opp">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 Add(this Transform self, Vector3 opp)
            => Add(self.position, opp);
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddX(this Transform self, float oppX)
        {
            return AddX(self.position, oppX);
        }
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddY(this Transform self, float oppY)
        {
            return AddY(self.position, oppY);
        }

        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="opp">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 Sub(this Transform self, Vector3 opp)
        {
            return Sub(self.position, opp);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubX(this Transform self, float oppX)
        {
            return SubX(self.position, oppX);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubY(this Transform self, float oppY)
        {
            return SubY(self.position, oppY);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppX">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetX(this Transform self, float oppX)
        {
            return SetX(self.position, oppX);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppY">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetY(this Transform self, float oppY)
        {
            return SetY(self.position, oppY);
        }

        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(X,0)</returns>
        public static Vector3 ExtractX(this Transform self)
        {
            return ExtractX(self.position);
        }
        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(0,Y)</returns>
        public static Vector3 ExtractY(this Transform self)
        {
            return ExtractY(self.position);
        }
    }
}
