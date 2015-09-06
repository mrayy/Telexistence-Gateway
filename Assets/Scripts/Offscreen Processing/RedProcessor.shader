Shader "Image/RedProcessor" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
 
			struct v2f 
			{
				float4 position : SV_POSITION;
				float2 texCoord  : TEXCOORD0;
			};
	 
			struct a2v
			{
				float4 vertex   : POSITION;
			};		

			v2f vert(a2v IN) {
				
				v2f Out;
				Out.position.xy=2*sign(IN.vertex.xy)-1;
				Out.position.z = 1.0;
				Out.position.w = 1.0;
				Out.texCoord.xy =sign(IN.vertex.xy)-0.5;// (Out.position.xy);
			   
			   return Out;
			}
			float4 frag(float2 texcoord : TEXCOORD0) :COLOR  {
				texcoord=texcoord*0.5-0.5;
			   	texcoord.y=1-texcoord.y;
				float4 c = tex2D (_MainTex, texcoord) * float4(1,0,0,1);
				return c;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
