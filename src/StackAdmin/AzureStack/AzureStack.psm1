#  
# Module manifest for module 'AzureStack'  
#  
# Generated by: Microsoft Corporation  
#  
# Generated on: 10/30/2017 14:09:38
#  

$PSDefaultParameterValues.Clear()
Set-StrictMode -Version Latest

# Import dependencies using required version, if it is allowed
if ($PSVersionTable.PSVersion.Major -ge 5)
{
  Import-Module AzureRM.Profile -RequiredVersion 3.4.1
  Import-Module AzureRM.AzureStackAdmin -RequiredVersion 0.11.1
  Import-Module AzureRM.AzureStackStorage -RequiredVersion 0.10.9

}
else
{
  Import-Module AzureRM.Profile
  Import-Module AzureRM.AzureStackAdmin
  Import-Module AzureRM.AzureStackStorage

}
