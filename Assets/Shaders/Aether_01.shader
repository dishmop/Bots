Shader "Custom noise/Aether_01" {
Properties {
	_RawData ("RawData", 2D) = "defaulttexture" {}
	_Width ("Width", int) = 0
	_Height ("Height", int) = 0
	_SpringConst ("SprintConst", float) = 0
}

SubShader {
       ZTest Off
//       Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha
	//	Blend SrcAlpha One // additive blending
	Pass {
		Tags {"Queue"="Overlay"}
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
		
		uniform sampler2D _RawData;
		uniform float _Width;
		uniform float _Height;
				

			
	//	uniform texture2D _RawData;

		
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
				float deltaX = 1f/_Width;
				float deltaY = 1f/_Height;
				
				float4 c = tex2D(_RawData, float2(i.uv.x, i.uv.y));
				float4 t = tex2D(_RawData, float2(i.uv.x, i.uv.y + deltaY));
				float4 b = tex2D(_RawData, float2(i.uv.x, i.uv.y - deltaY));
				float4 l = tex2D(_RawData, float2(i.uv.x - deltaX, i.uv.y));
				float4 r = tex2D(_RawData, float2(i.uv.x + deltaX, i.uv.y));
				
//				float diagX = 0.707106781186548 * deltaX;
//				float diagY = 0.707106781186548 * deltaY;

				float diagX = deltaX;
				float diagY = deltaY;
				
				float4 tr = tex2D(_RawData, float2(i.uv.x + diagX, i.uv.y + diagY));
				float4 br = tex2D(_RawData, float2(i.uv.x + diagX, i.uv.y - diagY));
				float4 tl = tex2D(_RawData, float2(i.uv.x - diagX, i.uv.y + diagY));
				float4 bl = tex2D(_RawData, float2(i.uv.x - diagX, i.uv.y - diagY));
				
				
			  	return  (2 * c + t + b + l + r + tr + br + tl + bl) / 10f;
		

		}
		
		ENDCG
	}
}

}