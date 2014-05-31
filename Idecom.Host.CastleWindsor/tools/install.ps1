param($installPath, $toolsPath, $package, $project)

if(!$toolsPath){
	$project = Get-Project
}

function FindHostContainerConfigurationClass($elem) {
    if ($elem.IsCodeType -and ($elem.Kind -eq [EnvDTE.vsCMElement]::vsCMElementClass)) 
    {            
        foreach ($e in $elem.ImplementedInterfaces) {
            if($e.FullName -eq "Idecom.Host.Interfaces.IWantToSpecifyContainer") {
                return $elem
            }
            
            return FindHostContainerConfigurationClass($e)
            
        }
    } 
    elseif ($elem.Kind -eq [EnvDTE.vsCMElement]::vsCMElementNamespace) {
        foreach ($e in $elem.Members) {
            $temp = FindHostContainerConfigurationClass($e)
            if($temp -ne $null) {
                return $temp
            }
        }
    }
    $null
}

function CheckAlreadyHasHostContainerConfiguration($project) {
    foreach ($item in $project.ProjectItems) {
        foreach ($codeElem in $item.FileCodeModel.CodeElements) {
            $elem = FindHostContainerConfigurationClass($codeElem)
            if($elem -ne $null) {
                return $true
            }
        }
    }
    $false
}

function AddHostConfigClassIfNeeded {
	$alreadyHasConfiguration = CheckAlreadyHasHostContainerConfiguration($project)
	if($alreadyHasConfiguration -eq $false) {
		$namespace = $project.Properties.Item("DefaultNamespace").Value
		$projectDir = [System.IO.Path]::GetDirectoryName($project.FullName)
		$endpoingConfigPath = [System.IO.Path]::Combine( $projectDir, "ContainerConfig.cs")
		Get-Content  "$installPath\Tools\ContainerConfig.cs" | ForEach-Object { $_ -replace "idecomrootns", $namespace } | Set-Content ($endpoingConfigPath)
		$project.ProjectItems.AddFromFile( $endpoingConfigPath )
	}
}


AddHostConfigClassIfNeeded
$project.Save()