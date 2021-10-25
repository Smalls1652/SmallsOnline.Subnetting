using System;
using System.Net;
using System.Text.RegularExpressions;

namespace SmallsOnline.Subnetting.Lib.Models
{
    /// <summary>
    /// A representation of a parsed network address.
    /// </summary>
    public class ParsedNetAddressString
    {
        /// <summary>
        /// Create from a network string.
        /// </summary>
        /// <param name="netAddressString">The string of the network.</param>
        public ParsedNetAddressString(string netAddressString)
        {
            Initialize(netAddressString);
        }

        /// <summary>
        /// The parsed IP address.
        /// </summary>
        public IPAddress IPAddress
        {
            get => _ipAddress;
        }

        /// <summary>
        /// The parsed CIDR notation.
        /// </summary>
        public double CidrNotation
        {
            get => _cidrNotation;
        }

        private IPAddress _ipAddress;
        private double _cidrNotation;

        /// <summary>
        /// Parses the string and sets the properties for the IP address and CIDR notation.
        /// </summary>
        /// <param name="netAddressString">The string of the network.</param>
        private void Initialize(string netAddressString)
        {
            Regex netAddressRegex = new("^(?'netAddress'(?:\\d{1,3}(?:\\.|)){4})\\/(?'cidrNotation'\\d{1,2})$");
            Match netAddressMatch = netAddressRegex.Match(netAddressString);

            _ipAddress = IPAddress.Parse(netAddressMatch.Groups["netAddress"].Value);
            _cidrNotation = Convert.ToDouble(netAddressMatch.Groups["cidrNotation"].Value);
        }
    }
}