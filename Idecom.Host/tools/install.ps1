param($installPath, $toolsPath, $package, $project)

if(!$toolsPath){
	$project = Get-Project
}

function UpdateProjectToStartIdecomHostIfNotAlready {
	[xml] $prjXml = Get-Content $project.FullName
	foreach($PropertyGroup in $prjXml.project.ChildNodes)
	{
		if($PropertyGroup.StartAction -ne $null)
		{
			return
		}
	}

    $exeName = "Idecom.Host.exe"

	$propertyGroupElement = $prjXml.CreateElement("PropertyGroup", $prjXml.Project.GetAttribute("xmlns"));
	$startActionElement = $prjXml.CreateElement("StartAction", $prjXml.Project.GetAttribute("xmlns"));
	$propertyGroupElement.AppendChild($startActionElement) | Out-Null
	$propertyGroupElement.StartAction = "Program"
	$startProgramElement = $prjXml.CreateElement("StartProgram", $prjXml.Project.GetAttribute("xmlns"));
	$propertyGroupElement.AppendChild($startProgramElement) | Out-Null
	$propertyGroupElement.StartProgram = "`$(ProjectDir)`$(OutputPath)$exeName"
	$prjXml.project.AppendChild($propertyGroupElement) | Out-Null
	$writerSettings = new-object System.Xml.XmlWriterSettings
	$writerSettings.OmitXmlDeclaration = $false
	$writerSettings.NewLineOnAttributes = $false
	$writerSettings.Indent = $true
	$projectFilePath = Resolve-Path -Path $project.FullName
	$writer = [System.Xml.XmlWriter]::Create($projectFilePath, $writerSettings)
	$prjXml.WriteTo($writer)
	$writer.Flush()
	$writer.Close()
}


function FindHostConfigurationClass($elem) {
    if ($elem.IsCodeType -and ($elem.Kind -eq [EnvDTE.vsCMElement]::vsCMElementClass)) 
    {            
        foreach ($e in $elem.Bases) {
            if($e.FullName -eq "Idecom.Host.Interfaces.HostedService") {
                return $elem
            }
            
            return FindHostConfigurationClass($e)
            
        }
    } 
    elseif ($elem.Kind -eq [EnvDTE.vsCMElement]::vsCMElementNamespace) {
        foreach ($e in $elem.Members) {
            $temp = FindHostConfigurationClass($e)
            if($temp -ne $null) {
                return $temp
            }
        }
    }
    $null
}

function CheckAlreadyHasHostConfiguration($project) {
    foreach ($item in $project.ProjectItems) {
        foreach ($codeElem in $item.FileCodeModel.CodeElements) {
            $elem = FindHostConfigurationClass($codeElem)
            if($elem -ne $null) {
                return $true
            }
        }
    }
    $false
}

function AddHostConfigClassIfNeeded {
	$alreadyHasConfiguration = CheckAlreadyHasHostConfiguration($project)
	if($alreadyHasConfiguration -eq $false) {
		$namespace = $project.Properties.Item("DefaultNamespace").Value
		$projectDir = [System.IO.Path]::GetDirectoryName($project.FullName)
		$endpoingConfigPath = [System.IO.Path]::Combine( $projectDir, "HostConfig.cs")
		Get-Content  "$installPath\Tools\HostConfig.cs" | ForEach-Object { $_ -replace "idecomrootns", $namespace } | Set-Content ($endpoingConfigPath)
		$project.ProjectItems.AddFromFile( $endpoingConfigPath )
	}
}


AddHostConfigClassIfNeeded
$project.Save()
UpdateProjectToStartIdecomHostIfNotAlready
