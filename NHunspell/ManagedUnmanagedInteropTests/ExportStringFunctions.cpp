#include <windows.h>
#define DLLEXPORT extern "C" __declspec( dllexport )


DLLEXPORT void InteropVoid(void)
{

}


DLLEXPORT void InteropChar(char * parameter)
{

}

DLLEXPORT void InteropWChar(wchar_t * parameter)
{

}


DLLEXPORT void InteropWCharAndMultByteConvert(wchar_t * parameter)
{
	// This Code is buggy, it is for test purposes only and crashes on MBS > 127 Byte
	size_t buffersize = WideCharToMultiByte(CP_UTF8,WC_ERR_INVALID_CHARS,parameter,-1,0,0,0,0); 
	char buffer[128];
	WideCharToMultiByte(CP_UTF8,WC_ERR_INVALID_CHARS,parameter,-1,buffer,buffersize,0,0);

}

DLLEXPORT void InteropWCharAndMultByteConvertNewDelete(wchar_t * parameter)
{
	size_t buffersize = WideCharToMultiByte(CP_UTF8,WC_ERR_INVALID_CHARS,parameter,-1,0,0,0,0); 
	char * buffer = new char[buffersize];
	WideCharToMultiByte(CP_UTF8,WC_ERR_INVALID_CHARS,parameter,-1,buffer,buffersize,0,0);
	delete[] buffer;
}

DLLEXPORT char * InteropStaticCharReturn( )
{
	return "TEST TEST TEST";
}


DLLEXPORT wchar_t * InteropStaticWCharReturn( )
{
	return L"TEST TEST TEST";
}

DLLEXPORT wchar_t * InteropAlocatedWCharReturn( )
{
	wchar_t * str = L"TEST TEST TEST";
	size_t length = wcslen( str );
	wchar_t * returnvalue  = (wchar_t *) CoTaskMemAlloc( (length +1) * sizeof(wchar_t) );
	wcscpy(returnvalue,str);
	return returnvalue;
}

static wchar_t * buffer = 0;
DLLEXPORT wchar_t * InteropNewWCharReturn( )
{
	if( buffer != 0 )
		delete[] buffer;

	wchar_t * str = L"TEST TEST TEST";
	size_t length = wcslen( str );
	wchar_t * buffer  = new wchar_t[length +1];
	wcscpy(buffer,str);
	return buffer;
}

DLLEXPORT void InteropRefWCharReturn(wchar_t ** parameter)
{
	CoTaskMemFree( *parameter );
	wchar_t * str = L"TEST TEST TEST";
	size_t length = wcslen( str );
	*parameter  = (wchar_t *) CoTaskMemAlloc( (length +1) * sizeof(wchar_t) );
	wcscpy(*parameter,str);
}

