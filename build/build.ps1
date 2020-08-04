
param(
  # get, don't execute
  [Parameter(Mandatory=$false)]
  [Alias("g")]
  [switch] $get = $false,

  # execute a command
  [Parameter(Mandatory=$false, ValueFromRemainingArguments=$true)]
  [String[]]
  $command
)



# ################################################################
# VARIABLES
# ################################################################

$rootPath = Join-Path $PSScriptRoot "/.."
$packagesPath = Join-Path $rootPath "/build/packages"
$zeroCore = "zero.Core"
$zeroWeb = "zero.Web"
$zeroWebUI = "zero.Web.UI"
$zeroCommerce = "zero.Commerce"


# ################################################################
# FUNCTIONS
# ################################################################

function Set-Project([String] $project)
{
  cd ..
  cd $project
}

function Go-Root()
{
  cd ..
  cd "build"
}

function New-Line()
{
  Write-Host ""
}



# ################################################################
# WARMUP
# ################################################################

# output welcome message
Write-Host "zero v0.1.0" -ForegroundColor DarkCyan
New-Line

# cleanup directory
if (Test-Path -Path $packagesPath -PathType Container)
{
  Write-Host "Cleanup packages directory..." -NoNewline
  Remove-Item -LiteralPath $packagesPath -Force -Recurse
  New-Item -Path $packagesPath -ItemType Directory
  Write-Host "Done" -ForegroundColor Cyan
}



# ################################################################
# BUILD + PACK
# ################################################################

# pack zero core
Write-Host "zero.Core: building and packing..."
New-Line
Set-Project $zeroCore
dotnet pack -o $packagesPath
Go-Root
Write-Host "Done" -ForegroundColor Cyan
New-Line

# pack zero web
Write-Host "zero.Web: building and packing..."
New-Line
Set-Project $zeroWeb
dotnet pack -o $packagesPath
Go-Root
Write-Host "Done" -ForegroundColor Cyan
New-Line

# pack zero web
Write-Host "zero.XTest: building and packing..."
New-Line
Set-Project "zero.XTest"
dotnet pack -o $packagesPath
Go-Root
Write-Host "Done" -ForegroundColor Cyan
New-Line

# pack zero commerce
Write-Host "zero.Commerce: building and packing..."
New-Line
Set-Project $zeroCommerce
dotnet pack -o $packagesPath
Go-Root
Write-Host "Done" -ForegroundColor Cyan
New-Line
