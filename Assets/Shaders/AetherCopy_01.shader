Shader "Custom noise/AetherCopy_01" {
Properties {
	_RawData01 ("RawData01", 2D) = "defaulttexture" {}
	_RawData02 ("RawData02", 2D) = "defaulttexture" {}
	_RawData03 ("RawData03", 2D) = "defaulttexture" {}

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
			return  tex2D(_RawData01, i.uv);
			
		}
		

		ENDCG
	}
}

}