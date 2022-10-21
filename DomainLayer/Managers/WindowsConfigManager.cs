using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DomainLayer.Managers
{
    public class WindowsConfigManager
    {
        SModule _moduleSettings;
        public SModule LoadFromConfig(int moduleType)
        {
            var settings = DeserializeObject(GetPathToConfig());

            if (settings == null) return null;

            return settings.Modules.First(x => x.id == moduleType.ToString());
        }
        public void SaveToConfig(SModule module)
        {
            var settings = DeserializeObject(GetPathToConfig());
            settings.Modules.RemoveAll(x => x.id == module.id.ToString());
            settings.Modules.Add(module);

            SerializeObject(settings);
        }
        private void SerializeObject(ModuleSettings settings)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ModuleSettings));
            TextWriter writer = new StreamWriter(GetPathToConfig());
            ser.Serialize(writer, settings);
            writer.Close();
        }
        private ModuleSettings DeserializeObject(string filename)
        {
            if (File.Exists(filename))
            {
                XmlSerializer serializer =
            new XmlSerializer(typeof(ModuleSettings), new XmlRootAttribute("ModuleSettings"));

                ModuleSettings i;

                using (Stream reader = new FileStream(filename, FileMode.Open))
                {
                    i = (ModuleSettings)serializer.Deserialize(reader);
                    return i;
                }
            }
            return null;
        }
        public ComponentType ReturnXKey(SModule module, int order)
        {
            var panel = module.Panels.FirstOrDefault(x => x.order == order);
            if (panel == null)
                return ComponentType.notSet;

            return (ComponentType)Enum.Parse(typeof(ComponentType), panel.component);
        }
        public void UpdateSettings(SModule module, ComponentType _xKey, string propertyName, object value, bool detached = false)
        {
            object source = module.Panels.First(x => x.component.Equals(_xKey.ToString()));

            if (detached)
                source = module.Panels.First(x => x.component.Equals(_xKey.ToString())).detachedWindow;

            var prop = source.GetType().GetProperty(propertyName);
            prop.SetValue(source, value);
        }

        //TODO:Need rwork
        public void UpdateOrder(SModule module, ComponentType _xKey, object value)
        {
            if ((int)value == 1)
            {
                module.Panels.First(x => x.order == 1).order = 2;
                UpdateSettings(module, _xKey, "order", value, false);
            }
            else
                UpdateSettings(module, _xKey, "order", value, false);
        }
        public object ReturnValue(SModule module, ComponentType _xKey, string propertyName, bool detached = false)
        {
            object source = module.Panels.First(x => x.component.Equals(_xKey.ToString()));

            if (detached)
                source = module.Panels.First(x => x.component.Equals(_xKey.ToString())).detachedWindow;

            var prop = source.GetType().GetProperty(propertyName);
            return prop.GetValue(source);
        }
        public WindowPositionDTO ReturnWindowPosition(SModule module, ComponentType xKey)
        {
            var h = (double)ReturnValue(module, xKey, "height", detached: true);
            var w = (double)ReturnValue(module, xKey, "width", detached: true);
            var t = (double)ReturnValue(module, xKey, "Y", detached: true);
            var l = (double)ReturnValue(module, xKey, "X", detached: true);

            return new WindowPositionDTO() { Height = h, Width = w, Top = t, Left = l };
        }
        private string GetPathToConfig()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(path, "Resource/WindowsConfig.xml");
        }


        #region Class to serialize
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ModuleSettings
        {

            private List<SModule> moduleField;

            private string versionField;

            [XmlElementAttribute("module")]
            public List<SModule> Modules
            {
                get
                {
                    return this.moduleField;
                }
                set
                {
                    this.moduleField = value;
                }
            }

            [XmlAttributeAttribute()]
            public string version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }


        [SerializableAttribute()]
        [DesignerCategoryAttribute("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class SModule
        {

            private List<SPanel> panelField;

            private string idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("panel")]
            public List<SPanel> Panels
            {
                get
                {
                    return this.panelField;
                }
                set
                {
                    this.panelField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SPanel
        {

            private SDetachedWindow detachedWindowField;

            private string componentField;

            private int orderField;

            private string widthField;

            private bool detachedField;

            /// <remarks/>
            public SDetachedWindow detachedWindow
            {
                get
                {
                    return this.detachedWindowField;
                }
                set
                {
                    this.detachedWindowField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string component

            {
                get
                {
                    return this.componentField;
                }
                set
                {
                    this.componentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int order
            {
                get
                {
                    return this.orderField;
                }
                set
                {
                    this.orderField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool detached
            {
                get
                {
                    return this.detachedField;
                }
                set
                {
                    this.detachedField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SDetachedWindow
        {

            private double xField;

            private double yField;

            private double widthField;

            private double heightField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double X
            {
                get
                {
                    return this.xField;
                }
                set
                {
                    this.xField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double Y
            {
                get
                {
                    return this.yField;
                }
                set
                {
                    this.yField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }
        }

        #endregion

    }
}
