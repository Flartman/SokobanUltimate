using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanUltimate
{
    public class LevelLibrary
    {
        public static string SimpleTenTenGround;
        public static string SimpleTenTenCollision;
        public static string Level1Ground;
        public static string Level1Collision;

        public static void GenerateAll()
        {
            var sb = new StringBuilder();
            sb.Append("GGGGGGGGGG");
            sb.Append("GGGGGGGGGG");
            sb.Append("GGEGGGGGGG");
            sb.Append("GGGGGGGGGG");
            sb.Append("GGGGEGGGGG");
            sb.Append("GGGGGGGGGG");
            sb.Append("GGGGGGGGGG");
            sb.Append("GGGGGGEGGG");
            sb.Append("GGGGGGGGGG");
            sb.Append("GGGGGGGGGG");

            SimpleTenTenGround = sb.ToString();

            sb.Clear();
            sb.Append("WWWWWWWWWW");
            sb.Append("W........W");
            sb.Append("W.....B..W");
            sb.Append("W..P.....W");
            sb.Append("W........W");
            sb.Append("W........W");
            sb.Append("W........W");
            sb.Append("W..B.....W");
            sb.Append("W........W");
            sb.Append("WWWWWWWWWW");
            SimpleTenTenCollision = sb.ToString();

            sb.Clear();
            sb.Append("WWWWWWW");
            sb.Append("W.W..WW");
            sb.Append("WPB...W");
            sb.Append("WBWB..W");
            sb.Append("W...W.W");
            sb.Append("W...B.W");
            sb.Append("WWWWWWW");
            Level1Collision = sb.ToString();

            sb.Clear();
            sb.Append("GGGGGGG");
            sb.Append("GEGEGGG");
            sb.Append("GEGGGGG");
            sb.Append("GGGGGGG");
            sb.Append("GGGGGGG");
            sb.Append("GGGGGEG");
            sb.Append("GGGGGGG");
            Level1Ground = sb.ToString();

        }
    }
}
