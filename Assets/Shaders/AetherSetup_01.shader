Shader "Custom noise/AetherSetup_01" {
Properties {
	
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
		

			


		
		v2f vert(appdata_base v)
		{
			v2f o;

			o.pos =	mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv = v.texcoord;
		    //float4 worldPos = mul ((float4x4)_Object2World, v.vertex );
		    //o.uv = worldPos;// / _TextureScale;  //setting and scaling UVs 		
		    
		  
			
			return o;
		}
		

		
		float4 frag(v2f i) : COLOR
		{
		
			if (i.uv.x < 0.55 && i.uv.x > 0.45 && i.uv.y < 0.55 && i.uv.y > 0.45){
				return float4(1, 0, 0, 1);
			}
			else{
				return float4(0, 0, 0, 1);
			}
	
		
			//return float4(i.uv.x, i.uv.y, 0, 0);
		}
		
		ENDCG
	}
}

}