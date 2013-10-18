properties {
  $testMessage = 'Executed Test!'
  $compileMessage = 'Executed Compile!'
  $cleanMessage = 'Executed Clean!'
  #Directories
  $base_dir = resolve-path .
  $source_dir = "$base_dir\src"
  $database_dir = "$base_dir\database"
  $tools_dir = "$base_dir\tools"
  $nuget_dir = "$base_dir\nuget"
  $build_dir = "$base_dir\build"
  #Project settings
  $projectName = 'MenuGen'
  #Database properties
  $databaseServer = '.'
  $databaseName = 'MenuGen_SampleApp'
  #NuGet settings
  $nuget_tempdir = "$base_dir\nuget-temp"
  $nuget_packageName = ''
  $nuget_version = '0.0'
}

task default -depends Test

task Test -depends Compile, Clean { 
  $testMessage
}

task Compile -depends Clean { 
  $compileMessage
}

task Clean { 
  $cleanMessage
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

# ------------------------------------ Database Tasks ------------------------------- 

task LoadSampleData {
	cd $tools_dir\sqlcmd\
	& sqlcmd -E -S $databaseServer -d $databaseName -i $database_dir\scripts\LoadSampleData.sql
}

task DeleteData {
	cd $tools_dir\sqlcmd\
	& sqlcmd -E -S $databaseServer -d $databaseName -i $database_dir\scripts\DeleteData.sql
}

# ------------------------------------ NuGet Tasks ---------------------------------- 

task Nuget-Pack {
	#this link show an example for packing multiple NuGet spec files : https://github.com/hibernating-rhinos/rhino-esb/blob/master/default.ps1
	
	#create new directory
	New-Item $nuget_tempdir\$nuget_packageName\lib\net35 -type directory -force
	#copy CmsLite.Core.dll to that dir
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Core.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Data.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Domains.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Interfaces.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Services.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Utilities.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	Copy-Item $source_dir\cmslite.core\bin\CmsLite.Resources.dll $nuget_tempdir\$nuget_packageName\lib\net35 -force
	#copy admin content to content dir
	Copy-Item $source_dir\cmslite.core\Areas\Admin\Content $nuget_tempdir\$nuget_packageName\content\Areas\Admin\Content -recurse -force
	Copy-Item $source_dir\cmslite.core\Areas\Admin\Views $nuget_tempdir\$nuget_packageName\content\Areas\Admin\Views -recurse -force
	Copy-Item $source_dir\cmslite.core\Areas\Admin\Scripts $nuget_tempdir\$nuget_packageName\content\Areas\Admin\Scripts -recurse -force
	#copy cms.nuspec file to that dir, two levels higher though
	Copy-Item $nuget_dir\cms.nuspec $nuget_tempdir\$nuget_packageName -force
	
	cd $nuget_tempdir\$nuget_packageName
	
	& $tools_dir\nuget\nuget.exe pack $nuget_tempdir\$nuget_packageName\cms.nuspec
}

# ------------------------------------ Tasks ---------------------------------------- 

# ------------------------------------ Build Tasks ---------------------------------- 


# ------------------------------------ Functions ------------------------------------ 

function ZipFiles( $zipfilename, $sourcedir )
{
    [System.Reflection.Assembly]::LoadFrom("$tools_dir\zip-v1.9\Release\Ionic.Zip.dll");

    $zipfile = new-object Ionic.Zip.ZipFile
    $e = $zipfile.AddDirectory($sourcedir, ".")
    $zipfile.Save($zipfilename)
    $zipfile.Dispose()
}