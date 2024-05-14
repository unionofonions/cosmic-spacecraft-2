Shader "Unlit/AutoScrollShader_Unscaled"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        [HideInInspector] _UnscaledTime ("UnscaledTime", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" }
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
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _ScrollSpeed;
            fixed4 _Color;
            float _UnscaledTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset = float2(0, _UnscaledTime * _ScrollSpeed);
                fixed4 col = tex2D(_MainTex, i.uv + offset) * _Color;
                return col;
            }
            ENDCG
        }
    }
}