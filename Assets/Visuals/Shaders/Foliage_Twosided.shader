// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/Foliage_Twosided"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Colour("Colour", 2D) = "white" {}
		_A_M_S("A_M_S", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_OpacityCutout("Opacity Cutout", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Colour;
		uniform float4 _Colour_ST;
		uniform sampler2D _A_M_S;
		uniform float4 _A_M_S_ST;
		uniform sampler2D _OpacityCutout;
		uniform float4 _OpacityCutout_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = tex2D( _Normal, uv_Normal ).rgb;
			float2 uv_Colour = i.uv_texcoord * _Colour_ST.xy + _Colour_ST.zw;
			o.Albedo = tex2D( _Colour, uv_Colour ).rgb;
			float2 uv_A_M_S = i.uv_texcoord * _A_M_S_ST.xy + _A_M_S_ST.zw;
			float4 tex2DNode2 = tex2D( _A_M_S, uv_A_M_S );
			o.Metallic = tex2DNode2.b;
			o.Smoothness = tex2DNode2.g;
			o.Occlusion = tex2DNode2.r;
			o.Alpha = 1;
			float2 uv_OpacityCutout = i.uv_texcoord * _OpacityCutout_ST.xy + _OpacityCutout_ST.zw;
			clip( tex2D( _OpacityCutout, uv_OpacityCutout ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;0;2560;1379;1185;460;1;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-553,-102;Inherit;True;Property;_Colour;Colour;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-550,94;Inherit;True;Property;_Normal;Normal;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-522,324;Inherit;True;Property;_A_M_S;A_M_S;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-518,524;Inherit;True;Property;_OpacityCutout;Opacity Cutout;4;0;Create;True;0;0;False;0;-1;None;da225b0c7f6a4e04899586c942121e17;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;155,-31;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Amplify/Foliage_Twosided;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;3;2;3
WireConnection;0;4;2;2
WireConnection;0;5;2;1
WireConnection;0;10;4;0
ASEEND*/
//CHKSM=31471EBB3390521666293F7010F45CF46354BAA3