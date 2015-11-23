Shader "Telexistence/Demo/WebCamShader_Doors" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass
		{
			LOD 200
			
			Lighting Off
			ZWrite Off
			Cull Off
			Fog { Mode off }
		
				// Only render pixels whose value in the stencil buffer equals 1.
			Stencil {
			  Ref 1
			  Comp Equal
			}

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "TelexistenceCG.cginc"

			sampler2D _MainTex;


			float2 PixelShift=float2(0,0);
			float2 TextureSize=float2(1,1);

			struct v2f {
			    float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			};
			v2f vert(appdata_base  v) {
			    v2f o;
			    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			    o.uv = v.texcoord;
			    return o;
			}
			half4 frag(v2f IN) : SV_Target {
				IN.uv+=PixelShift/TextureSize;
				float4 c;
				float2 tc=_CorrectDistortion(IN.uv);
				if (any(clamp(tc, float2(0.0,0.0), float2(1.0, 1.0)) - tc))    
					c=0;
				else			
					c = tex2D (_MainTex, tc);
				c.a=1;
				return c;
			}

	        ENDCG
	        
		}
	} 
}
