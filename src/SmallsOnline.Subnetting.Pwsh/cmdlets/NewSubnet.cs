using System;
using System.Management.Automation;
using System.Net;

namespace SmallsOnline.Subnetting.Pwsh.Cmdlets
{
    using Lib.Models;

    [Cmdlet(VerbsCommon.New, "Subnet")]
    public class NewSubnet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Default")]
        [ValidateNotNullOrEmpty()]
        public IPAddress IPAddress
        {
            get => ipAddress;
            set => ipAddress = value;
        }

        [Parameter(Position = 1, ParameterSetName = "Default")]
        [ValidateNotNullOrEmpty()]
        public double CidrNotation
        {
            get => cidrNotation;
            set => cidrNotation = value;
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "SubnetString")]
        [ValidateNotNullOrEmpty()]
        public string NetworkString
        {
            get => networkString;
            set => networkString = value;
        }

        private IPAddress ipAddress;
        private double cidrNotation = 24;
        private string networkString;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            IPv4Subnet subnetItem = ParameterSetName switch
            {
                "SubnetString" => new(networkString),
                _ => new(ipAddress, cidrNotation)
            };

            WriteObject(subnetItem);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}