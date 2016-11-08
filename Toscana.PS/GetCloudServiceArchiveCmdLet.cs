using System.Management.Automation;

namespace Toscana.PS
{
    [Cmdlet(VerbsCommon.Get, "ToscaCloudServiceArchive")]
    [OutputType(typeof(ToscaCloudServiceArchive))]
    public class GetToscaCloudServiceArchiveCmdLet : PSCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to Cloud Service Archive file")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1,
            HelpMessage = "Optional path to import location")]
        public string ImportPath { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var path in Path)
            {
                WriteObject(ToscaCloudServiceArchive.Load(path, ImportPath));
            }
        }
    }
}