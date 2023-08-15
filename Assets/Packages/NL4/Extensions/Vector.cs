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
        public static Vector3 Add(this Vector3 self, Vector3 opp)
            => new Vector3(self.x + opp.x, self.y + opp.y);
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="opp">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 Add(this Vector2 self, Vector3 opp)
            => new Vector3(self.x + opp.x, self.y + opp.y);
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddX(this Vector3 self, float oppX)
        {
            return new Vector3(self.x + oppX, self.y);
        }
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddX(this Vector2 self, float oppX)
        {
            return new Vector3(self.x + oppX, self.y);
        }
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddY(this Vector3 self, float oppY)
        {
            return new Vector3(self.x, self.y + oppY);
        }
        /// <summary>
        /// 加算したものを返します。return = self + opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 AddY(this Vector2 self, float oppY)
        {
            return new Vector3(self.x, self.y + oppY);
        }

        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="opp">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 Sub(this Vector3 self, Vector3 opp)
        {
            return new Vector3(self.x - opp.x, self.y - opp.y);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="opp">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 Sub(this Vector2 self, Vector3 opp)
        {
            return new Vector3(self.x - opp.x, self.y - opp.y);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubX(this Vector3 self, float oppX)
        {
            return new Vector3(self.x - oppX, self.y);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppX">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubX(this Vector2 self, float oppX)
        {
            return new Vector3(self.x - oppX, self.y);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubY(this Vector3 self, float oppY)
        {
            return new Vector3(self.x, self.y - oppY);
        }
        /// <summary>
        /// 減算したものを返します。return = self - opp;
        /// </summary>
        /// <param name="self">左オペランド</param>
        /// <param name="oppY">右オペランド</param>
        /// <returns>加算した結果</returns>
        public static Vector3 SubY(this Vector2 self, float oppY)
        {
            return new Vector3(self.x, self.y - oppY);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppX">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetX(this Vector3 self, float oppX)
        {
            return new Vector3(oppX, self.y);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppX">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetX(this Vector2 self, float oppX)
        {
            return new Vector3(oppX, self.y);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppY">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetY(this Vector3 self, float oppY)
        {
            return new Vector3(self.x, oppY);
        }
        /// <summary>
        /// 設定したものを返します。
        /// </summary>
        /// <param name="self">設定対象</param>
        /// <param name="oppY">設定する値</param>
        /// <returns>設定した結果</returns>
        public static Vector3 SetY(this Vector2 self, float oppY)
        {
            return new Vector3(self.x, oppY);
        }

        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(X,0)</returns>
        public static Vector3 ExtractX(this Vector3 self)
        {
            return new Vector3(self.x, 0);
        }
        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(X,0)</returns>
        public static Vector3 ExtractX(this Vector2 self)
        {
            return new Vector3(self.x, 0);
        }
        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(0,Y)</returns>
        public static Vector3 ExtractY(this Vector3 self)
        {
            return new Vector3(0, self.y);
        }
        /// <summary>
        /// 抽出(取り出)したものを返します。抽出されない値は0で返します。
        /// </summary>
        /// <param name="self">抽出対象</param>
        /// <returns>Vector2(0,Y)</returns>
        public static Vector3 ExtractY(this Vector2 self)
        {
            return new Vector3(0, self.y);
        }
    }
}