namespace Certification.Tasks
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class VerifyCertificationResultTask : Task
    {
        [Required]
        public string File { get; set; }

        public override bool Execute()
        {
            if (!System.IO.File.Exists(File))
            {
                Log.LogError("Cannot find result file: {0}", File);
                return false;
            }

            try
            {
                var document = XDocument.Load(File);
                Log.LogMessage("Loaded result file: {0}", File);

                var resultAttribute = document.Root.Attribute("OVERALL_RESULT");
                if (resultAttribute == null)
                {
                    Log.LogError("Cannot find result attribute.");
                    return false;
                }

                if (resultAttribute.Value.ToUpperInvariant() == "FAIL")
                {
                    Log.LogError("Certification failed for result {0} ({1})", Path.GetFileName(File), File);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                return false;
            }

            return true;
        }
    }
}
