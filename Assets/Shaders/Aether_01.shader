Shader "Custom noise/Aether_01" {
Properties {
	_RawData01 ("RawData01", 2D) = "defaulttexture" {}
	_RawData02 ("RawData02", 2D) = "defaulttexture" {}
	_RawData03 ("RawData03", 2D) = "defaulttexture" {}
	_RawData04 ("RawData04", 2D) = "defaulttexture" {}
	
	_History_0 ("History_0", 2D) = "defaulttexture" {}
	_History_1 ("History_1", 2D) = "defaulttexture" {}
	_History_2 ("History_2", 2D) = "defaulttexture" {}
	_History_3 ("History_3", 2D) = "defaulttexture" {}
	_History_4 ("History_4", 2D) = "defaulttexture" {}
	_History_5 ("History_5", 2D) = "defaulttexture" {}
	_History_6 ("History_6", 2D) = "defaulttexture" {}
	_History_7 ("History_7", 2D) = "defaulttexture" {}
	_History_8 ("History_8", 2D) = "defaulttexture" {}
	_History_9 ("History_9", 2D) = "defaulttexture" {}
	_History_10 ("History_10", 2D) = "defaulttexture" {}
	_History_11 ("History_11", 2D) = "defaulttexture" {}
	
	
	_Width ("Width", int) = 0
	_Height ("Height", int) = 0
	_SpringConst ("SprintConst", float) = 0
	_Drag ("Drag", float) = 0
	_IntTime ("IntTime", int) = 0
	_BufferIndex ("BufferIndex", int) = 0
	
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
		
		uniform sampler2D _RawData01;
		uniform sampler2D _RawData02;
		uniform sampler2D _RawData03;
		uniform sampler2D _RawData04;
		
		
		uniform sampler2D _History_0;
		uniform sampler2D _History_1;
		uniform sampler2D _History_2;
		uniform sampler2D _History_3;
		uniform sampler2D _History_4;
		uniform sampler2D _History_5;
		uniform sampler2D _History_6;
		uniform sampler2D _History_7;
		uniform sampler2D _History_8;
		uniform sampler2D _History_9;
		uniform sampler2D _History_10;
		uniform sampler2D _History_11;

				
		uniform float _Width;
		uniform float _Height;
		uniform int _IntTime;
		uniform int _BufferIndex;
				

			
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
			return float4(0, 1, 1, 0);
		}
		

		
		ENDCG
	}
}

}