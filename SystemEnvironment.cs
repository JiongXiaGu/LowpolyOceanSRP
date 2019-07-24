using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.LowpolyOceanSRP
{

    public class SystemEnvironment
    {
        public static int MinimumGraphicsShaderLevel = 46;

        public static IReadOnlyList<RenderTextureFormat> RequiredRenderTextureFormat = new List<RenderTextureFormat>()
        {
            RenderTextureFormat.ARGBFloat,
            RenderTextureFormat.ARGBHalf,
            RenderTextureFormat.RHalf,
            RenderTextureFormat.RFloat,
            RenderTextureFormat.Depth,
            RenderTextureFormat.RGHalf,
            RenderTextureFormat.RGFloat,
            RenderTextureFormat.R8,
        };

        public int GraphicsShaderLevel { get; set; }
        public bool SupportGraphicsShaderLevel => GraphicsShaderLevel >= MinimumGraphicsShaderLevel;
        public BitArray SupportRenderTextureFormats { get; private set; }
        public bool Supports2DArrayTextures { get; set; }

        public SystemEnvironment()
        {
            GraphicsShaderLevel = SystemInfo.graphicsShaderLevel;

            SupportRenderTextureFormats = new BitArray(RequiredRenderTextureFormat.Count);
            for (int i = 0; i < RequiredRenderTextureFormat.Count; i++)
            {
                var format = RequiredRenderTextureFormat[i];
                SupportRenderTextureFormats[i] = SystemInfo.SupportsRenderTextureFormat(format);
            }

            Supports2DArrayTextures = SystemInfo.supports2DArrayTextures;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem(EditorHelper.MenuItemNameRoot + nameof(LogSystemInfos))]
        private static void LogSystemInfos()
        {
            var info = new SystemEnvironment();
            Debug.Log(info.ToString());
        }
#endif

        private string SetColor(bool isUseGreen, string str)
        {
            var red = "red";
            var green = "green";
            return string.Format("<color={0}>{1}</color>", isUseGreen ? green: red, str);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            bool isSupport = true;

            isSupport &= SupportGraphicsShaderLevel;
            builder.AppendLine(SetColor(SupportGraphicsShaderLevel, string.Format("GraphicsShaderLevel : {0}", GraphicsShaderLevel)));

            for (int i = 0; i < SupportRenderTextureFormats.Count; i++)
            {
                var format = RequiredRenderTextureFormat[i];
                bool isSupportFormat = SupportRenderTextureFormats[i];
                builder.AppendLine(SetColor(isSupportFormat, string.Format("Support RenderTexture.{0} : {1}", format.ToString(), isSupportFormat)));
            }

            builder.AppendLine(SetColor(Supports2DArrayTextures, string.Format("Support 2DArrayTexture : {0}", Supports2DArrayTextures)));

            builder.Insert(0, "---" + Environment.NewLine);
            builder.Insert(0, SetColor(isSupport, "Current environment : " + (isSupport ? "Support" : "Not support") + " [Lowpoly water SRP]" + Environment.NewLine));

            return builder.ToString();
        }
    }

}
