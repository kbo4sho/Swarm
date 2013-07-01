//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#pragma once

class FFTSampleAggregator
{
public:
	FFTSampleAggregator(int channelsCount);
	~FFTSampleAggregator(void);
	 void Add(float *sample);
	 void FFT(float *result);
	static const int fft_samples_count=512;
private:
	std::vector<float> samples;
	std::vector<float> window;
	
	XDSP::XMVECTOR	*p_xdspReal;
	XDSP::XMVECTOR	*p_xdspImag;
	XDSP::XMVECTOR	*p_xdspUnityTable;

	int m_channelsCount;
	DWORD currentPos;
	CRITICAL_SECTION  m_section;

};

