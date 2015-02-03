using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Generator4Developers.Presentation.Windows.Common;
using Generator4Developers.Presentation.Windows.Properties;
using Generator4Developers.Presentation.Windows.Services;

namespace Generator4Developers.Presentation.Windows.ViewModels
{
    //[GeneratorService]
    public class ConnectionStringGeneratorViewModel : ServiceViewModel
    {
        public ConnectionStringGeneratorViewModel()
        {
            CurrentId = Guid.NewGuid();
        }

        private IDictionary<GuidGeneratorMode, string> _modes;

        public override string DisplayName
        {
            get { return Resources.ServiceName_ConnectionStringGenerator; }
        }

        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                OnPropertyChanged();
            }
        }

        public override string Generate()
        {
            return Generate((int)SelectedMode, Pattern);
        }

        public override string Generate(int mode, string pattern)
        {
            try
            {
                CurrentId = Guid.NewGuid();
                return GetGuidString((GuidGeneratorMode)mode, pattern);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private string GetGuidString(GuidGeneratorMode mode, string pattern)
        {
            switch (mode)
            {
                case GuidGeneratorMode.Pattern:
                    {
                        if (String.IsNullOrEmpty(pattern))
                        {
                            return String.Empty;
                        }

                        var inputList = Regex.Matches(pattern, @"\{[0-9]+\}").OfType<Match>().Select(x => int.Parse(x.Value.Trim(new[] { '{', '}' })));
                        var count = inputList.Max() + 1;

                        var items = Enumerable.Range(0, count).Select(x => Guid.NewGuid().ToString().ToUpper()).Cast<object>().ToList();
                        return string.Format(pattern, items.ToArray());
                    }

                case GuidGeneratorMode.Attribute:
                    return string.Format("[Guid(\"{0}\")]", CurrentId);

                case GuidGeneratorMode.Tag:
                    return string.Format("<Guid(\"{0}\")>", CurrentId);
                case GuidGeneratorMode.Registry:
                    return string.Format("{0:B}", CurrentId);

                case GuidGeneratorMode.Structure:
                    {
                        var id = CurrentId;
                        var builder = new StringBuilder();
                        builder.AppendFormat("// {0:B}", id);
                        builder.AppendLine();
                        builder.AppendFormat("static const GUID <<name>> = {0:X}", id);

                        return builder.ToString();
                    }

                case GuidGeneratorMode.ComDefine:
                    return CreateComGuid("DEFINE_GUID(<<name>>,");

                case GuidGeneratorMode.ComImplement:
                    return CreateComGuid("IMPLEMENT_OLECREATE(<<class>, <<external_name>>,");

                default:
                    return string.Empty;
            }
        }

        private string CreateComGuid(string leadingText)
        {
            var id = CurrentId;
            var byteArray = id.ToByteArray();
            var builder = new StringBuilder();
            builder.AppendFormat("// {0}", id.ToString("B").ToUpper());
            builder.AppendLine();

            builder.AppendLine(leadingText);
            builder.AppendFormat("0x{0}", string.Join(String.Empty, byteArray.Take(4).Reverse().Select(x => x.ToString("x2"))));
            builder.AppendFormat(", 0x{0}", string.Join(String.Empty, byteArray.Skip(4).Take(2).Reverse().Select(x => x.ToString("x2"))));
            builder.AppendFormat(", 0x{0}", string.Join(String.Empty, byteArray.Skip(6).Take(2).Reverse().Select(x => x.ToString("x2"))));

            for (var i = 8; i < byteArray.Length; i++)
            {
                builder.AppendFormat(", 0x{0}", string.Join(String.Empty, byteArray.Skip(i).Take(1).Reverse().Select(x => x.ToString("x2"))));
            }

            builder.Append(");");

            return builder.ToString();
        }

        public IDictionary<GuidGeneratorMode, string> Modes
        {
            get { return _modes ?? (_modes = EnumHelper.ToDictionary<GuidGeneratorMode>()); }
        }

        private static bool IsCustomMode(GuidGeneratorMode mode)
        {
            switch (mode)
            {
                case GuidGeneratorMode.Pattern:
                    return true;
                default:
                    return false;
            }
        }

        public override void Activate()
        {
            SelectedMode = GuidGeneratorMode.Registry;
        }

        private GuidGeneratorMode _selectedMode;
        private string _pattern;
        private Guid CurrentId { get; set; }

        public GuidGeneratorMode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                if (!Equals(_selectedMode, value))
                {
                    _selectedMode = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsCustomPattern");
                    OnModeChanged(GetGuidString(SelectedMode, Pattern));
                }
            }
        }

        public bool IsCustomPattern
        {
            get { return IsCustomMode(SelectedMode); }
        }
    }
}