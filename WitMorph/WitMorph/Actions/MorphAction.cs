using System;
using System.Reflection;
using System.Xml;

namespace WitMorph.Actions
{
    public abstract class MorphAction
    {
        protected MorphAction()
        {
            GetDeserializeMethod(GetType());
        }

        public static MethodInfo GetDeserializeMethod(Type actionType)
        {
            if (!typeof(MorphAction).IsAssignableFrom(actionType))
            {
                throw new InvalidOperationException(string.Format("Type '{0}' must inherit from base type '{1}'", actionType, typeof(MorphAction)));
            }
            var deserializeMethod = actionType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(XmlReader) }, null);
            if (deserializeMethod == null || !typeof(MorphAction).IsAssignableFrom(deserializeMethod.ReturnType))
            {
                throw new InvalidOperationException(string.Format("Type '{0} must implement static method '{1} Deserialize(XmlReader reader)' on .", actionType.FullName, typeof(MorphAction)));
            }
            return deserializeMethod;
        }

        public abstract void Execute(ExecutionContext context);
        public abstract void Serialize(XmlWriter writer);
    }

}