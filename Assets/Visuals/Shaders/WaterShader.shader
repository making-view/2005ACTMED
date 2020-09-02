// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyCollection/WaterShader"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _TextureSample0;


		float2 voronoihash4( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi4( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -2; j <= 2; j++ )
			{
				for ( int i = -2; i <= 2; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash4( n + g );
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time4 = _CosTime.z;
			float2 uv_TexCoord6 = v.texcoord.xy * float2( 2,2 );
			float2 panner5 = ( 1.0 * _Time.y * float2( 0.02,0.01 ) + uv_TexCoord6);
			float cos7 = cos( panner5.x );
			float sin7 = sin( panner5.x );
			float2 rotator7 = mul( uv_TexCoord6 - float2( 0.5,0.5 ) , float2x2( cos7 , -sin7 , sin7 , cos7 )) + float2( 0.5,0.5 );
			float2 coords4 = rotator7 * 20.0;
			float2 id4 = 0;
			float fade4 = 0.5;
			float voroi4 = 0;
			float rest4 = 0;
			for( int it4 = 0; it4 <2; it4++ ){
			voroi4 += fade4 * voronoi4( coords4, time4, id4,0 );
			rest4 += fade4;
			coords4 *= 2;
			fade4 *= 0.5;
			}//Voronoi4
			voroi4 /= rest4;
			float3 temp_cast_1 = (voroi4).xxx;
			v.vertex.xyz += temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord6 = i.uv_texcoord * float2( 2,2 );
			float2 panner5 = ( 1.0 * _Time.y * float2( 0.02,0.01 ) + uv_TexCoord6);
			o.Normal = UnpackScaleNormal( tex2D( _TextureSample0, panner5 ), 2.0 );
			float time4 = _CosTime.z;
			float cos7 = cos( panner5.x );
			float sin7 = sin( panner5.x );
			float2 rotator7 = mul( uv_TexCoord6 - float2( 0.5,0.5 ) , float2x2( cos7 , -sin7 , sin7 , cos7 )) + float2( 0.5,0.5 );
			float2 coords4 = rotator7 * 20.0;
			float2 id4 = 0;
			float fade4 = 0.5;
			float voroi4 = 0;
			float rest4 = 0;
			for( int it4 = 0; it4 <2; it4++ ){
			voroi4 += fade4 * voronoi4( coords4, time4, id4,0 );
			rest4 += fade4;
			coords4 *= 2;
			fade4 *= 0.5;
			}//Voronoi4
			voroi4 /= rest4;
			float temp_output_22_0 = ( voroi4 * 2.9 );
			float4 temp_cast_1 = (temp_output_22_0).xxxx;
			float4 color10 = IsGammaSpace() ? float4(0.07008721,0.4245283,0.399029,0) : float4(0.005991078,0.1507122,0.1321888,0);
			float4 blendOpSrc16 = temp_cast_1;
			float4 blendOpDest16 = color10;
			float4 clampResult28 = clamp( (( blendOpSrc16 > 0.5 ) ? max( blendOpDest16, 2.0 * ( blendOpSrc16 - 0.5 ) ) : min( blendOpDest16, 2.0 * blendOpSrc16 ) ) , float4( 0.240299,0.3773585,0.3704401,0 ) , float4( 0,0.7999997,1,0 ) );
			o.Albedo = clampResult28.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV27 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode27 = ( -0.3 + 0.47 * pow( 1.0 - fresnelNdotV27, -0.37 ) );
			float3 temp_cast_3 = (fresnelNode27).xxx;
			o.Emission = temp_cast_3;
			o.Smoothness = temp_output_22_0;
			float clampResult32 = clamp( temp_output_22_0 , 0.07 , 0.43 );
			o.Alpha = clampResult32;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
3072;368.2;1920;1018;1414.515;420.3334;1.278248;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1716.423,151.8597;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;5;-1533.423,506.5815;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;7;-1181.423,442.8597;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CosTime;13;-1330.023,172.8597;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;4;-933.9793,211.5001;Inherit;False;1;0;1;3;2;False;1;False;False;4;0;FLOAT2;1,1;False;1;FLOAT;3.06;False;2;FLOAT;20;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT2;1
Node;AmplifyShaderEditor.RangedFloatNode;20;-869.2404,452.5283;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;2.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-860.4231,-227.1403;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;False;0.07008721,0.4245283,0.399029,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-581.356,425.2355;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;16;-451.6127,-241.1663;Inherit;False;PinLight;False;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;21;-598.066,194.8643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;28;-61.80701,-240.6845;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.240299,0.3773585,0.3704401,0;False;2;COLOR;0,0.7999997,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-1359.016,-133.0513;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;39;-1085.26,-46.7617;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-204.807,532.3156;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;False;1.79;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-426.4283,-0.9027975;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;2dcef4ebe50f9ba44a1f2d5ab5aa431f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;32;-217.016,284.1588;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.07;False;2;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;27;-113.7781,16.96455;Inherit;False;Standard;TangentNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.3;False;2;FLOAT;0.47;False;3;FLOAT;-0.37;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;271.8466,-50.60675;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AmplifyCollection/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;7;0;6;0
WireConnection;7;2;5;0
WireConnection;4;0;7;0
WireConnection;4;1;13;3
WireConnection;22;0;4;0
WireConnection;22;1;20;0
WireConnection;16;0;22;0
WireConnection;16;1;10;0
WireConnection;21;0;4;0
WireConnection;28;0;16;0
WireConnection;39;0;38;0
WireConnection;37;1;5;0
WireConnection;32;0;22;0
WireConnection;0;0;28;0
WireConnection;0;1;37;0
WireConnection;0;2;27;0
WireConnection;0;4;22;0
WireConnection;0;9;32;0
WireConnection;0;11;4;0
ASEEND*/
//CHKSM=8A1A4B0BD6B36E552F43FC85EDDE5AA2DCA54BF2