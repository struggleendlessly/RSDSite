param(
    [Parameter(Mandatory = $true)]
    [string]$UserEmail,

    [Parameter(Mandatory = $true)]
    [string]$UserPassword,

    [Parameter(Mandatory = $true)]
    [string]$SiteName,
	
	[Parameter(Mandatory = $true)]
    [string]$PublishDirectory
)

# Validate parameters
if (-not $UserEmail -or -not $UserPassword -or -not $SiteName) {
    Write-Host "Usage: .\create-website.ps1 -UserEmail <user-email> -UserPassword <user-password> -SiteName <site-name> -PublishDirectory <publish-directory>"
    exit 1
}

# Get the path of the directory where the script resides
$ScriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path

# Create a directory named after the site name
$SiteDirectory = Join-Path -Path $ScriptDirectory -ChildPath $SiteName

if (-not (Test-Path $SiteDirectory)) {
    New-Item -ItemType Directory -Path $SiteDirectory | Out-Null
}

# Copy the content of the publish directory to the site directory
Copy-Item -Path $PublishDirectory\* -Destination $SiteDirectory -Recurse -Force

# Path to the appsettings.json file within the site directory
$AppSettingsFile = Join-Path -Path $SiteDirectory -ChildPath "appsettings.json"

# Update the appsettings.json file with the provided parameters
$appSettingsContent = Get-Content $AppSettingsFile -Raw | ConvertFrom-Json
$appSettingsContent.SiteData.UserEmail = $UserEmail
$appSettingsContent.SiteData.UserPassword = $UserPassword
$appSettingsContent.SiteData.SiteName = $SiteName
$appSettingsContent | ConvertTo-Json | Set-Content $AppSettingsFile

# Create a new site
New-WebSite -Name $SiteName -Port 81 -PhysicalPath $PublishDirectory -ApplicationPool "DefaultAppPool" -Force

# Start the site
Start-WebSite -Name $SiteName

Write-Host "Deployment completed successfully."