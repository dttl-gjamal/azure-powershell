﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using Microsoft.Azure.Commands.Network.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.New, "AzureRmVirtualNetworkSubnetConfig", DefaultParameterSetName = "SetByResource"), OutputType(typeof(PSSubnet))]
    public class NewAzureVirtualNetworkSubnetConfigCommand : AzureVirtualNetworkSubnetConfigBase
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of the subnet")]
        [ValidateNotNullOrEmpty]
        public override string Name { get; set; }

        public override void Execute()
        {
            base.Execute();

            if (string.IsNullOrEmpty(this.AddressPrefix) &&
                (this.AddressPrefixes == null || !this.AddressPrefixes.Any()))
            {
                throw new ArgumentException("Either AddressPrefix or AddressPrefixes must be specified");
            }

            if (string.Equals(ParameterSetName, Microsoft.Azure.Commands.Network.Properties.Resources.SetByResource))
            {
                if (this.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroupId = this.NetworkSecurityGroup.Id;
                }

                if (this.RouteTable != null)
                {
                    this.RouteTableId = this.RouteTable.Id;
                }
            }

            var subnet = new PSSubnet();
            subnet.Name = this.Name;
            subnet.AddressPrefix = this.AddressPrefix;
            subnet.AddressPrefixes = this.AddressPrefixes;

            if (!string.IsNullOrEmpty(this.NetworkSecurityGroupId))
            {
                subnet.NetworkSecurityGroup = new PSNetworkSecurityGroup();
                subnet.NetworkSecurityGroup.Id = this.NetworkSecurityGroupId;
            }

            if (!string.IsNullOrEmpty(this.RouteTableId))
            {
                subnet.RouteTable = new PSRouteTable();
                subnet.RouteTable.Id = this.RouteTableId;
            }

            if (this.ServiceEndpoint != null)
            {
                subnet.ServiceEndpoints = new List<PSServiceEndpoint>();
                foreach (var item in this.ServiceEndpoint)
                {
                    var service = new PSServiceEndpoint();
                    service.Service = item;
                    subnet.ServiceEndpoints.Add(service);
                }
            }


            WriteObject(subnet);
        }
    }
}
