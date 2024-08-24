Shader "Skybox/Dual Panoramic Cubemap" {
	Properties{
		_Tint1("Tint Color 1", Color) = (.5, .5, .5, .5)
		_Tint2("Tint Color 2", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure1("Exposure 1", Range(0, 8)) = 1.0
		[Gamma] _Exposure2("Exposure 2", Range(0, 8)) = 1.0
		_Rotation1("Rotation1", Range(0, 360)) = 0
		_Rotation2("Rotation2", Range(0, 360)) = 0
		[NoScaleOffset] _Texture1("Cubemap 1", CUBE) = "black" {}
		[NoScaleOffset] _Texture2("Cubemap 2", CUBE) = "black" {}
		_Blend("Blend", Range(0.0, 1.0)) = 0.0
	}
 
	SubShader{
		Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off
 
		Pass {
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
 
			samplerCUBE _Texture1;
			samplerCUBE _Texture2;
 
			half4 _Tint1;
			half4 _Tint2;
			half _Exposure1;
			half _Exposure2;
			float _Rotation1;
			float _Rotation2;
 
			float _Blend;

			float3 RotateAroundYInDegrees(float3 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float3(mul(m, vertex.xz), vertex.y).xzy;
			}
 
			struct appdata_t {
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
 
			struct v2f {
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
 
			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 rotated = RotateAroundYInDegrees(v.vertex.xyz, _Rotation1);
				o.vertex = UnityObjectToClipPos(rotated);
				o.texcoord = rotated;
				return o;
			}
 
			fixed4 frag(v2f i) : SV_Target
			{
				half4 tex1 = texCUBE(_Texture1, normalize(i.texcoord));
				half4 tex2 = texCUBE(_Texture2, normalize(i.texcoord));

				half3 c1 = tex1.rgb;
				half3 c2 = tex2.rgb;

				c1 = lerp(c1, c2, _Blend) * lerp(_Tint1.rgb, _Tint2.rgb, _Blend) * unity_ColorSpaceDouble.rgb * lerp(_Exposure1, _Exposure2, _Blend);
				return half4(c1, 1);
			}
			ENDCG
		}
	}
 
	//CustomEditor "SkyboxPanoramicShaderGUI"
	Fallback Off
}