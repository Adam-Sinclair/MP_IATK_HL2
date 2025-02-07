﻿Shader "DUAL/Dual_Textured_Lightup" {
	Properties{

		_Fade1("Fade 1", Range(0.0,1.0)) = 1.0
		_Fade2("Fade 2", Range(0.0,1.0)) = 1.0

		// standard
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5

		_Stencil("Stencil (A)", 2D) = "white"{}

		_Lightup1("Lightup 1", Range(0.0,1.0)) = 0.0
		_Color1("Color 1", Color) = (1,1,1,1)
		_Tex1("Texture 1 (RGB)", 2D) = "white" {}

		_Lightup2("Lightup 2", Range(0.0,1.0)) = 0.0
		_Color2("Color 2", Color) = (1,1,1,1)
		_Tex2("Texture 2 (RGB)", 2D) = "white"{}

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 300

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			fixed4 _Color1;
			fixed4 _Color2;
			sampler2D _Tex1;
			sampler2D _Stencil;
			sampler2D _Tex2;

			float _Lightup1;
			float _Lightup2;
			float _Fade1;
			float _Fade2;

			// Standard
			half _Glossiness;
			half _Metallic;

			struct Input {
				float2 uv_Tex1;
				float2 uv_Stencil;
				float2 uv_Tex2;
			};

			void surf(Input IN, inout SurfaceOutputStandard o) {

				half4 FirstColor = tex2D(_Tex1, IN.uv_Tex1);
				half4 SecondColor = tex2D(_Tex2, IN.uv_Tex2);

				float Stencil = tex2D(_Stencil, IN.uv_Stencil).a;
				float Opposite_stencil = 1.0 - Stencil;

				float first_stencil_value = (Stencil * _Fade1) + (Opposite_stencil * (1 - _Fade2));
				float second_stencil_value = (Opposite_stencil * _Fade2) + (Stencil * (1 - _Fade1));

				o.Albedo = (FirstColor.rgb * _Color1.rgb * first_stencil_value) +
						   (SecondColor.rgb * _Color2.rgb * second_stencil_value);

				o.Alpha = (FirstColor.a * _Color1.a * first_stencil_value) +
						  (SecondColor.a * _Color2.a * second_stencil_value);

				o.Emission = (FirstColor.rgb * _Color1.rgb * first_stencil_value * _Lightup1) +
							 (SecondColor.rgb * _Color2.rgb * second_stencil_value * _Lightup2);

				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;

			}
			ENDCG
	}
		FallBack "DUAL/Dual_Textured_Diffuse"
}