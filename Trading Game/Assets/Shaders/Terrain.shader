Shader "Custom/Terrain" {
	Properties {
		_ShowBeach1 ("Show Beach 1", float) = 0
		_ShowBeach2 ("Show Beach 2", float) = 0	
		_ShowBeach3 ("Show Beach 3", float) = 0	
		_ShowBeach4 ("Show Beach 4", float) = 0	
		_ShowBeach5 ("Show Beach 5", float) = 0	
		_ShowBeach6 ("Show Beach 6", float) = 0		
										
		_MainTex ("Base (RGB)", 2D) = "" {}
		_Beach1 ("Beach 1 (RGBA)", 2D) = "black" {}
		_Beach2 ("Beach 2 (RGBA)", 2D) = "black" {}
		_Beach3 ("Beach 3 (RGBA)", 2D) = "black" {}
		_Beach4 ("Beach 4 (RGBA)", 2D) = "black" {}
		_Beach5 ("Beach 5 (RGBA)", 2D) = "black" {}
		_Beach6 ("Beach 6 (RGBA)", 2D) = "black" {}																			
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Beach1;
		sampler2D _Beach2;
		sampler2D _Beach3;
		sampler2D _Beach4;
		sampler2D _Beach5;
		sampler2D _Beach6;
		
		float _ShowBeach1;
		float _ShowBeach2;
		float _ShowBeach3;
		float _ShowBeach4;
		float _ShowBeach5;
		float _ShowBeach6;										

		struct Input {
			float2 uv_MainTex;
			float2 uv2_Beach1;
			float2 uv2_Beach2;
			float2 uv2_Beach3;
			float2 uv2_Beach4;	
			float2 uv2_Beach5;	
			float2 uv2_Beach6;								
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 base = tex2D (_MainTex, IN.uv_MainTex);
			half4 beach1 = tex2D (_Beach1, IN.uv2_Beach1);
			half4 beach2 = tex2D (_Beach2, IN.uv2_Beach2);
			half4 beach3 = tex2D (_Beach3, IN.uv2_Beach3);	
			half4 beach4 = tex2D (_Beach4, IN.uv2_Beach4);		
			half4 beach5 = tex2D (_Beach5, IN.uv2_Beach5);	
			half4 beach6 = tex2D (_Beach6, IN.uv2_Beach6);												
//			half4 alpha = tex2D (_SecondaryAlpha, IN.uv2_SecondaryAlpha);
			o.Albedo = lerp(base.rgb, beach1.rgb, beach1.a * _ShowBeach1);
			o.Albedo = lerp(o.Albedo, beach2.rgb, beach2.a * _ShowBeach2);
			o.Albedo = lerp(o.Albedo, beach3.rgb, beach3.a * _ShowBeach3);
			o.Albedo = lerp(o.Albedo, beach4.rgb, beach4.a * _ShowBeach4);
			o.Albedo = lerp(o.Albedo, beach5.rgb, beach5.a * _ShowBeach5);
			o.Albedo = lerp(o.Albedo, beach6.rgb, beach6.a * _ShowBeach6);												
			o.Alpha = base.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
