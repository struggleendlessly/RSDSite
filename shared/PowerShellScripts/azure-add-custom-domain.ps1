param(
    [Parameter(Mandatory = $true)]
    [string]$WebAppName,
	
    [Parameter(Mandatory = $true)]
    [string]$WebAppResourceGroup,
	
	[Parameter(Mandatory = $true)]
    [string]$CustomDomain
)

if (-not $WebAppName -or -not $WebAppResourceGroup -or -not $CustomDomain) {
    Write-Host "Usage: .\azure-add-custom-domain.ps1 -WebAppName <web-app-name> -WebAppResourceGroup <web-app-resource-group> -CustomDomain <custom-domain>"
    exit 1
}

$Hostnames=(Get-AzWebApp -ResourceGroupName $WebAppResourceGroup -Name $WebAppName).Hostnames
$Hostnames += $CustomDomain

Set-AzWebApp -Name $WebAppName -ResourceGroupName $WebAppResourceGroup -HostNames @($Hostnames)

Write-Host "Custom domain successfully added!"