using System.Management.Automation;
using System.Net;
using SmallsOnline.Subnetting.Lib.Models;

namespace SmallsOnline.Subnetting.Pwsh.Cmdlets;

[Cmdlet(VerbsCommon.New, "IPv4Subnet")]
public class NewIPv4Subnet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "IPAddressAndCIDRNotation")]
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "IPAddressAndSubnetMask")]
    public IPAddress IPAddress { get; set; } = new([192, 168, 1, 0]);

    [Parameter(Position = 1, ParameterSetName = "IPAddressAndCIDRNotation")]
    public double CidrNotation { get; set; } = 24;

    [Parameter(Position = 1, ParameterSetName = "IPAddressAndSubnetMask")]
    public IPAddress SubnetMask { get; set; } = new([255, 255, 255, 0]);

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
    }

    protected override void ProcessRecord()
    {
        IPv4Subnet subnet = ParameterSetName switch
        {
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
