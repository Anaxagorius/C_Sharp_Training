# PowerShell script to open Visual Studio, load solution, and open Solution Explorer & Properties
$vsPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe"
$solution = "C:\Users\W0516036\NSCC\Placeholdername\C sharp\SimpleGuiExample.sln"  # Adjust if solution name differs

if (-Not (Test-Path $vsPath)) {
    Write-Host "Visual Studio executable not found at $vsPath" -ForegroundColor Red
    exit 1
}

if (-Not (Test-Path $solution)) {
    Write-Host "Solution file not found at $solution" -ForegroundColor Red
    exit 1
}

Write-Host "Launching Visual Studio with Solution Explorer and Properties window..." -ForegroundColor Green

Start-Process -FilePath $vsPath -ArgumentList ""$solution" /Command View.SolutionExplorer" -Wait
Start-Process -FilePath $vsPath -ArgumentList ""$solution" /Command View.PropertiesWindow"
