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
				fixed4 bla = (1.0, 0.0, 0.0, 1.0);

				if (height < 200.0) {
					bla = fixed4(0.0, 1.0, 0.0, 1.0);
				}
				else {
					bla = fixed4(0.0, 0.0, 1.0, 1.0);
				}
				return bla;
			}
			
			v2f vert(appdata v)
			{
				v2f o;
				//float height = v.vertex.z + 1.0;// 
				float4 vertexws = mul(unity_ObjectToWorld, v.vertex);

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
