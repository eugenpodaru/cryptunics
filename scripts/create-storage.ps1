param(
  [Parameter(Mandatory=$true)]
  [string] $name,
  [string] $rgName,
  [string] $location = "westeurope",
  [string] $sku = "Standard_LRS"
  )

$name = $name.ToLower()

if([string]::IsNullOrEmpty($rgName)){
  $rgName = "$name-rg"
}

$storageName = $($name -replace '[-]','') + "st"

echo "Creating resource group $rgName in $location..."
az group create -n $rgName -l $location

echo "Creating storage account $storageName in resource group $rgName..."
az storage account create -n $storageName -l $location -g $rgName --sku $sku
