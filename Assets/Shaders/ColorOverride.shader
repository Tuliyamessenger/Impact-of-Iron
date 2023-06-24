Shader "Custom/Sprite/ColorOverride"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OverrideColor ("覆盖色", Color) = (1,1,1,1)
        _OverrideFactor ("覆盖程度", Range(0, 1)) = 0.5
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
            fixed4 _OverrideColor;
            half _OverrideFactor;

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
                if(_OverrideFactor < 0) _OverrideFactor = 0;
                if(_OverrideFactor > 1) _OverrideFactor = 1;
                col.rgb = lerp(col.rgb, _OverrideColor.rgb, _OverrideFactor);
                return col;
            }
            ENDCG
        }
    }
}
