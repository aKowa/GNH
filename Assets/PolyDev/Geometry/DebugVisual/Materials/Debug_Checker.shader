// based on Unity3D's Texturing shader examples Procedural checkerboard pattern on https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
// extended by PolyDev by a Color value

Shader "PolyPaw/Debug/Checker"
{
	Properties{
		_Color1("Color1", Color) = (0,0,0,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Density("Density", Range(2,50) ) = 30
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

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

			float _Density;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv * _Density;
				return o;
			}
			
			fixed4 _Color1;
			fixed4 _Color2;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 c = floor(i.uv) / 2;
				float value = frac(c.x + c.y) * 2;
				
				if (value == 0) 
				{
					return _Color1;
				}
				return _Color2;
			}
			ENDCG
		}
	}
}
