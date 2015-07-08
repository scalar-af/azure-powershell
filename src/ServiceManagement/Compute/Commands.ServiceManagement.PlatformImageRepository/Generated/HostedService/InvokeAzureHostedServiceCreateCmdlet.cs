// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using Microsoft.Azure;
using Microsoft.WindowsAzure.Commands.ServiceManagement;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Management.Compute;
using System.Management.Automation;

namespace Microsoft.WindowsAzure.Commands.Compute.Automation
{
    [Cmdlet("Invoke", "AzureHostedServiceCreate")]
    public class InvokeAzureHostedServiceCreateCmdlet : ComputeAutomationBaseCmdlet
    {
        [Parameter(Mandatory = true), ValidateNotNullOrEmpty]
        public Microsoft.WindowsAzure.Management.Compute.Models.HostedServiceCreateParameters Parameters { get; set; }

        protected override void OnProcessRecord()
        {
            ServiceManagementProfile.Initialize();
            base.OnProcessRecord();

            ExecuteClientActionNewSM(
                null,
                CommandRuntime.ToString(),
                () => HostedServiceClient.Create(Parameters),
                (s, response) => response);
        }
    }
}
