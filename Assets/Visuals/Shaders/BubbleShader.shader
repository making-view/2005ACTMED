// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/BubbleShader"
{
	Properties
	{
		_Color0("Color 0", Color) = (1,0,0,0)
		_SpotsGrunge("SpotsGrunge", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma only_renderers d3d11 glcore gles gles3 vulkan 
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform sampler2D _SpotsGrunge;
		uniform sampler2D _TextureSample1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner24 = ( 1.0 * _Time.y * float2( 0.02,0 ) + i.uv_texcoord);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.9 + 18.37 * pow( 1.0 - fresnelNdotV1, 20.0 ) );
			float4 temp_cast_0 = (fresnelNode1).xxxx;
			float4 blendOpSrc21 = ( ( _Color0 * ( 0.9 * tex2D( _SpotsGrunge, panner24 ) ) ) * 4.0 );
			float4 blendOpDest21 = temp_cast_0;
			o.Emission = ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest21) / max( blendOpSrc21, 0.00001) ) ) )).rgb;
			o.Smoothness = 1.0;
			float cos11 = cos( 0.7 * _Time.y );
			float sin11 = sin( 0.7 * _Time.y );
			float2 rotator11 = mul( i.uv_texcoord - float2( 1,1 ) , float2x2( cos11 , -sin11 , sin11 , cos11 )) + float2( 1,1 );
			o.Alpha = ( tex2D( _TextureSample1, rotator11 ) * 0.5 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
3072;368.2;1920;1018;1837.177;521.3301;1.6;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1448.588,291.8989;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;24;-1180.029,132.9699;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-983.2,200.5;Inherit;True;Property;_SpotsGrunge;SpotsGrunge;1;0;Create;True;0;0;False;0;False;-1;4c733b966ffbc6c48914bd73527d32fa;4c733b966ffbc6c48914bd73527d32fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-838.0886,128.899;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-534.9885,142.4989;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-463.5999,-233.6;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;False;1,0,0,0;0,0.9226117,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-217.013,87.35583;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;11;-1221.913,467.0558;Inherit;False;3;0;FLOAT2;0.1,0.1;False;1;FLOAT2;1,1;False;2;FLOAT;0.7;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-219.7946,-12.43554;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-867.0291,689.9699;Inherit;False;Constant;_Float4;Float 4;3;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-6.594563,27.86447;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;1;-198.3999,-202.4997;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.9;False;2;FLOAT;18.37;False;3;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-932.0291,416.9699;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;2c635ba82479d7a4b975a0a8bdf0a2df;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-417.0291,456.9699;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;22;131.1709,397.1699;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;21;129.9053,-194.4359;Inherit;False;ColorBurn;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;367.9,-13;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Amplify/BubbleShader;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;5;d3d11;glcore;gles;gles3;vulkan;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;15;0
WireConnection;8;1;24;0
WireConnection;13;0;12;0
WireConnection;13;1;8;0
WireConnection;10;0;5;0
WireConnection;10;1;13;0
WireConnection;11;0;15;0
WireConnection;17;0;10;0
WireConnection;17;1;18;0
WireConnection;26;1;11;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;21;0;17;0
WireConnection;21;1;1;0
WireConnection;0;2;21;0
WireConnection;0;4;22;0
WireConnection;0;9;28;0
ASEEND*/
//CHKSM=34839776024988783AC79AAC85ACB4E8EFE56B04