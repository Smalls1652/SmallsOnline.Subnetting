using System;
using System.Net;
using System.Text.RegularExpressions;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class ParsedNetAddressString
    {
        public ParsedNetAddressString(string netAddressString)
        {
            Initialize(netAddressString);
        }

        public IPAddress IPAddress
        {
            get => _ipAddress;
        }

        public double CidrNotation
        {
            get => _cidrNotation;
        }

        private IPAddress _ipAddress;
        private double _cidrNotation;

        private void Initialize(string netAddressString)
        {
            Regex netAddressRegex = new("^(?'netAddress'(?:\\d{1,3}(?:\\.|)){4})\\/(?'cidrNotation'\\d{1,2})$");
            Match netAddressMatch = netAddressRegex.Match(netAddressString);

            _ipAddress = IPAddress.Parse(netAddressMatch.Groups["netAddress"].Value);
            _cidrNotation = Convert.ToDouble(netAddressMatch.Groups["cidrNotation"].Value);
        }
    }
}