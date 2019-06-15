Shader "ErbGameArt/LWRP/Particles/Blend_LitGlow"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 2
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		[Toggle(_USEDEPTH_ON)] _Usedepth("Use depth?", Float) = 0
		_Depthpower("Depth power", Float) = 1
		[Toggle(_USECENTERGLOW_ON)] _Usecenterglow("Use center glow?", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_LightFalloff("Light Falloff", Float) = 0.5
		_LightRange("Light Range", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="LightweightPipeline" "PreviewType"="Plane"}
		Cull Off
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
		Pass
		{
			Tags { "LightMode"="LightweightForward" }
			Name "Base"
			
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest Off
			ColorMask RGBA
			
		    HLSLPROGRAM
			#pragma multi_compile lines
		    #pragma prefer_hlslcc gles
		    //#pragma exclude_renderers d3d11_9x
		    #pragma vertex vert
		    #pragma fragment frag
		
			#pragma shader_feature _USECENTERGLOW_ON
			#pragma shader_feature _USEDEPTH_ON
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/functions.hlsl"
			
			CBUFFER_START(UnityPerMaterial)
			float _LightRange;
			float _LightFalloff;
			sampler2D _MainTex;
			float4 _SpeedMainTexUVNoiseZW;
			float4 _MainTex_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float4 _Color;
			sampler2D _Mask;
			float4 _Mask_ST;
			float _Emission;
			uniform sampler2D _CameraDepthTexture;
			float _Depthpower;
			float _Opacity;
			CBUFFER_END
								
			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float4 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
	
		    struct GraphVertexOutput
		    {
		        float4 position : POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
		        UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
		    };
		
		    GraphVertexOutput vert (GraphVertexInput v )
			{
		        GraphVertexOutput o = (GraphVertexOutput)0;
		        UNITY_SETUP_INSTANCE_ID(v);
		        UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord1.xyz = v.ase_texcoord.xyz;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				v.vertex.xyz +=  float3( 0, 0, 0 ) ;
				v.ase_normal =  v.ase_normal ;
		        o.position = TransformObjectToHClip(v.vertex.xyz);
		        return o;
			}
		
		    half4 frag( GraphVertexOutput IN  ) : SV_Target
		    {
		        UNITY_SETUP_INSTANCE_ID(IN);
				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float4 appendResult70 = (float4(ase_worldPos , ase_worldPos.z));
				float2 appendResult21 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
				float2 uv_MainTex = IN.ase_texcoord1.xyz * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode13 = tex2D( _MainTex, ( ( appendResult21 * _Time.y ) + uv_MainTex ) );
				float2 uv_Noise = IN.ase_texcoord1.xyz.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float4 tex2DNode14 = tex2D( _Noise, ( uv_Noise + ( _Time.y * appendResult22 ) ) );
				float3 temp_output_30_0 = ( (tex2DNode13).rgb * (tex2DNode14).rgb * (_Color).rgb * (IN.ase_color).rgb );
				float2 uv_Mask = IN.ase_texcoord1.xyz.xy * _Mask_ST.xy + _Mask_ST.zw;
				float3 temp_output_58_0 = (tex2D( _Mask, uv_Mask )).rgb;
				float3 uv29 = IN.ase_texcoord1.xyz;
				uv29.xy = IN.ase_texcoord1.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float3 temp_cast_1 = ((1.0 + (uv29.z - 0.0) * (0.0 - 1.0) / (1.0 - 0.0))).xxx;
				float3 clampResult38 = clamp( ( temp_output_58_0 - temp_cast_1 ) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 clampResult40 = clamp( ( temp_output_58_0 * clampResult38 ) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				#ifdef _USECENTERGLOW_ON
				float3 staticSwitch46 = ( temp_output_30_0 * clampResult40 );
				#else
				float3 staticSwitch46 = temp_output_30_0;
				#endif
				
				float temp_output_60_0 = ( tex2DNode13.a * tex2DNode14.a * _Color.a * IN.ase_color.a );
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_60_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_60_0;
				#endif
				
		        float3 Color = ( ( _MainLightColor * (0.0 + (max( ( _LightRange - pow( distance( float4( _MainLightPosition.xyz , 0.0 ) , appendResult70 ) , _LightFalloff ) ) , 0.0 ) - 0.0) * (1.0 - 0.0) / (_LightRange - 0.0)) * 2.0 ) + float4( ( staticSwitch46 * _Emission ) , 0.0 ) ).rgb;
		        float Alpha = ( staticSwitch47 * _Opacity );
		        float AlphaClipThreshold = 0;
		#if _AlphaClip
		        clip(Alpha - AlphaClipThreshold);
		#endif
		    	return half4(Color, Alpha);
		    }
		    ENDHLSL
		}
		
		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite Off
			ColorMask 0
			
			HLSLPROGRAM
			#pragma multi_compile lines
			#pragma prefer_hlslcc gles  
			#pragma multi_compile_instancing
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
			#pragma shader_feature _USEDEPTH_ON


			CBUFFER_START(UnityPerMaterial)
			sampler2D _MainTex;
			float4 _SpeedMainTexUVNoiseZW;
			float4 _MainTex_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float4 _Color;
			uniform sampler2D _CameraDepthTexture;
			float _Depthpower;
			float _Opacity;
			CBUFFER_END			
			
			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float4 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct GraphVertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			GraphVertexOutput vert (GraphVertexInput v)
			{
				GraphVertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				v.vertex.xyz +=  float3(0,0,0) ;
				v.ase_normal =  v.ase_normal ;
				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag (GraphVertexOutput IN ) : SV_Target
		    {
		    	UNITY_SETUP_INSTANCE_ID(IN);

				float2 appendResult21 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode13 = tex2D( _MainTex, ( ( appendResult21 * _Time.y ) + uv_MainTex ) );
				float2 uv_Noise = IN.ase_texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float4 tex2DNode14 = tex2D( _Noise, ( uv_Noise + ( _Time.y * appendResult22 ) ) );
				float temp_output_60_0 = ( tex2DNode13.a * tex2DNode14.a * _Color.a * IN.ase_color.a );
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_60_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_60_0;
				#endif
				

				float Alpha = ( staticSwitch47 * _Opacity );
				float AlphaClipThreshold = AlphaClipThreshold;
				
				#if _AlphaClip
					clip(Alpha - AlphaClipThreshold);
				#endif
				return Alpha;
				return 0;
		    }
			ENDHLSL
		}
	}	
	FallBack "Hidden/InternalErrorShader"	
}