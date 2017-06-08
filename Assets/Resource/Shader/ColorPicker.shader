Shader "Unlit/ColorPicker"
{
	Properties
	{
		BarHeight("BarHeight",Range(0.1,0.3)) = 0.15
		PanelHeight("BarHeight",Range(0.15,0.8)) = 0.5
		StartColor("StartColor",Color)=(1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="3000" }
		LOD 100
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
			float BarHeight;
			float PanelHeight;
			float4 StartColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 result = fixed4(1,1,1,0);
				float2 uv = i.uv;
				if (uv.y > (1 - BarHeight))
				{
					fixed3 start = fixed3(1, 1, 1);
					fixed3 end = start;
					
					float d1 = 1.0 / 3.0;
					float d2 = 2.0 / 3.0;
					float d3 = 1;
					if (uv.x < d1)
					{
						start = fixed3(1, 0, 0);
						end = fixed3(0, 1, 0);
						d3 = uv.x / d1;
					}
					else if (uv.x >= d1 && uv.x < d2)
					{
						start = fixed3(0, 1, 0);
						end = fixed3(0, 0, 1);
						d3 = (uv.x - d1) / d1;
					}
					else if (uv.x >= d2)
					{
						start = fixed3(0, 0, 1);
						end = fixed3(1, 0, 0);
						d3 = (uv.x - d2) / d1;
					}
					fixed3 d_color = end - start;
					result.rgb = start + d_color * d3;
					result.a = 1;
				}
				else if (uv.y < PanelHeight)
				{
					float3 d_color = StartColor.rgb - float3(1, 1, 1);
					float3 color_start = float3(1, 1, 1) + d_color*uv.x;
					result.rgb = color_start*(uv.y/ PanelHeight);
					result.a = 1;
				}
				
				return result;
			}
			ENDCG
		}
	}
}
