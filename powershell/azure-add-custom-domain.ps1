param (
    [Parameter(Mandatory)]
    [string]$WebAppName,

    [Parameter(Mandatory)]
    [string]$ContainerName
)

# Global variables
$WebAppResourceGroup = "rsdsite"
$StorageAccountName = "csb100320036e70384e"
$DateFormat = "MM-dd-yyyy"
$ArchivedContainerName = "archived"

# Domain verification statuses
$PendingVerificationStatus = "PendingVerification"
$VerifiedStatus = "Verified"
$VerificationFailedStatus = "VerificationFailed"

# Other statuses
$AzureOperationSuccessStatus = "Success"

# Context for accessing Azure Storage
$Context = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey "J1gUxVk+6VL5kw+q3Q6C0lpvrCdsPSD2UuRdz4CkGiUAu6JBSdbd2eCkAmJIfbPT/Jc4w5QFMd+B+AStwCRqvQ=="

# Function to get a specific blob by name from a given container
function Get-BlobByName {
    param (
        [Parameter(Mandatory)]
        [string]$containerName,
        
        [Parameter(Mandatory)]
        [string]$blobName
    )
    
    return Get-AzStorageBlob -Container $containerName -Context $Context | Where-Object { $_.Name -eq $blobName }
}

# Function to process a blob, verify custom domains, and update the status in the blob
function Add-CustomDomain {
    param (
        [Parameter(Mandatory)]
        [string]$date
    )

    $blobName = "$date.json"
    $blob = Get-BlobByName -ContainerName $ContainerName -BlobName $blobName

    if ($blob) {
        # Download the blob content to a local file
        $localFilePath = Join-Path -Path (Get-Location) -ChildPath $blobName
        Get-AzStorageBlobContent -Blob $blobName -Container $ContainerName -Context $Context -Destination $localFilePath
    
        # Read and parse the JSON content of the blob
        $blobContentString = Get-Content -Path $localFilePath -Raw
        $models = $blobContentString | ConvertFrom-Json
    
        foreach ($model in $models) {
            # Check if the domain status is PendingVerification or VerificationFailed
            if ($model.Status -eq $PendingVerificationStatus -or $model.Status -eq $VerificationFailedStatus) {
                $customDomain = $model.Domain
                $hostnames = (Get-AzWebApp -ResourceGroupName $WebAppResourceGroup -Name $WebAppName).Hostnames
                $hostnames += $customDomain
    
                # Attempt to add the custom domain to the Azure Web App
                $addCustomDomainResult = Set-AzWebApp -Name $WebAppName -ResourceGroupName $WebAppResourceGroup -HostNames @($hostnames)
    
                if ($addCustomDomainResult.HostNames -contains $customDomain) {
                    # Create and bind a new SSL certificate for the custom domain
                    $certificateName = "$customDomain-$WebAppName"
                    New-AzWebAppCertificate -ResourceGroupName $WebAppResourceGroup -WebAppName $WebAppName -Name $certificateName -HostName $customDomain -AddBinding
    
                    # Update the model status to Verified
                    $model.Status = $VerifiedStatus
                    $model.Error = $null
                } else {
                    # Update the model status to VerificationFailed
                    $model.Status = $VerificationFailedStatus
                    $model.Error = "Failed to add custom domain"
                }
            }
        }
    
        # Save the updated models back to the blob
        $models | ConvertTo-Json -Depth 10 | Set-Content -Path $localFilePath
        Set-AzStorageBlobContent -Container $ContainerName -Blob $blobName -Context $Context -File $localFilePath -Force

        # Remove the local temporary file
        Remove-Item -Path $localFilePath

        Write-Output "Blob '$blobName' processed and uploaded successfully."
    } else {
        Write-Output "Blob with name '$blobName' not found."
    }
}

# Function to move a blob from one container to another
function Move-BlobToArchive {
    param(
        [Parameter(Mandatory)]
        [string]$sourceContainer,
        
        [Parameter(Mandatory)]
        [string]$destinationContainer,
        
        [Parameter(Mandatory)]
        [string]$blobName
    )

    $blob = Get-BlobByName -ContainerName $sourceContainer -BlobName $blobName

    if ($blob) {
        # Start the blob copy operation
        Start-AzStorageBlobCopy -SrcBlob $blobName -SrcContainer $sourceContainer -DestBlob $blobName -DestContainer $destinationContainer -Context $Context -Force

        # Wait for the copy operation to complete
        while ((Get-AzStorageBlobCopyState -Blob $blobName -Container $destinationContainer -Context $Context).Status -ne $AzureOperationSuccessStatus) {
            Start-Sleep -Seconds 1
        }
            
        # Verify the copy operation was successful and delete the source blob
        $copyStatus = (Get-AzStorageBlobCopyState -Blob $blobName -Container $destinationContainer -Context $Context).Status
        if ($copyStatus -eq $AzureOperationSuccessStatus) {
            Remove-AzStorageBlob -Container $sourceContainer -Blob $blobName -Context $Context
            Write-Output "Blob '$blobName' moved from '$sourceContainer' to '$destinationContainer' successfully."
        } else {
            Write-Output "Failed to move blob '$blobName' from '$sourceContainer' to '$destinationContainer'."
        }
    } else {
        Write-Output "Failed to move blob '$blobName' to '$destinationContainer' because the blob was not found."
    }
}

# Process the blob from the previous day
$YesterdayDate = (Get-Date).AddDays(-1).ToString($DateFormat)
$YesterdayDateBlobName = "$YesterdayDate.json"

Add-CustomDomain -Date $YesterdayDate
Move-BlobToArchive -SourceContainer $ContainerName -DestinationContainer $ArchivedContainerName -BlobName $YesterdayDateBlobName

# Process today's blob
$TodayDate = Get-Date -Format $DateFormat
Add-CustomDomain -Date $TodayDate
