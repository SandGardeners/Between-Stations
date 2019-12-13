// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Noise-Simplex" {
Properties {
	_Freq ("Frequency", Float) = 1
	_Speed ("Speed", Float) = 1
	_Min ("Min", Float) = 0.490
	_Max ("Max", Float) = 0.495
	_MainTex ("Albedo", 2D) = "defaulttexture" {}
	_MaskTex ("Mask", 2D) = "defaulttexture" {}
	_Alpha("Alpha", Float) = 1
	 _Color ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
 _StencilComp ("Stencil Comparison", Float) = 8.000000
 _Stencil ("Stencil ID", Float) = 0.000000
 _StencilOp ("Stencil Operation", Float) = 0.000000
 _StencilWriteMask ("Stencil Write Mask", Float) = 255.000000
 _StencilReadMask ("Stencil Read Mask", Float) = 255.000000
 _ColorMask ("Color Mask", Float) = 15.000000
}

SubShader {
	Tags {"Queue"="Transparent" "RenderType"="Transparent"}
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	Pass {
		CGPROGRAM
		
		#pragma target 3.0 
		
		#pragma vertex vert
		#pragma fragment frag
		#include "SimplexNoise2D.hlsl"
		
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 srcPos : TEXCOORD0;
			float3 noisePos : TEXCOORD1;
		};
		
		sampler2D _MainTex;
		sampler2D _MaskTex;

		uniform float
			_Freq,
			_Speed,
			_Min,
			_Max,
			_Alpha;
		
		v2f vert(appdata_t IN)
		{
			v2f o;

			o.pos =	UnityObjectToClipPos(IN.vertex);
			o.srcPos = IN.texcoord;
			o.noisePos = mul(unity_ObjectToWorld, IN.vertex).xyz;
			o.noisePos *= _Freq;
			o.noisePos.y += _Time.x * _Speed;

			
			
			return o;
		}
		
		float4 frag(v2f i) : COLOR
		{
			float ns = snoise(i.noisePos)*0.5+0.5;
			float4 c = tex2D(_MainTex, i.srcPos);
			float4 c2 = tex2D(_MainTex, i.noisePos);
			float4 cc = ns>_Min&&ns<_Max?c2*ns:c*ns;
			
			// fract(ns);
			float4 olol = smoothstep(cc, c*ns, cos(_Time.x)*0.1);
			olol.a = _Alpha;//,fract(c*(sin(_Time.x)*0.5 + 0.5)*40.0));
			return tex2D(_MaskTex, i.srcPos).a*olol;
//			return float4(ns*c.x*0.5,ns*c.y*0.5, ns*c.z*0.5, ns);
		}
		
		ENDCG
	}
}

}