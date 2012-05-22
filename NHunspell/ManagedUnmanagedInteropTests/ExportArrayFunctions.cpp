#include <windows.h>
#define DLLEXPORT extern "C" __declspec( dllexport )



DLLEXPORT HRESULT InteropStringArray( const wchar_t ** &buffer )
{
	/*
	// Free the Array
	for( int i = 0; i < *pSize; ++i )
	{
		CoTaskMemFree(ppArray[i]);
	}

	CoTaskMemFree( ppArray );
	ppArray = 0;
		
	pSize = 0;
	
	// ppArray = (wchar_t **) CoTaskMemAlloc(sizeof( wchar_t **));
	// *ppArray = 0;
	*/
	/*	
	buffer = new wchar_t * [5];
	for( int i = 0; i < 5; ++ i )
	{
		buffer[i] = new wchar_t[20];
		wcscpy(buffer[i],L"0: TEST");
		buffer[i][0] = i + L'0';
	}
*/

	const wchar_t * staticBuffer[] = {L"0: TEST",L"1: TEST",L"2: TEST",L"3: TEST" };
	buffer = staticBuffer;


	return 0;
}