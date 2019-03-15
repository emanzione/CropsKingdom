Shader "BoxWar/Indicators/Generic Textured 2 Colors"
{
	Properties
	{
		_Color1("Color", Color) = (1,1,1,1)
		_Color2("Hidden Color", Color) = (1,1,1,1)
		_Texture("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "RenderQueue"="Transparent"}
		Cull off
		Pass
		{
			Zwrite off
			ZTest GEqual
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color2;
			sampler2D _Texture;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color2 * tex2D(_Texture, i.uv);
				
				return col;
			}
			ENDCG
		}
	
		Pass
		{
			Zwrite off
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color1;
			sampler2D _Texture;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Color1 * tex2D(_Texture, i.uv);
				return col;
			}
			ENDCG
		}

	}
}
