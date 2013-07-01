//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#include "pch.h"


#include "utils.h"
#include "player.h"
#include "SpectrumAnalyzerXAPO.h"

using namespace MMAudioPlayer;

namespace MMAudioPlayer
{
	struct StreamingVoiceCallback : public IXAudio2VoiceCallback
	{
	private:
		Player ^m_player;

	public:
		StreamingVoiceCallback(Player ^player){m_player=player;}

		STDMETHOD_( void, OnVoiceProcessingPassStart )( UINT32 bytesRequired )
		{
			//unblock render thread
			SetEvent(m_player->m_musicRenderThreadHandle);
		}


		STDMETHOD_( void, OnVoiceProcessingPassEnd )(){}
		STDMETHOD_( void, OnStreamEnd )(){}
		STDMETHOD_( void, OnBufferStart )( void* pContext ){}
		STDMETHOD_( void, OnBufferEnd )( void* pContext ){}
		STDMETHOD_( void, OnLoopEnd )( void* pContext ){}
		STDMETHOD_( void, OnVoiceError )( void* pContext, HRESULT error ){}
	private:
	};

}

void Player::Start()
{
	if (m_musicSourceVoice!=nullptr)
	{
		m_bPlaying=TRUE;
		ThrowIfFailed(
			m_musicSourceVoice->Start()
			);
	}
}

void Player::Stop()
{
	if (m_musicSourceVoice!=nullptr)
	{
		//stop submitting new buffers from render thread
		m_bPlaying=FALSE;

		ThrowIfFailed(
			m_musicSourceVoice->Stop()
			);
	}
}

Player::Player()
{
	m_musicMasteringVoice=nullptr;
	m_musicSourceVoice=nullptr;
	m_mediaRdr=nullptr;
	callback=nullptr;
	pSpectrumAnalyzerXAPO=nullptr;
	currentPositionDelta=0;
	playPosCorrelation=0;

	m_bPlaying=FALSE;

	ZeroMemory(&Wav,sizeof(Wav));

	ThrowIfFailed(
		MFStartup(MF_VERSION)
		);

	ThrowIfFailed(
		XAudio2Create(&m_musicEngine, 0)
		);
#ifdef _DEBUG
	XAUDIO2_DEBUG_CONFIGURATION debugConfiguration = { 0 };
	debugConfiguration.TraceMask = XAUDIO2_LOG_WARNINGS;
	debugConfiguration.BreakMask = XAUDIO2_LOG_ERRORS;
	m_musicEngine->SetDebugConfiguration( &debugConfiguration );
#endif

	ThrowIfFailed(m_musicEngine->CreateMasteringVoice(
		&m_musicMasteringVoice,
		XAUDIO2_DEFAULT_CHANNELS,
		44100, //usually
		0,
		nullptr,  
		nullptr,  
		AudioCategory_BackgroundCapableMedia
		));


	m_RenderThreadExitHandle=CreateEventEx(nullptr, nullptr, CREATE_EVENT_MANUAL_RESET, WRITE_OWNER | EVENT_ALL_ACCESS);
	ResetEvent(m_RenderThreadExitHandle);
}


Player::~Player(void)
{
	Cleanup();

	CloseHandle(m_RenderThreadExitHandle);

	if (m_musicMasteringVoice!=nullptr)
	{
		m_musicMasteringVoice->DestroyVoice();
		m_musicMasteringVoice=nullptr;
	}


	if (m_musicEngine!=nullptr)
	{
		assert(m_musicEngine->Release()==0);
		m_musicEngine=nullptr;
	}

	ThrowIfFailed(
		MFShutdown()
		);
}


void Player::Cleanup()
{

	currentPositionDelta=0;
	playPosCorrelation=0;

	if (m_musicSourceVoice!=nullptr)
	{
		m_musicSourceVoice->DestroyVoice();


		//set exit flag from render thread
		m_bExitFromRenderThreadFlag=TRUE;
		SetEvent(m_musicRenderThreadHandle);

		//wait for render thread exit
		WaitForSingleObjectEx(m_RenderThreadExitHandle,INFINITE,TRUE);
		ResetEvent(m_RenderThreadExitHandle);

		m_musicSourceVoice=nullptr;
		llTimeStamp=0;

		CloseHandle(m_musicRenderThreadHandle);
	}

	if (m_mediaRdr!=nullptr)
	{
		assert(m_mediaRdr->Release()==0);
		m_mediaRdr=nullptr;
	}

	if (callback!=nullptr)
	{
		delete callback;
		callback=nullptr;
	}

	if (pSpectrumAnalyzerXAPO!=nullptr)
	{
		assert(pSpectrumAnalyzerXAPO->Release()==0);
		pSpectrumAnalyzerXAPO=nullptr;
	}
}

void Player::SetAudioData( IRandomAccessStream ^ stream )
{

	ComPtr<IMFByteStream> spMFByteStream;
	ComPtr<IMFMediaType> partial;
	ComPtr<IMFMediaType> actualType;

	Cleanup();



	m_musicRenderThreadHandle=CreateEventEx(nullptr, nullptr, 
		CREATE_EVENT_MANUAL_RESET, WRITE_OWNER | EVENT_ALL_ACCESS);
	m_bExitFromRenderThreadFlag=FALSE;
	ResetEvent(m_musicRenderThreadHandle);

	ThrowIfFailed(
		MFCreateMFByteStreamOnStreamEx((IUnknown*)stream, &spMFByteStream)
		);

	ThrowIfFailed(
		MFCreateSourceReaderFromByteStream(
		spMFByteStream.Get(), nullptr, &m_mediaRdr)
		);


	assert(m_mediaRdr!=nullptr);
	assert(m_musicSourceVoice==nullptr);
	assert(callback==nullptr);
	assert(pSpectrumAnalyzerXAPO==nullptr);

	ThrowIfFailed(
		m_mediaRdr->SetStreamSelection(
		MF_SOURCE_READER_ALL_STREAMS,FALSE)
		);

	ThrowIfFailed(
		m_mediaRdr->SetStreamSelection(
		MF_SOURCE_READER_FIRST_AUDIO_STREAM,TRUE)
		);


	ThrowIfFailed(
		MFCreateMediaType(&partial)
		);

	ThrowIfFailed(
		partial->SetGUID(MF_MT_MAJOR_TYPE,MFMediaType_Audio)
		);

	ThrowIfFailed(
		partial->SetGUID(MF_MT_SUBTYPE, MFAudioFormat_Float)
		);


	ThrowIfFailed(
		m_mediaRdr->SetCurrentMediaType(
		MF_SOURCE_READER_FIRST_AUDIO_STREAM,NULL,partial.Get())
		);


	ThrowIfFailed(
		m_mediaRdr->GetCurrentMediaType(
		MF_SOURCE_READER_FIRST_AUDIO_STREAM,&actualType)
		);



	WAVEFORMATEX *pWav = NULL;
	UINT32 cbFormat = 0;

	ThrowIfFailed(
		MFCreateWaveFormatExFromMFMediaType(
		actualType.Get(), &pWav, &cbFormat)
		);


	callback=new StreamingVoiceCallback (this);



	ThrowIfFailed(
		m_musicEngine->CreateSourceVoice(
		&m_musicSourceVoice, pWav, 0, 1.0f, callback, nullptr)
		);


	pSpectrumAnalyzerXAPO = new SpectrumAnalyzerXAPO();

	XAUDIO2_EFFECT_DESCRIPTOR descriptor;
	descriptor.InitialState = true;
	descriptor.OutputChannels = pWav->nChannels;
	descriptor.pEffect = pSpectrumAnalyzerXAPO;


	XAUDIO2_EFFECT_CHAIN chain;
	chain.EffectCount = 1;
	chain.pEffectDescriptors = &descriptor;


	ThrowIfFailed(
		m_musicSourceVoice->SetEffectChain(&chain)
		);


	CopyMemory(&Wav,pWav,sizeof(Wav));
	CoTaskMemFree(pWav);

	concurrency::create_async([this]()
	{
		this->MusicRederThread();
	});
}

Platform::Array<float> ^Player::GetFFT()
{
	if (pSpectrumAnalyzerXAPO!=nullptr)
		return pSpectrumAnalyzerXAPO->GetFFT();
	else
		return ref new Platform::Array<float>(0);
}

double Player::GetDuration()
{
	if (m_mediaRdr!=nullptr)
	{
		PROPVARIANT var;

		//it's just attribute, not accurate!
		ThrowIfFailed(
			m_mediaRdr->GetPresentationAttribute(
			MF_SOURCE_READER_MEDIASOURCE, MF_PD_DURATION, &var)
			);
		LONGLONG duration = var.uhVal.QuadPart;
		float64 durationInSeconds = (duration / (float64)(10000 * 1000));

		return durationInSeconds;
	}
	else
	{
		return 0;
	}
}


void Player::SetCurrentPosition(double seconds)
{
	if (m_mediaRdr!=nullptr)
	{
		BOOL bPlaying=m_bPlaying;
		if (bPlaying)
		{
			Stop();

		}

		ThrowIfFailed(
			m_musicSourceVoice->FlushSourceBuffers()
			);

		XAUDIO2_VOICE_STATE state;
		m_musicSourceVoice->GetState(&state,0);



		PROPVARIANT var;
		ZeroMemory(&var, sizeof(var));
		var.vt = VT_I8;

		LONGLONG pos=(LONGLONG)(seconds*((float64)10000 * 1000));
		var.uhVal.QuadPart=pos;

		playPosCorrelation=state.SamplesPlayed;


		ThrowIfFailed(
			m_mediaRdr->SetCurrentPosition(GUID_NULL,var)
			);
		currentPositionDelta=seconds;

		if (bPlaying)
			Start();
	}
}


double Player::GetCurrentPosition()
{
	XAUDIO2_VOICE_STATE state;

	this->m_musicSourceVoice->GetState(&state,0);


	double currentPos=currentPos=(double)((double)state.SamplesPlayed-playPosCorrelation)/(double)Wav.nSamplesPerSec;

	return currentPos+currentPositionDelta;
}

float Player::GetVolume()
{
	float retval=0;
	if (m_musicSourceVoice!=nullptr)
	{
		m_musicSourceVoice->GetVolume(&retval);
	}

	return retval;
}

void Player::SetVolume(float vol)
{
	if (m_musicSourceVoice!=nullptr)
	{
		ThrowIfFailed(
			m_musicSourceVoice->SetVolume(vol)
			);
	}
}


void Player::MusicRederThread()
{

	DWORD dwActualStreamIndex=0;
	DWORD dwStreamFlags=0;
	int iSwap=0;

	BOOL endofStreamOrError=FALSE;



	static const int BUFFER_COUNT=3;
	std::vector<BYTE> play_buffer[BUFFER_COUNT];

	while (true)
	{
		//wait for voice callback::OnVoiceProcessingPassStart 
		WaitForSingleObjectEx(m_musicRenderThreadHandle,INFINITE,TRUE);

		//another place where renderThreadHandle sets is Cleanup()
		//just check this flag and exit immideately
		if (m_bExitFromRenderThreadFlag==TRUE)
			break;

		if (m_bPlaying==FALSE)
		{
			ResetEvent(m_musicRenderThreadHandle);
			continue;
		}


		XAUDIO2_VOICE_STATE state;
		m_musicSourceVoice->GetState(&state);


		//queue is full, skip and wait again
		if (state.BuffersQueued>=BUFFER_COUNT)
		{
			ResetEvent(m_musicRenderThreadHandle);
			continue;
		}


		ComPtr<IMFSample> sample;

		LONGLONG tick=0;

		ThrowIfFailed(
			m_mediaRdr->ReadSample(
			MF_SOURCE_READER_FIRST_AUDIO_STREAM,0,
			&dwActualStreamIndex,&dwStreamFlags,&tick,&sample)
			);


		if (dwStreamFlags & MF_SOURCE_READERF_ENDOFSTREAM || 
			dwStreamFlags & MF_SOURCE_READERF_CURRENTMEDIATYPECHANGED ||
			dwStreamFlags & MF_SOURCE_READERF_ERROR)
		{


			if (state.BuffersQueued==0)
			{

				m_bPlaying=FALSE;

				ThrowIfFailed(
					m_musicSourceVoice->Discontinuity()
					);


				ThrowIfFailed(
					m_musicSourceVoice->Stop()
					);


				currentPositionDelta=0;
				playPosCorrelation=0;


				EndOfStreamReason rsn;
				if (dwStreamFlags == MF_SOURCE_READERF_ENDOFSTREAM)
					rsn=EndOfStreamReason::ok;
				else
					rsn=EndOfStreamReason::not_ok;

				SetCurrentPosition(0);

				concurrency::create_async([this,rsn]()
				{
					RaiseEndOfStream(rsn);
				});
			}

			//just continue and wait for cleanup() exit 
			// or seek to pos and play again
			ResetEvent(m_musicRenderThreadHandle);
			continue;;
		}

		//ppSample is null, do nothing
		if (dwStreamFlags & MF_SOURCE_READERF_STREAMTICK )
		{
			ResetEvent(m_musicRenderThreadHandle);
			continue;
		}

		llTimeStamp=tick;

		ComPtr<IMFMediaBuffer> buffer;

		ThrowIfFailed(
			sample->ConvertToContiguousBuffer(&buffer)
			);

		BYTE *pAudioData=nullptr;

		DWORD cbCurrentLength=0;

		ThrowIfFailed(
			buffer->Lock(&pAudioData,nullptr,&cbCurrentLength)
			);



		if (play_buffer[iSwap].size()<cbCurrentLength)
			play_buffer[iSwap].resize(cbCurrentLength);

		CopyMemory(play_buffer[iSwap].data(),
			pAudioData,cbCurrentLength);


		ThrowIfFailed(
			buffer->Unlock()
			);

		XAUDIO2_BUFFER abuf = {0};
		abuf.AudioBytes = cbCurrentLength;
		abuf.pAudioData = play_buffer[iSwap].data();


		ThrowIfFailed(
			m_musicSourceVoice->SubmitSourceBuffer(&abuf)
			);

		iSwap = ++iSwap % BUFFER_COUNT;

		ResetEvent(m_musicRenderThreadHandle);
	}

	SetEvent(m_RenderThreadExitHandle);
}