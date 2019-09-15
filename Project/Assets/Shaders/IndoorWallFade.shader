﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/IndoorWallFade" {
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		_FadeOffset("Fade Offset", Range(0, 10)) = 1
		_FadeLength("Fade Length", Range(0, 10)) = 3
	}
		SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On

		Tags
	{
		"RenderType" = "Transparent"
		"Queue" = "Transparent"
	}

		Pass
	{
		CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata v, out float4 vertex : SV_POSITION)
	{
		v2f o;
		vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		return o;
	}

	sampler2D _CameraDepthTexture;
	fixed4 _Color;
	fixed3 _GlowColor;
	float _FadeLength;
	float _FadeOffset;

	fixed4 frag(v2f i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
	{
		float2 screenuv = vpos.xy / _ScreenParams.xy;
		float screenDepth = Linear01Depth(tex2D(_CameraDepthTexture, screenuv));
		float diff = screenDepth - Linear01Depth(vpos.z);
		float intersect = 0;

		if (diff > 0)
			intersect = 1 - smoothstep(0, _ProjectionParams.w * _FadeLength, diff - (_FadeOffset * 0.01f));

		fixed4 glowColor = fixed4(_GlowColor, intersect);
		return glowColor;
	}
		ENDCG
	}
	}
}
