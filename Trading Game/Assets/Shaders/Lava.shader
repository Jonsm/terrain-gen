Shader "Custom/Lava" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OverlayTex ("Overlay (RGBA)", 2D) = "white" {}
		_Offset ("Offset", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		float _Offset;
		sampler2D _MainTex;
		sampler2D _OverlayTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_OverlayTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 offsetUV = IN.uv_OverlayTex - _SinTime.r * _SinTime.r;
			if (offsetUV.x >= 1) offsetUV.x = offsetUV.x - 1;
			if (offsetUV.y >= 1) offsetUV.y = offsetUV.y - 1;
			
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 overlay = tex2D (_OverlayTex, offsetUV);
			o.Albedo = c.rgb + overlay.rgb * overlay.a;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
