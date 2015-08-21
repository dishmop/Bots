Shader "Custom noise/AetherPropagate_01" {
Properties {
	_RawData01 ("RawData01", 2D) = "defaulttexture" {}
	_RawData02 ("RawData02", 2D) = "defaulttexture" {}
	_RawData03 ("RawData03", 2D) = "defaulttexture" {}
	_Width ("Width", int) = 0
	_Height ("Height", int) = 0
	_IntTime ("IntTime", int) = 0
}

SubShader {
//       ZTest Off
//       Cull Off
		Blend Off
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
		
		uniform sampler2D _RawData01;
		uniform sampler2D _RawData02;
		uniform sampler2D _RawData03;
		uniform float _Width;
		uniform float _Height;
		uniform int _IntTime;
		uniform float _Drag;
				

			
	//	uniform texture2D _RawData;

		
		v2f vert(appdata_base v)
		{
			v2f o;

			o.pos =	mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv = o.pos * 0.5 + float4(0.5, 0.5, 0.5, 0.5);
		    //float4 worldPos = mul ((float4x4)_Object2World, v.vertex );
		    //o.uv = worldPos;// / _TextureScale;  //setting and scaling UVs 		
		    

			
			return o;
		}
		
		
		float4 frag(v2f i) : COLOR
		{
				float deltaX = 1/_Width;
				float deltaY = 1/_Height;
				

				
				float4 northCoord = (i.uv + float4(0, 		deltaY, 0, 0));
				float4 eastCoord = (i.uv + float4(deltaX, 	0, 0, 0));
				float4 southCoord = (i.uv + float4(0, 		-deltaY, 0, 0));
				float4 westCoord = (i.uv + float4(-deltaX,	 0, 0, 0));
				


				
				// North, east, south, west
				float4 north = tex2D(_RawData02, northCoord);
				float4 east = tex2D(_RawData02, eastCoord);
				float4 south = tex2D(_RawData02, southCoord);
				float4 west = tex2D(_RawData02, westCoord);
				
				return float4(north[2], east[3], south[0], west[1]);

		}
		
//
//		
//		float4 frag2(v2f i) : COLOR
//		{
//				float4 texSize = float4(_Width, _Height, 0, 0);
//				float4 base = floor(i.uv * texSize);
//				
//				float4 northCoord = (base + float4(0, 1, 0, 0)) / texSize;
//				float4 eastCoord = (base + float4(1, 0, 0, 0)) / texSize;
//				float4 southCoord = (base + float4(0, -1, 0, 0)) / texSize;
//				float4 westCoord = (base + float4(-1, 0, 0, 0)) / texSize;
//				
//
//
//				
//				// North, east, south, west
//				float4 north = tex2D(_RawData02, northCoord);
//				float4 east = tex2D(_RawData02, eastCoord);
//				float4 south = tex2D(_RawData02, southCoord);
//				float4 west = tex2D(_RawData02, westCoord);
//				
//				return float4(north[2], east[3], south[0], west[1]);
//
//		}
//		
		ENDCG
	}
}

}