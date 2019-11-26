Shader "Custom/LightningShader1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Speed("speed", Range(1,100)) = 42
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SecondTex("Albedo (RGB)", 2D) = "white" {}
		_ThirdTex("Albedo (RGB)", 2D) = "white" {}
		_ScopeTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.3
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondTex;
		sampler2D _ThirdTex;
		sampler2D _ScopeTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondTex;
			float2 uv_ThirdTex;
			float2 uv_ScopeTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Speed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			fixed2 scrolledUV = IN.uv_MainTex;
			fixed2 scrolledSV = IN.uv_SecondTex;
			fixed2 scrolledTV = IN.uv_ThirdTex;
			fixed2 scrolledscope = IN.uv_ScopeTex;

			fixed scrollValue = _Time * _Speed;
			scrollValue += fixed2(scrollValue, 0);
			scrollValue = _Time * _Speed / 1.28f;

			fixed scrollValue2 = _Time * _Speed;
			scrollValue2 += fixed2(scrollValue2,0 );
			scrollValue2 = _Time * _Speed /1.13f ;

			fixed scrollValueZERO = _Time * _Speed;
			scrollValueZERO += fixed2(scrollValueZERO, 0);
			scrollValueZERO = _Time * _Speed / 1.33f;

			fixed2 scrolledUV2 = IN.uv_MainTex + fixed2(scrollValue, 0); //déplacemenet droit
			fixed2 scrolledUVS2 = IN.uv_SecondTex + fixed2(0, scrollValue2); //déplacemenet haut en bas
			fixed2 scrolledUVT2 = IN.uv_ThirdTex - fixed2(scrollValue, 0);//déplacemenet gauche
			fixed2 scrolledUScopeT2 = IN.uv_ScopeTex + fixed2(0, 0);



			fixed4 c = tex2D(_MainTex , scrolledUV2) * tex2D(_ThirdTex, scrolledUVT2);
			fixed4 c2 = tex2D(_SecondTex, scrolledSV) * tex2D(_MainTex, scrolledUV2);
			fixed4 c3 = tex2D(_ThirdTex, scrolledTV) * tex2D(_SecondTex, scrolledUVS2);

			fixed4 c4 = tex2D(_ScopeTex, IN.uv_ScopeTex);
			fixed3 c5 = c.rgb + c2.rgb + c3.rgb;
			o.Albedo = pow(c5,0.8f) + pow(c5.b*3, 0.2f);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = min((c5.r + c5.g + c5.b), 1 - c4.a);
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
