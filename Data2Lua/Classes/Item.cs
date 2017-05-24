using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data2Lua.Classes
{
    class Item
    {
        private int _id;
        private string _unidentifiedDisplayName;
        private string _unidentifiedResourceName;
        private string _unidentifiedDescriptionName;
        private string _identifiedDisplayName;
        private string _identifiedResourceName;
        private string _identifiedDescriptionName;
        private int _slotCount;
        private int _classNum;

        public override string ToString()
        {

            string _item = "\t["+ this._id + "] = {\n";
            _item += "\t\tunidentifiedDisplayName = \""+ this.UnidentifiedDisplayName + "\",\n";
            _item += "\t\tunidentifiedResourceName = \""+ this.UnidentifiedResourceName + "\",\n";
            _item += "\t\tunidentifiedDescriptionName = {"+ this.UnidentifiedDescriptionName + "},\n";
            _item += "\t\tidentifiedDisplayName = \"" + this.IdentifiedDisplayName + "\",\n";
            _item += "\t\tidentifiedResourceName = \""+ this.IdentifiedResourceName + "\",\n";
            _item += "\t\tidentifiedDescriptionName = {"+ this.IdentifiedDescriptionName + "},\n";
            _item += "\t\tslotCount = "+ this.SlotCount + ",\n";
            _item += "\t\tClassNum = "+ this.ClassNum + "\n\t}";

            return _item;
        }

        protected static string DescriptionFormat(string description)
        {
            return "\n\t\t\t\"" + description.Replace("\n", "\",\n\t\t\t\"").Substring(0, description.Length - 1) + "\"\n\t\t";
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string UnidentifiedDisplayName
        {
            get
            {
                return _unidentifiedDisplayName.Replace("_", " ").Trim();
            }

            set
            {
                _unidentifiedDisplayName = value;
            }
        }

        public string UnidentifiedResourceName
        {
            get
            {
                return _unidentifiedResourceName;
            }

            set
            {
                _unidentifiedResourceName = value;
            }
        }

        public string UnidentifiedDescriptionName
        {
            get
            {
                if (string.IsNullOrEmpty(_unidentifiedDescriptionName)) return "";
                return DescriptionFormat(_unidentifiedDescriptionName.Replace("\"", "\\\""));
            }

            set
            {
                _unidentifiedDescriptionName = value;
            }
        }

        public string IdentifiedDisplayName
        {
            get
            {
                return _identifiedDisplayName.Replace("_", " ").Trim();
            }

            set
            {
                _identifiedDisplayName = value;
            }
        }

        public string IdentifiedResourceName
        {
            get
            {
                return _identifiedResourceName;
            }

            set
            {
                _identifiedResourceName = value;
            }
        }

        public string IdentifiedDescriptionName
        {
            get
            {
                if (string.IsNullOrEmpty(_identifiedDescriptionName)) return "";
                return DescriptionFormat(_identifiedDescriptionName.Replace("\"", "\\\""));
            }

            set
            {
                _identifiedDescriptionName = value;
            }
        }

        public int SlotCount
        {
            get
            {
                return _slotCount;
            }

            set
            {
                _slotCount = value;
            }
        }

        public int ClassNum
        {
            get
            {
                return _classNum;
            }

            set
            {
                _classNum = value;
            }
        }
    }
}
