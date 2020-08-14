// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyCollection/Foliage_Shader"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#include "TerrainEngine.cginc"
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float4 _Color0;


		float2 voronoihash22( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi22( float2 v, float time, inout float2 id, float smoothness )
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
			 		float2 o = voronoihash22( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return (F2 + F1) * 0.5;
		}


		float4 WindAnimateVertex1_g2( float4 Pos , float3 Normal , float4 AnimParams )
		{
			return AnimateVertex(Pos,Normal,AnimParams);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time22 = 0.0;
			float voronoiSmooth0 = 0.0;
			float2 uv_TexCoord18 = v.texcoord.xy * float2( 2,2 );
			float2 panner19 = ( 0.3 * _Time.y * float2( 0,0.4 ) + uv_TexCoord18);
			float2 coords22 = panner19 * 2.0;
			float2 id22 = 0;
			float voroi22 = voronoi22( coords22, time22,id22, voronoiSmooth0 );
			float4 temp_cast_0 = (( voroi22 * 0.1 )).xxxx;
			float4 Pos1_g2 = temp_cast_0;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 Normal1_g2 = ase_vertexNormal;
			float4 AnimParams1_g2 = float4( 0,0,0,0 );
			float4 localWindAnimateVertex1_g2 = WindAnimateVertex1_g2( Pos1_g2 , Normal1_g2 , AnimParams1_g2 );
			v.vertex.xyz += localWindAnimateVertex1_g2.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
3072;368.2;1920;1018;1384.589;288.4702;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-973.3662,-28.82904;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;21;-1051.181,194.4571;Inherit;False;Constant;_Speed;Speed;3;0;Create;True;0;0;False;0;False;0,0.4;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;19;-754.0051,194.5957;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;0.3;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-763.1323,349.3214;Inherit;False;Constant;_MovementRange;Movement Range;4;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;22;-574.731,193.5957;Inherit;False;0;0;1;3;1;False;1;False;True;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;2;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-482.1323,353.3214;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-499.8145,1031.88;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;16;-287.8145,1001.879;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;11;-1081.814,542.8794;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1347.814,933.8794;Inherit;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;False;4.7;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;13;-1231.814,710.8793;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-728.8146,767.8794;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-985.8146,816.8794;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-828.8146,1113.88;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;0.02179486;1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;15;-518.8146,817.8794;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;29;-308.1323,339.3214;Inherit;False;Terrain Wind Animate Vertex;-1;;2;3bc81bd4568a7094daabf2ccd6a7e125;0;3;2;FLOAT4;0,0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;34;-500.589,-11.97021;Inherit;False;Property;_Color0;Color 0;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.6451253,0.754717,0.1957992,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AmplifyCollection/Foliage_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;18;0
WireConnection;19;2;21;0
WireConnection;22;0;19;0
WireConnection;33;0;22;0
WireConnection;33;1;30;0
WireConnection;17;0;2;0
WireConnection;16;0;15;0
WireConnection;16;3;17;0
WireConnection;16;4;2;0
WireConnection;14;0;11;0
WireConnection;14;1;12;0
WireConnection;12;0;13;1
WireConnection;12;1;10;0
WireConnection;15;0;14;0
WireConnection;29;2;33;0
WireConnection;0;0;34;0
WireConnection;0;11;29;0
ASEEND*/
//CHKSM=E3D742AA0DEA74F9C6677FD55FB1D3A86E8EB24B