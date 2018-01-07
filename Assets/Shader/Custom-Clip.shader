// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Clip" {
	Properties {
		_HoleWidth("Hole Width", float) = 0.1
		_HoleHeight("Hole Height", float) = 0.1
		_StartPosX("Start PosX", float) = 0.5
		_StartPosY("Start PosY", float) = 0.5
		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)
	}
	
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				
			#include "UnityCG.cginc"
			
			float _HoleWidth;
			float _HoleHeight;
			float _StartPosX;
			float _StartPosY;
			fixed4 _Color;
		
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = float2(v.texcoord.x, v.texcoord.y);
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 col = _Color;
				float2 pos = i.uv;
				
				if(pos.x > _StartPosX - _HoleWidth * 0.5 && pos.x < _StartPosX + _HoleWidth*0.5 && pos.y < _StartPosY + _HoleHeight*0.5 && pos.y > _StartPosY - _HoleHeight*0.5){
					clip(-1.0);
				}
				
				return col;
			}	
			ENDCG
		}
	}
}
