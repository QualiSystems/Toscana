using System.Management.Automation;

namespace Toscana.PS
{
    [Cmdlet(VerbsCommon.Get, "ToscaServiceTemplate")]
    [OutputType(typeof(ToscaServiceTemplate))]
    public class GetToscaServiceTemplateCmdLet : Cmdlet
    {
        [Parameter(Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             ValueFromPipeline = true,
             Position = 0,
             HelpMessage = "Path to Cloud Service Archive file")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var path in Path)
            {
                WriteObject(ToscaServiceTemplate.Load(path));
            }
        }
    }
}