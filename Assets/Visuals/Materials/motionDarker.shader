Shader "Unlit/motionDarker"
{
	Properties
	{
		_FocusSpot ("Focus Spot", Vector) = (0,0,0,0)
		_Radius ("radius of focus", Float) = 0.4
	}
	SubShader
	{

		Tags { "RenderType"="Overlay" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
		ZWrite off
		ZTest off
		Blend SrcAlpha OneMinusSrcAlpha
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

			float4 _FocusSpot;
			float _Radius;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = o.vertex.xy/o.vertex.w;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 space = i.uv;
				float light = length(space - float2(_FocusSpot.x,_FocusSpot.y))/_Radius;
				return fixed4(0.0f,0.0f,0.0f,clamp(light,0.0f,1.0f));
			}
			ENDCG
		}
	}
}
