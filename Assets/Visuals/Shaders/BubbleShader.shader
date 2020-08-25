// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/BubbleShader"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0.8979101,1,0)
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
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform sampler2D _SpotsGrunge;
		uniform sampler2D _TextureSample1;


		float2 voronoihash30( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi30( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash30( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 //		if( d<F1 ) {
			 //			F2 = F1;
			 			float h = smoothstep(0.0, 1.0, 0.5 + 0.5 * (F1 - d) / smoothness); F1 = lerp(F1, d, h) - smoothness * h * (1.0 - h);mg = g; mr = r; id = o;
			 //		} else if( d<F2 ) {
			 //			F2 = d;
			 //		}
			 	}
			}
			return F1;
		}


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time30 = 14.07;
			float voronoiSmooth0 = 0.34;
			float2 panner35 = ( 1.0 * _Time.y * float2( 0,0.5 ) + v.texcoord.xy);
			float2 coords30 = panner35 * 2.05;
			float2 id30 = 0;
			float voroi30 = voronoi30( coords30, time30,id30, voronoiSmooth0 );
			float2 temp_cast_0 = (voroi30).xx;
			float simplePerlin2D29 = snoise( temp_cast_0*2.75 );
			simplePerlin2D29 = simplePerlin2D29*0.5 + 0.5;
			float3 temp_cast_1 = (( simplePerlin2D29 * 0.02 )).xxx;
			v.vertex.xyz += temp_cast_1;
		}

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
3072;368.2;1920;1018;1684.815;376.0046;1.485995;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1449.588,290.8989;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;24;-1176.829,132.9699;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-817.2888,1.499;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;False;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-903.2,205.5;Inherit;True;Property;_SpotsGrunge;SpotsGrunge;1;0;Create;True;0;0;False;0;False;-1;4c733b966ffbc6c48914bd73527d32fa;4c733b966ffbc6c48914bd73527d32fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-827.3415,826.6069;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;35;-530.9415,843.5068;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;5;-463.5999,-233.6;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;False;0,0.8979101,1,0;0,0.9226117,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-534.9885,142.4989;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-217.013,87.35583;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-219.7946,-12.43554;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;11;-1221.913,467.0558;Inherit;False;3;0;FLOAT2;0.1,0.1;False;1;FLOAT2;1,1;False;2;FLOAT;0.7;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;30;-241.0438,540.6072;Inherit;False;0;0;1;0;1;False;1;False;True;4;0;FLOAT2;0.98,0.68;False;1;FLOAT;14.07;False;2;FLOAT;2.05;False;3;FLOAT;0.34;False;2;FLOAT;0;FLOAT2;1
Node;AmplifyShaderEditor.RangedFloatNode;34;-49.94157,309.207;Inherit;False;Constant;_Float3;Float 3;3;0;Create;True;0;0;False;0;False;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;29;86.72342,526.8696;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-198.3999,-202.4997;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.9;False;2;FLOAT;18.37;False;3;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-6.594563,27.86447;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-615.8292,558.7701;Inherit;False;Constant;_Float4;Float 4;3;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-932.0291,416.9699;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;2c635ba82479d7a4b975a0a8bdf0a2df;2c635ba82479d7a4b975a0a8bdf0a2df;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;163.671,98.16968;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;21;129.9053,-194.4359;Inherit;False;ColorBurn;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;146.3585,274.1067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-441.729,388.0701;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;367.9,-13;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Amplify/BubbleShader;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;5;d3d11;glcore;gles;gles3;vulkan;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;15;0
WireConnection;8;1;24;0
WireConnection;35;0;36;0
WireConnection;13;0;12;0
WireConnection;13;1;8;0
WireConnection;10;0;5;0
WireConnection;10;1;13;0
WireConnection;11;0;15;0
WireConnection;30;0;35;0
WireConnection;29;0;30;0
WireConnection;17;0;10;0
WireConnection;17;1;18;0
WireConnection;26;1;11;0
WireConnection;21;0;17;0
WireConnection;21;1;1;0
WireConnection;33;0;29;0
WireConnection;33;1;34;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;0;2;21;0
WireConnection;0;4;22;0
WireConnection;0;9;28;0
WireConnection;0;11;33;0
ASEEND*/
//CHKSM=98799FDFC5DBEE37684F01DB786BA593B1B32C6B