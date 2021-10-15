using System;
using System.Net;

namespace SmallsOnline.Subnetting.Lib.Models
{
    using Core;

    public class Subnet
    {
        public Subnet(IPAddress netAddress, double cidrNotation)
        {
            Initialize(netAddress, cidrNotation);
        }

        public Subnet(string netAddress)
        {
            ParsedNetAddressString parsedNetAddress = new(netAddress);
            Initialize(parsedNetAddress.IPAddress, parsedNetAddress.CidrNotation);
        }

        public IPAddress NetworkAddress
        {
            get => _networkAddress;
        }

        public IPAddress SubnetMask
        {
            get => _subnetMask;
        }

        public double CidrMask
        {
            get => _cidrMask;
        }

        public IPAddress BroadcastAddress
        {
            get => _broadcastAddress;
        }

        public double TotalAddresses
        {
            get => _totalAddresses;
        }

        public double UsableAddresses
        {
            get => _totalAddresses - 2;
        }

        public UsableHostRange UsableHostRange
        {
            get => _usableHostRange;
        }

        private IPAddress _networkAddress;
        private IPAddress _subnetMask;
        private double _cidrMask;
        private IPAddress _broadcastAddress;
        private double _totalAddresses;
        private UsableHostRange _usableHostRange;

        private void Initialize(IPAddress netAddress, double cidrNotation)
        {
            _networkAddress = Calculator.GetSubnetBoundary(netAddress, cidrNotation);
            _cidrMask = cidrNotation;

            _totalAddresses = Calculator.GetMaxAddresses(_cidrMask);
            _subnetMask = Calculator.GetSubnetMask(_cidrMask);

            _broadcastAddress = Calculator.GetBroadcastAddress(_networkAddress, _cidrMask);

            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        public override string ToString()
        {
            return $"{_networkAddress}/{_cidrMask}";
        }
    }
}