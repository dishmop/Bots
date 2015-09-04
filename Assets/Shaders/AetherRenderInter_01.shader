Shader "Custom noise/AetherRender_01" {
Properties {
	_RawData01 ("RawData01", 2D) = "defaulttexture" {}
	_RawData02 ("RawData02", 2D) = "defaulttexture" {}
	_Width ("Width", int) = 0
	_Height ("Height", int) = 0
	_SpringConst ("SprintConst", float) = 0
	_Drag ("Drag", float) = 0
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
		uniform float _Width;
		uniform float _Height;
		uniform float _SpringConst;
		uniform float _Drag;
				

			
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
				float4 incoming = tex2D(_RawData01, float2(i.uv.x, i.uv.y));
				float pressure = 0.5 *(incoming[0] + incoming[1] + incoming[2] + incoming[3]);
				return float4(pressure, pressure, pressure, pressure);
//		

		}
		
		ENDCG
	}
}

}