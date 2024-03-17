using System.Management.Automation;
using System.Net;
using SmallsOnline.Subnetting.Lib.Models;

namespace SmallsOnline.Subnetting.Pwsh.Cmdlets;

/// <summary>
/// Creates a new IPv4 subnet.
/// </summary>
[Cmdlet(VerbsCommon.New, "IPv4Subnet", DefaultParameterSetName = "IPAddressAndCIDRNotation")]
public class NewIPv4Subnet : PSCmdlet
{
    /// <summary>
    /// An IP address in a subnet.
    /// </summary>
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "IPAddressAndCIDRNotation")]
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "IPAddressAndSubnetMask")]
    public IPAddress IPAddress { get; set; } = new([192, 168, 1, 0]);

    /// <summary>
    /// The CIDR notation for the subnet.
    /// </summary>
    [Parameter(Position = 1, ParameterSetName = "IPAddressAndCIDRNotation")]
    public double CidrNotation { get; set; } = 24;

    /// <summary>
    /// The subnet mask for the subnet.
    /// </summary>
    [Parameter(Position = 1, ParameterSetName = "IPAddressAndSubnetMask")]
    public IPAddress SubnetMask { get; set; } = new([255, 255, 255, 0]);

    /// <summary>
    /// A string representation of the subnet.
    /// </summary>
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "SubnetString")]
    [ValidateNotNullOrEmpty]
    public string NetworkString { get; set; } = "192.168.1.0/24";

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
        IPv4Subnet subnet = ParameterSetName switch
        {
            "SubnetString" => new(NetworkString),
            "IPAddressAndSubnetMask" => new(IPAddress, SubnetMask),
            _ => new(IPAddress, CidrNotation)
        };

        WriteObject(subnet);
    }

    protected override void EndProcessing()
    {
        base.EndProcessing();
    }
}
