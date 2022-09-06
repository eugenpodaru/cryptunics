param(
  [Parameter(Mandatory=$true)]
  [string] $name,
  [string] $rgName,
  [string] $planName,
  [string] $location = "westeurope",
  [string] $sku = "F1"
  )

$name = $name.ToLower()

if([string]::IsNullOrEmpty($rgName)){
  $rgName = "$name-rg"
}

if([string]::IsNullOrEmpty($planName)){
  $planName = "$name-plan"
}

$appName = "$name-app"

echo "Creating resource group $rgName in $location..."
az group create -n $rgName -l $location

echo "Creating app service plan $planName($sku) in resource group $rgName..."
az appservice plan create -n $planName -g $rgName -l $location --sku $sku --is-linux

echo "Creating web app $appName using plan $planName in resource group $rgName..."
az webapp create -n $appName -p $planName -g $rgName --runtime "DOTNETCORE:6.0"

echo "Assigning msi to web app $appName.."
az webapp identity assign -n $appName -g $rgName

echo "Setting 64bit and always-on to true if supported..."
if($sku -ne "D1" -and $sku -ne "F1")
{
  az webapp config set -n $appName -g $rgName --always-on true --use-32bit-worker-process false
}
else 
{
  az webapp config set -n $appName -g $rgName --always-on false --use-32bit-worker-process true
}