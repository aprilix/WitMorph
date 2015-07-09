using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client.Provision;
using WitMorph.Model;

namespace WitMorph.Actions
{
    using System.Threading;

    public class ImportWorkItemTypeDefinitionMorphAction : MorphAction
    {
        private readonly WorkItemTypeDefinition _typeDefinition;

        public ImportWorkItemTypeDefinitionMorphAction(WorkItemTypeDefinition typeDefinition)
        {
            _typeDefinition = typeDefinition;
        }

        public string WorkItemTypeName
        {
            get { return _typeDefinition.Name; }
        }

        public override void Execute(ExecutionContext context)
        {
            if (context.TraceLevel >= TraceLevel.Verbose)
            {
                string traceFile;
                int count = 0;
                do
                {
                    count++;
                    traceFile = Path.Combine(context.OutputPath, string.Format("{0}-{1}-definition.xml", WorkItemTypeName, count));

                } while (File.Exists(traceFile));
                _typeDefinition.WITDElement.OwnerDocument.Save(traceFile);
            }

            var project = context.GetWorkItemProject();
            var accumulator = new ImportEventArgsAccumulator();
            project.WorkItemTypes.ImportEventHandler += accumulator.Handler;
            try
            {
                project.WorkItemTypes.Import(_typeDefinition.WITDElement);
                if (accumulator.ImportEventArgs.Count != 0)
                {
                    throw new ProvisionValidationException(string.Format("Could not import work item type definition '{0}'", WorkItemTypeName));
                }
                Thread.Sleep(5000);            

                project.Store.RefreshCache();
                project.Store.SyncToCache();
            }
            catch (ProvisionValidationException)
            {
                foreach (var e in accumulator.ImportEventArgs)
                {
                    context.Log("IMPORT: " + e.Message, TraceLevel.Error);
                }
                throw;
            }
            finally
            {
                project.WorkItemTypes.ImportEventHandler -= accumulator.Handler;
            }
        }

        protected override void SerializeCore(XmlWriter writer)
        {
            writer.WriteCData(_typeDefinition.WITDElement.OuterXml);
        }

        public static MorphAction Deserialize(XmlElement element, DeserializationContext context)
        {
            var cdata = element.ChildNodes.OfType<XmlCDataSection>().Single();
            var doc = new XmlDocument();
            doc.LoadXml(cdata.Value);
            var typeDef = new WorkItemTypeDefinition(doc);
            return new ImportWorkItemTypeDefinitionMorphAction(typeDef);
        }

        public override string ToString()
        {
            return string.Format("Import work item type definition '{0}'", _typeDefinition.Name);
        }

        class ImportEventArgsAccumulator
        {
            public ImportEventArgsAccumulator()
            {
                ImportEventArgs = new List<ImportEventArgs>();
            }
            
            public void Handler(object sender, ImportEventArgs eventArgs)
            {
                ImportEventArgs.Add(eventArgs);
            }

            public List<ImportEventArgs> ImportEventArgs { get; set; }
        }

    }
}