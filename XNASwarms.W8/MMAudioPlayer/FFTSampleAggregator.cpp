//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#include "pch.h"
#include "xdsp.h"
#include "FFTSampleAggregator.h"

double Hamming(int n, int frameSize)
{
	return 0.54 - 0.46 * cos((2 * M_PI * n) / (frameSize - 1));
}

double Hann(int n, int frameSize)
{
	return 0.5 * (1 - cos((2 * M_PI * n) / (frameSize - 1)));
}

double BlackmannHarris(int n, int frameSize)
{
	return 0.35875 - (0.48829 * cos((2 * M_PI * n) / (frameSize - 1))) + 
		(0.14128 * cos((4 * M_PI * n) / (frameSize - 1))) - 
		(0.01168 * cos((4 * M_PI * n) / (frameSize - 1)));
}


FFTSampleAggregator::FFTSampleAggregator(int channelsCount)
{
	InitializeCriticalSectionEx(&m_section,0,0);

	m_channelsCount=channelsCount;

	
	p_xdspUnityTable=(XDSP::XMVECTOR *)_aligned_malloc
		(fft_samples_count*sizeof(XDSP::XMVECTOR)*4,16);
	p_xdspReal=(XDSP::XMVECTOR *)_aligned_malloc
		(fft_samples_count*sizeof(XDSP::XMVECTOR)/4,16);
	p_xdspImag=(XDSP::XMVECTOR *)_aligned_malloc
		(fft_samples_count*sizeof(XDSP::XMVECTOR)/4,16);
	
	XDSP::FFTInitializeUnityTable(p_xdspUnityTable,fft_samples_count);

	samples.resize(fft_samples_count);
	window.resize(fft_samples_count);


	for (int i=0;i<fft_samples_count;i++)
		window[i]=(float)Hamming(i,fft_samples_count);

	currentPos=0;
}


FFTSampleAggregator::~FFTSampleAggregator(void)
{
	_aligned_free(p_xdspUnityTable);
	_aligned_free(p_xdspReal);
	_aligned_free(p_xdspImag);
	DeleteCriticalSection(&m_section);
}


void FFTSampleAggregator::FFT(float *result)
{
	float* pReal_temp=(float *)p_xdspReal;
	float* pImag_temp=(float *)p_xdspImag;

	EnterCriticalSection(&m_section);
	CopyMemory(pReal_temp,samples.data(),
		sizeof(float)*fft_samples_count);
	LeaveCriticalSection(&m_section);

	//apply window
	for (int x=0;x<fft_samples_count;x++)
		pReal_temp[x]=pReal_temp[x]*window[x];

		
	ZeroMemory(pImag_temp,sizeof(float)*fft_samples_count);

	
	XDSP::FFTInterleaved(p_xdspReal,p_xdspImag,p_xdspUnityTable,
		1, //always equal 1 cause we sum it in SampleAggregator::Add
		(int)(log(fft_samples_count)*M_LOG2E));
	
	//calculate power
	XDSP::FFTPolar((XDSP::XMVECTOR *)result,
		p_xdspReal,
		p_xdspImag,
		fft_samples_count/2);

}

void FFTSampleAggregator::Add(float *sample)
{
	float mid=0;

	switch (m_channelsCount)
	{
	case 2:
		mid=(sample[0]+sample[1])/2;
		break;
	case 1:
		mid=sample[0];
		break;
	default:
		for (int i=0;i<m_channelsCount;i++)
			mid=mid+sample[i];
		mid=mid/m_channelsCount;
		break;
	}

	//don't waste buffer then we do FFT calculation
	EnterCriticalSection(&m_section);
	samples[currentPos]=mid;
	LeaveCriticalSection(&m_section);

	currentPos++;

	if (currentPos>=fft_samples_count)
		currentPos=0;
}