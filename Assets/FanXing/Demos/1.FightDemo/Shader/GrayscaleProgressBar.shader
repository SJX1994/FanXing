Shader "Custom/GrayscaleProgressBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 输入的灰度图
        _GrayTex ("Gray Texture", 2D) = "white" {} // 灰度图
        [PerRendererData] _Progress ("Progress", Range(0,1)) = 1 // 进度值
        _RotationSpeed ("Rotation Speed", Float) = 30 // 旋转速度
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _GrayTex;
            float _Progress;
            float _RotationSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 根据灰度图亮度计算当前进度
                float grayscale = tex2D(_GrayTex, i.texcoord).r;
                float angle = _Time.y * _RotationSpeed;
                float2 center = float2(0.5, 0.5); // 旋转中心为纹理中心
                float2 uv = i.texcoord - center;
                float2 rotatedUV = float2(
                    uv.x * cos(angle) - uv.y * sin(angle) + center.x,
                    uv.x * sin(angle) + uv.y * cos(angle) + center.y
                );
                float4 color = tex2D(_MainTex, rotatedUV);
                color.a = tex2D(_MainTex, i.texcoord).a;
                float4 colorDark = color * 0.5;
                float progress = step(1-_Progress, grayscale);
                return lerp(colorDark, color, progress);
                
                // return color; // 使用原始颜色
                // return fixed4(progress, progress, progress, 1); // 使用进度值作为颜色值
            }
            ENDCG
        }
    }
}