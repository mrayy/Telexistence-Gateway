Shader "Custom/WebCamShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Lighting Off
		ZWrite Off
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert


		sampler2D _MainTex;
		

		struct Input {
			float2 uv_MainTex;
		};
		
		float2 LensCenter=float2(0.5,0.5);
		float2 FocalLength=float2(1,1);
		float4 WrapParams=float4(1,1,1,1);
		
		float2 _CorrectDistortion(float2 uv)
		{ 
			float2 xy=(uv-LensCenter)/FocalLength;

			float r=sqrt(dot(xy,xy));
			float r2=r*r;
			float r4=r2*r2;
			float coeff=(WrapParams.x*r2+WrapParams.y*r4); //radial factor

			float dx=WrapParams.z*2.0*xy.x*xy.y    + WrapParams.w*(r2+2.0*xy.x*xy.x);
			float dy=WrapParams.z*(r2+2.0*xy.y*xy.y) + WrapParams.w*2.0*xy.x*xy.y;

			xy=((xy+xy*coeff.xx+float2(dx,dy))*FocalLength+LensCenter);
		    return xy;
		    
		}
		void surf (Input IN, inout SurfaceOutput o) {
		
			float2 tc=_CorrectDistortion(IN.uv_MainTex);
			float4 c;
			if (any(clamp(tc, float2(0.0,0.0), float2(1.0, 1.0)) - tc))    
				c=0;
			else			
				c = tex2D (_MainTex, tc);
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission=c.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
