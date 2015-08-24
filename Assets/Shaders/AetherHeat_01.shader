Shader "Custom noise/AetherHeat_01" {
Properties {
	_EmissionColor ("EmissionColor", Color) =  (0.0, 0.0, 0.0)
	_Color ("Color", Color) =  (0.0, 0.0, 0.0)
	
}

SubShader {
       ZTest Off
//       Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//Blend SrcAlpha One // additive blending
	Pass {
		Tags {"RenderType"="Test" "Queue"="Overlay"}
		 ZWrite off
		CGPROGRAM
		
		#pragma target 3.0
		
		#pragma vertex vert
		#pragma fragment frag
		

		 #include "UnityCG.cginc"
		
		struct v2f {
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
//			float3 srcPos1 : TEXCOORD1;
		};
		
		uniform float4 _EmissionColor;
		uniform float4 _Color;
			


		
		v2f vert(appdata_base v)
		{
			v2f o;

			o.pos =	mul(UNITY_MATRIX_MVP, v.vertex);
			
			//o.uv = o.pos * 0.5 + float4(0.5, 0.5, 0.5, 0.5);
			o.uv = v.texcoord;
		    //float4 worldPos = mul ((float4x4)_Object2World, v.vertex );
		    //o.uv = worldPos;// / _TextureScale;  //setting and scaling UVs 		
		    
		  
			
			return o;
		}
		
		
		
		float4 frag(v2f i) : COLOR
		{

			return float4(_Color.a, _Color.a, _Color.a, _Color.a);
			//return _EmissionColor ;
		}
		
		ENDCG
	}
}

}