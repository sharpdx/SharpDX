param($installPath, $toolsPath, $package, $project)

$vsRef = $project.Object.References.Item("NuGet.VisualStudio")
if ($vsRef -and !$vsRef.EmbedInteropTypes)
{
    $vsRef.EmbedInteropTypes = $true
}