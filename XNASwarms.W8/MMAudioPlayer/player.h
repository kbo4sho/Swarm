//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#pragma once


using namespace Microsoft::WRL;
using namespace Windows::Foundation;
using namespace Windows::Storage;
using namespace Windows::Storage::Streams;


class SpectrumAnalyzerXAPO;
namespace MMAudioPlayer
{
	struct StreamingVoiceCallback;

	public enum class EndOfStreamReason 
	{ 
		ok=1,
		not_ok=2
	};


	public delegate void OnEndOfStreamDelegate(EndOfStreamReason reason);


	public ref class Player sealed
	{
		friend struct StreamingVoiceCallback;

	public:
		Player();
		virtual ~Player(void);
		void Start();
		void Stop();
		void SetAudioData( IRandomAccessStream ^ stream );

		property double Duration
		{
			double get()
			{
				return GetDuration();
			}
		}


		property double CurrentPosition
		{
			double get()
			{
				return GetCurrentPosition();
			}
			void set(double seconds)
			{
				SetCurrentPosition(seconds);
			}
		}


		property float Vol
		{
			float get()
			{
				return GetVolume();
			}
			void set(float val)
			{
				SetVolume(val);
			}

		}


		Platform::Array<float> ^GetFFT();

		event OnEndOfStreamDelegate^ OnEndOfStream
		{
			Windows::Foundation::EventRegistrationToken add(
				OnEndOfStreamDelegate ^dlg)
			{
				auto subscr= _InternalHandler += dlg;

				InterlockedIncrement(&SubScribersCount);
				return subscr;
			}

			void remove(Windows::Foundation::EventRegistrationToken token)
			{
				InterlockedDecrement(&SubScribersCount);
				_InternalHandler -= token;

			}

			void raise(EndOfStreamReason reason)
			{
				return _InternalHandler(reason);
			}

		}

		property bool IsPlaying
		{
			bool get()
			{
				return m_bPlaying;
			}
		}

	private:

		float GetVolume();
		void SetVolume(float vol);
		void SetCurrentPosition(double seconds);
		double GetCurrentPosition();
		double GetDuration();

		event OnEndOfStreamDelegate^ _InternalHandler;


		void RaiseEndOfStream(EndOfStreamReason reason)
		{
			if (SubScribersCount>0)
				OnEndOfStream(reason);
		}
		void MusicRederThread();

		void Cleanup();
		void EndOfStream();

		IMFSourceReader	*m_mediaRdr;
		IXAudio2 *m_musicEngine;
		IXAudio2MasteringVoice *m_musicMasteringVoice;
		IXAudio2SourceVoice * m_musicSourceVoice;
		StreamingVoiceCallback *callback;

		WAVEFORMATEX Wav;
		SpectrumAnalyzerXAPO *pSpectrumAnalyzerXAPO;
		LONGLONG llTimeStamp;
		LONGLONG playPosCorrelation;
		LONG SubScribersCount;

		double currentPositionDelta;

		HANDLE m_musicRenderThreadHandle;
		HANDLE m_RenderThreadExitHandle;

		BOOL m_bExitFromRenderThreadFlag;

		BOOL m_bPlaying;

	};
}