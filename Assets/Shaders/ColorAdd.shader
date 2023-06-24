Shader "Custom/Sprite/ColorAdd"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CoverTex ("Cover", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue" = "Transparent"}
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float4 overColor : TEXCOORD1;
                //float overFactor : TEXCOORD2;
                float4 color : COLOR;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _CoverTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                float gray = col.r * 0.3 + col.g*0.59 + col.b*0.11;
                col.rgb = lerp(col.rgb, tex2D(_CoverTex, i.uv).rgb, 1 - gray);
                return col;
            }
            ENDCG
        }
    }
}
