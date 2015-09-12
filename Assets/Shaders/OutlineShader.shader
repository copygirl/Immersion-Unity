//
// "Silhouette-Outlined Diffuse" shader by AnomalousUnderdog
// (Modified to only render the outline pass.)
// Source: http://wiki.unity3d.com/index.php/Silhouette-Outlined_Diffuse
//

Shader "Outlined/Silhouetted Diffuse" {
	Properties {
		_Color ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline Width", Range (0.0, 0.03)) = 0.005
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _Color;
 
v2f vert(appdata v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	
	float3 norm   = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
	float2 offset = TransformViewToProjection(norm.xy);
	
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _Color;
	return o;
}
ENDCG
	
	SubShader {
		Pass {
			Name "OUTLINE"
			Tags {
				"Queue" = "Overlay"
				"LightMode" = "Always"
			}
			Cull Off
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

half4 frag(v2f i) : COLOR {
	return i.color;
}
ENDCG
		}
	}
	
	SubShader {
		Pass {
			Name "OUTLINE"
			Tags {
				"Queue" = "Overlay"
				"LightMode" = "Always"
			}
			Cull Front
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			SetTexture [_MainTex] { combine primary }
		}
	}
	
	Fallback "Diffuse"
}
