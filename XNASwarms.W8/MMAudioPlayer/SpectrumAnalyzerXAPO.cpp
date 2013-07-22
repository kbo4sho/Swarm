//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#include "pch.h"
#include "SpectrumAnalyzerXAPO.h"
#include "xdsp.h"
#include "FFTSampleAggregator.h"

XAPO_REGISTRATION_PROPERTIES reg=
{
	__uuidof(SpectrumAnalyzerXAPO),
	L"SpectrumAnalyzerXAPO",
	L"Copyright (C)2013 Microsoft Corporation",
	1,
	0,
	XAPO_FLAG_INPLACE_REQUIRED
	| XAPO_FLAG_CHANNELS_MUST_MATCH
	| XAPO_FLAG_FRAMERATE_MUST_MATCH
	| XAPO_FLAG_BITSPERSAMPLE_MUST_MATCH
	| XAPO_FLAG_BUFFERCOUNT_MUST_MATCH
	| XAPO_FLAG_INPLACE_SUPPORTED,
	1, 1, 1, 1
};
SpectrumAnalyzerXAPO::SpectrumAnalyzerXAPO():CXAPOBase(&reg)
{
	sampleAggregator=nullptr;
	uChannels=0;
	uBytesPerSample=0;
	nSamplesPerSec=0;

	cnt=0;
}



SpectrumAnalyzerXAPO::~SpectrumAnalyzerXAPO(void)
{
	if (sampleAggregator!=nullptr)
		delete sampleAggregator;
}


HRESULT SpectrumAnalyzerXAPO::LockForProcess (
	UINT32 InputLockedParameterCount,
	const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS  *pInputLockedParameters,
	UINT32 OutputLockedParameterCount,
	const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS  *pOutputLockedParameters
	)
{
	assert(!IsLocked());
	assert(InputLockedParameterCount == 1);
	assert(OutputLockedParameterCount == 1);
	assert(pInputLockedParameters != NULL);
	assert(pOutputLockedParameters != NULL);
	assert(pInputLockedParameters[0].pFormat != NULL);
	assert(pOutputLockedParameters[0].pFormat != NULL);


	uChannels = pInputLockedParameters[0].pFormat->nChannels;
	nSamplesPerSec=pInputLockedParameters[0].pFormat->nSamplesPerSec;
	uBytesPerSample = 
		(pInputLockedParameters[0].pFormat->wBitsPerSample >> 3);
	
	if (sampleAggregator!=nullptr)
	{
		delete sampleAggregator;
	}
	sampleAggregator=new FFTSampleAggregator(uChannels);


	return CXAPOBase::LockForProcess(
		InputLockedParameterCount,
		pInputLockedParameters,
		OutputLockedParameterCount,
		pOutputLockedParameters);
}

void SpectrumAnalyzerXAPO::Process(
	UINT32 InputProcessParameterCount,
	const XAPO_PROCESS_BUFFER_PARAMETERS *pInputProcessParameters,
	UINT32 OutputProcessParameterCount,
	XAPO_PROCESS_BUFFER_PARAMETERS *pOutputProcessParameters,
	BOOL IsEnabled
	)
{
		assert(IsLocked());
		assert(InputProcessParameterCount == 1);
		assert(OutputProcessParameterCount == 1);
		assert(NULL != pInputProcessParameters);
		assert(NULL != pOutputProcessParameters);


		XAPO_BUFFER_FLAGS inFlags = pInputProcessParameters[0].BufferFlags;
		XAPO_BUFFER_FLAGS outFlags = pOutputProcessParameters[0].BufferFlags;

		assert(inFlags == XAPO_BUFFER_VALID || 
			inFlags == XAPO_BUFFER_SILENT);
		assert(outFlags == XAPO_BUFFER_VALID || 
			outFlags == XAPO_BUFFER_SILENT);


		void* pvSrc = pInputProcessParameters[0].pBuffer;
		assert(pvSrc != NULL);

		void* pvDst = pOutputProcessParameters[0].pBuffer;
		assert(pvDst != NULL);


		switch (inFlags)
		{
		case XAPO_BUFFER_VALID:
			{
				//this XAPO doest really modify input signal
				// so either copy of source to dest doesn't require
				//memcpy(pvDst,pvSrc,pInputProcessParameters[0].ValidFrameCount * 
				// uChannels * uBytesPerSample);
				//
				// fun synth, example above
				// WARNING: VERY SLOW CODE
				/*
				for (int m=0;m<pInputProcessParameters[0].ValidFrameCount;m++)
				{
					for (int y=0;y<uChannels;y++)
					{
						float freq=440;
						double m_sampleTick=(DirectX::XM_PI*2.0f)/nSamplesPerSec;

						float *sample_ptr=(float *)pvDst+(m*uChannels);
						sample_ptr[y]=sin( (freq*m_sampleTick)*cnt);
					}
					cnt++;
				}*/
		
				for (unsigned int i=0;i<pInputProcessParameters[0].ValidFrameCount;i++)
				{
					sampleAggregator->Add((float *)pvSrc+i*uChannels);
				}
			}

		case XAPO_BUFFER_SILENT:
			{
				// All that needs to be done for this case is setting the
				// output buffer flag to XAPO_BUFFER_SILENT which is done below.
				break;
			}
		}

		pOutputProcessParameters[0].ValidFrameCount = 
			pInputProcessParameters[0].ValidFrameCount; 
		pOutputProcessParameters[0].BufferFlags     = 
			pInputProcessParameters[0].BufferFlags;
}


Platform::Array<float> ^SpectrumAnalyzerXAPO::GetFFT()
{
	int spectrumsamples=FFTSampleAggregator::fft_samples_count/2;
	float result[4096];
	assert(sizeof(result)>spectrumsamples);

	this->sampleAggregator->FFT(result);
	Platform::Array<float> ^arr=ref new Platform::Array<float>
		(spectrumsamples);
	CopyMemory(arr->Data,result,spectrumsamples*sizeof(float));
	
	return arr;
}