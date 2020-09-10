// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/CaveWall"
{
	Properties
	{
		_Colour1("Colour 1", 2D) = "white" {}
		_Colour2("Colour 2", 2D) = "white" {}
		_Normal1("Normal 1", 2D) = "bump" {}
		_Normal2("Normal 2", 2D) = "bump" {}
		_MetalSmooth("MetalSmooth", 2D) = "white" {}
		_ColourSwitch("ColourSwitch", Range( 0 , 1)) = 0
		_NormalSwitch("NormalSwitch", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal1;
		uniform float4 _Normal1_ST;
		uniform sampler2D _Normal2;
		uniform float4 _Normal2_ST;
		uniform float _NormalSwitch;
		uniform sampler2D _Colour1;
		uniform float4 _Colour1_ST;
		uniform sampler2D _Colour2;
		uniform float4 _Colour2_ST;
		uniform float _ColourSwitch;
		uniform sampler2D _MetalSmooth;
		uniform float4 _MetalSmooth_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal1 = i.uv_texcoord * _Normal1_ST.xy + _Normal1_ST.zw;
			float2 uv_Normal2 = i.uv_texcoord * _Normal2_ST.xy + _Normal2_ST.zw;
			float3 lerpResult27 = lerp( UnpackNormal( tex2D( _Normal1, uv_Normal1 ) ) , UnpackNormal( tex2D( _Normal2, uv_Normal2 ) ) , _NormalSwitch);
			o.Normal = lerpResult27;
			float2 uv_Colour1 = i.uv_texcoord * _Colour1_ST.xy + _Colour1_ST.zw;
			float2 uv_Colour2 = i.uv_texcoord * _Colour2_ST.xy + _Colour2_ST.zw;
			float4 lerpResult24 = lerp( tex2D( _Colour1, uv_Colour1 ) , tex2D( _Colour2, uv_Colour2 ) , _ColourSwitch);
			o.Albedo = lerpResult24.rgb;
			float2 uv_MetalSmooth = i.uv_texcoord * _MetalSmooth_ST.xy + _MetalSmooth_ST.zw;
			float4 tex2DNode7 = tex2D( _MetalSmooth, uv_MetalSmooth );
			o.Metallic = tex2DNode7.r;
			o.Smoothness = tex2DNode7.a;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
700;138;481;545;534.3196;161.9957;1;True;True
Node;AmplifyShaderEditor.SamplerNode;3;-631.2,-300.2;Inherit;True;Property;_Colour1;Colour 1;0;0;Create;True;0;0;False;0;False;-1;15fe4b4c4f8b9ef438c0cf2d024033ca;d9937729cebac8c468ebfe39783ada1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-794.0817,263.463;Inherit;True;Property;_Normal2;Normal 2;3;0;Create;True;0;0;False;0;False;-1;None;744e269b4fed7fd4d99b277e1e3639bc;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-794.8998,76.69991;Inherit;True;Property;_Normal1;Normal 1;2;0;Create;True;0;0;False;0;False;-1;None;7b00dea07109246428e82d447a11ec0a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-631.5821,-105.7369;Inherit;True;Property;_Colour2;Colour 2;1;0;Create;True;0;0;False;0;False;-1;885fcb5548440e8409ead9f60203b90c;885fcb5548440e8409ead9f60203b90c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-443.226,64.9671;Inherit;False;Property;_ColourSwitch;ColourSwitch;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-508.2512,305.0864;Inherit;False;Property;_NormalSwitch;NormalSwitch;6;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;24;-209.3807,-132.7371;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-426.6,379.3005;Inherit;True;Property;_MetalSmooth;MetalSmooth;4;0;Create;True;0;0;False;0;False;-1;None;61176dd43deed1d48b17dbbc11cb5846;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;27;-275.2512,143.0864;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Amplify/CaveWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;4;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;3;0
WireConnection;24;1;16;0
WireConnection;24;2;25;0
WireConnection;27;0;6;0
WireConnection;27;1;15;0
WireConnection;27;2;26;0
WireConnection;0;0;24;0
WireConnection;0;1;27;0
WireConnection;0;3;7;0
WireConnection;0;4;7;4
ASEEND*/
//CHKSM=402D0462ED01BE1535081EF1A18316B60E9A896F