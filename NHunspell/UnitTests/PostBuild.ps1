# Post Build Script
Write-Host "Args:" $Args.Length
foreach( $Arg in $Args ) { Write-Host "Arg: $Arg" }
if( $Args.Length -ne 4 ) { Throw "Wrong Number of Arguments: $Args.Length"  }

# Setting Parameters from Arguments
$ConfigurationName = $Args[0]
$PlatformName = $Args[1]
$ProjectDir = $Args[2]
$TargetDir = $Args[3]

Copy-Item $ProjectDir\Hunspellx86.dll  $TargetDir
Copy-Item $ProjectDir\Hunspellx64.dll  $TargetDir
Copy-Item $ProjectDir\ManagedUnmanagedInteropTestsx32.dll $TargetDir
Copy-Item $ProjectDir\ManagedUnmanagedInteropTestsx64.dll  $TargetDir