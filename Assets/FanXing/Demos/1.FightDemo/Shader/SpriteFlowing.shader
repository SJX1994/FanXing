Shader "Custom/SpriteFlowEmissionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Float) = 1
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Float) = 1
    }
    SubShader
    {
        Cull Off
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float _FlowSpeed;
            float4 _EmissionColor;
            float _EmissionStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset = float2(_FlowSpeed * _Time.y, 0);
                fixed4 col = tex2D(_MainTex, i.texcoord + offset) * i.color;

                fixed4 emission = _EmissionColor * _EmissionStrength;
                col.rgb += emission.rgb;

                return col;
            }
            ENDCG
        }
    }
}