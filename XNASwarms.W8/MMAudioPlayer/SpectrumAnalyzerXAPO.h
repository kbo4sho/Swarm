//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
#pragma once



class FFTSampleAggregator;
class SpectrumAnalyzerXAPO: public CXAPOBase
{
public:
	SpectrumAnalyzerXAPO();
	~SpectrumAnalyzerXAPO(void);

	Platform::Array<float> ^GetFFT();
	STDMETHOD(LockForProcess) (
		UINT32 InputLockedParameterCount,
		const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS  *pInputLockedParameters,
		UINT32 OutputLockedParameterCount,
		const XAPO_LOCKFORPROCESS_BUFFER_PARAMETERS  *pOutputLockedParameters
		);

	STDMETHOD_(void, Process)(
		UINT32 InputProcessParameterCount,
		const XAPO_PROCESS_BUFFER_PARAMETERS *pInputProcessParameters,
		UINT32 OutputProcessParameterCount,
		XAPO_PROCESS_BUFFER_PARAMETERS *pOutputProcessParameters,
		BOOL IsEnabled
		);
private:
	int uChannels;
	int uBytesPerSample;
	int nSamplesPerSec;

	LONGLONG cnt;

	FFTSampleAggregator *sampleAggregator;
};

class __declspec(uuid("0912CFDB-23D9-4FC6-9655-D6072D7261B9")) 
	SpectrumAnalyzerXAPO;
