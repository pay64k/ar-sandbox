// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/AlessandroExample"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//HeightMapFloat("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D HeightMap;

			fixed4 get_color(float height)
			{
				fixed4 color = (1.0, 1.0, 1.0, 1.0);

				if (height < 0.10f) {
					color = fixed4(0.2, 0.9, 0.0, 1.0);
				}

				else if (height < 0.2f) {
					color = fixed4(0.61, 0.91, 0.04, 1.0);
				}
				
				else if (height < 0.30f) {
					color = fixed4(0.93, 0.86, 0.08, 1.0);
				}

				else if (height < 0.40f) {
					color = fixed4(0.95, 0.52, 0.12, 1.0);
				}
				
				else if (height < 0.50f) {
					color = fixed4(0.97, 0.20, 0.16, 1.0);
				}

				return color;

			}
			
			v2f vert(appdata v)
			{
				v2f o;
				//float height = v.vertex.z + 1.0;// 
				float4 vertexws = mul(1, v.vertex);

				float height = vertexws.y + tex2Dlod(_MainTex, float4(v.uv, 0, 0)).x;

				vertexws = float4(vertexws.x, height, vertexws.z , vertexws.w);
				
				o.color = get_color(height);

				o.vertex = mul(UNITY_MATRIX_VP, vertexws);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.color;
			/*	fixed4 col = fixed4(i.uv, 0, 1);*/
				return col;
				//return half4(1.0, 0.0, 0.0, 1.0);
			}
			ENDCG
		}
	}
}
