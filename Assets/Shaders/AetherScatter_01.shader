Shader "Custom noise/AetherScatter_01" {
Properties {
	_RawData01 ("RawData01", 2D) = "defaulttexture" {}
	_RawData02 ("RawData02", 2D) = "defaulttexture" {}
	_RawData03 ("RawData03", 2D) = "defaulttexture" {}
	_RawData04 ("RawData04", 2D) = "defaulttexture" {}
	_Width ("Width", int) = 0
	_Height ("Height", int) = 0
	_IntTime ("IntTime", int) = 0
}

SubShader {
       ZTest Off
//       Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha
	//	Blend SrcAlpha One // additive blending
			Blend Off

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
		
		uniform float _Width;
		uniform float _Height;
		uniform float _SpringConst;
		uniform float _Drag;
		uniform int _IntTime;
				

			
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
		
		float4 frag2(v2f i) : COLOR
		{

			// North, east, south, west
			float4 incoming = tex2D(_RawData01, float2(i.uv.x, i.uv.y));
			
			// input is "incoming" pressure
			return incoming;

		}
		
		float4 addSources3(float4 uv){
			
			float4 source = tex2D(_RawData03, uv)[0];
			
			float value = source.r * source.g;
			
			return float4(value, value, value, value);
			
		}
		
		float4 addSources4(float4 uv){
			// samples per wavelength
			float freqB = 8;
			float freqG = 10;
			float freqR = 12;
			
			float PI = 3.14159;
			
			float floatTime = _IntTime;
			
			
			float4 source = tex2D(_RawData03, uv);
			float waveValue = sin(2 * PI * floatTime * source.g + uv.x*1000);
			
			float value = source.r * waveValue;
			//float value = source;
			
			return float4(value, value, value, value);
			
		}
		
		
		float4 addSources(float4 uv){
			// samples per wavelength
			float freqR = 12;
			
			float PI = 3.14159;
			
			float floatTime = _IntTime + 100 * sin(cos(_IntTime) * 100*sin(uv.x * 100 + uv.y));
			
			
			float4 source = tex2D(_RawData03, uv);
			float value = 
				0.25 * source.r * sin(2 * PI * floatTime / freqR);
			//float value = source;
			
			return float4(value, value, value, value);
			
		}
		
		float4 addSources2(float4 uv){
			// samples per wavelength
			float freqB = 8;
			float freqG = 10;
			float freqR = 12;
			
			float PI = 3.14159;
			
			float floatTime = _IntTime;
			
			
			float4 source = tex2D(_RawData03, uv);
			float value = 
				0.25 * source.r * sin(2 * PI * floatTime / freqR) + 
				0.25 * source.g * sin(2 * PI * floatTime / freqG) + 
				0.25 * source.b * sin(2 * PI * floatTime / freqB);
			//float value = source;
			
			return float4(value, value, value, value);
			
		}
		
		float4 mulSink(float4 uv){
			// samples per wavelength
			
			
			float4 source = tex2D(_RawData04, uv);
			
			if (source.r  + source.g + source.b > 0.01){
				return 0;
			}
			else{
				return 1;
			}

			
		}

		
		float4 frag(v2f i) : COLOR
		{
		
//			float deltaX = 1/_Width;
//			float deltaY = 1/_Height;
//			
//			float border = 20;
//			if (i.uv.x < border * deltaX || i.uv.x > 1 - border * deltaX || i.uv.y < border * deltaY || i.uv.y > 1 - border * deltaY){
//				return float4(0, 0, 0, 0);
//			}

			// North, east, south, west
			float4 incoming = tex2D(_RawData01, float2(i.uv.x, i.uv.y));
			
			// input is "incoming" pressure
			return mulSink(i.uv) * float4( 
				0.5 * (-incoming[0] + incoming[1] + incoming[2] + incoming[3]),
				0.5 * ( incoming[0] - incoming[1] + incoming[2] + incoming[3]),
				0.5 * ( incoming[0] + incoming[1] - incoming[2] + incoming[3]),
				0.5 * ( incoming[0] + incoming[1] + incoming[2] - incoming[3])) +  addSources(i.uv);

		}
		
		ENDCG
	}
}

}