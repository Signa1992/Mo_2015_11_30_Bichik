Shader "Hidden/MapInversionTransform"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Flags( "Flags", Vector) = (0.0, 0.0, 0.0, 0.0)
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Flags;
			float4x4 _TargetMVP;

			fixed4 frag (v2f i) : SV_Target
			{
				float x;

				if (_Flags.x >= 1.0)
				{
					x = i.uv.x - 0.5;
				}
				else
				{
					x = 0.5 - i.uv.x;
				}

				float y;

				if (_Flags.y >= 1.0)
				{
					y = i.uv.y - 0.5;
				}
				else
				{
					y = 0.5 - i.uv.y;
				}

				float4 uv = mul(_TargetMVP, float4(x, 0.0, y, 1.0));
				return tex2D(_MainTex, (uv.xy / uv.w + 1.0) * 0.5);
			}
			ENDCG
		}
	}
}
