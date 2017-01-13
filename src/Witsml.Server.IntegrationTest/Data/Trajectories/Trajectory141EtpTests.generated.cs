//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Energistics.Common;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Energistics.Datatypes;
using Energistics.Protocol;
using Energistics.Protocol.Core;
using Energistics.Protocol.Discovery;
using Energistics.Protocol.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Trajectories
{
    [TestClass]
    public partial class Trajectory141EtpTests : Trajectory141TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            EtpSetUp(DevKit.Container);
            BeforeEachTest();
            _server.Start();
        }

        protected override void OnTestCleanUp()
        {
            _server?.Stop();
            EtpCleanUp();
            AfterEachTest();
        }

        [TestMethod]
        public void Trajectory141_Ensure_Creates_Trajectory_With_Default_Values()
        {

            DevKit.EnsureAndAssert<TrajectoryList, Trajectory>(Trajectory);

        }

        [TestMethod]
        public async Task Trajectory141_GetResources_Can_Get_All_Trajectory_Resources()
        {
            AddParents();

            DevKit.AddAndAssert<TrajectoryList, Trajectory>(Trajectory);

            await RequestSessionAndAssert();

            var uri = Trajectory.GetUri();
            var parentUri = uri.Parent;

            await GetResourcesAndAssert(parentUri);

            var folderUri = parentUri.Append(uri.ObjectType);
            await GetResourcesAndAssert(folderUri);
        }

        [TestMethod]
        public async Task Trajectory141_PutObject_Can_Add_Trajectory()
        {
            AddParents();
            await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = Trajectory.GetUri();

            var dataObject = CreateDataObject<TrajectoryList, Trajectory>(uri, Trajectory);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check for message flag indicating No Data
            Assert.IsNotNull(args?.Header);
            Assert.AreEqual((int)MessageFlags.NoData, args.Header.MessageFlags);

            // Put Object
            await PutAndAssert(handler, dataObject);

            // Get Object
            args = await GetAndAssert(handler, uri);

            // Check Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetXml();

            var result = Parse<TrajectoryList, Trajectory>(xml);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Trajectory141_PutObject_Can_Update_Trajectory()
        {
            AddParents();
            await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = Trajectory.GetUri();

            // Add a Comment to Data Object            
            Trajectory.CommonData = new CommonData()
            {
                Comments = "Test PutObject"
            };

            var dataObject = CreateDataObject<TrajectoryList, Trajectory>(uri, Trajectory);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check for message flag indicating No Data
            Assert.IsNotNull(args?.Header);
            Assert.AreEqual((int)MessageFlags.NoData, args.Header.MessageFlags);

            // Put Object for Add
            await PutAndAssert(handler, dataObject);

            // Get Added Object
            args = await GetAndAssert(handler, uri);

            // Check Added Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetXml();

            var result = Parse<TrajectoryList, Trajectory>(xml);

            Assert.IsNotNull(result);

            Assert.IsNotNull(result.CommonData.Comments);

            // Remove Comment from Data Object
            result.CommonData.Comments = null;

            var updateDataObject = CreateDataObject<TrajectoryList, Trajectory>(uri, result);

            // Put Object for Update
            await PutAndAssert(handler, updateDataObject);

            // Get Updated Object
            args = await GetAndAssert(handler, uri);

            // Check Added Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var updateXml = args.Message.DataObject.GetXml();

            result = Parse<TrajectoryList, Trajectory>(updateXml);

            Assert.IsNotNull(result);

            // Test Data Object overwrite

            Assert.IsNull(result.CommonData.Comments);

        }

        [TestMethod]
        public async Task Trajectory141_DeleteObject_Can_Delete_Trajectory()
        {
            AddParents();
			await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = Trajectory.GetUri();

            var dataObject = CreateDataObject<TrajectoryList, Trajectory>(uri, Trajectory);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check for message flag indicating No Data
            Assert.IsNotNull(args?.Header);
            Assert.AreEqual((int)MessageFlags.NoData, args.Header.MessageFlags);

            // Put Object
            await PutAndAssert(handler, dataObject);

            // Get Object
            args = await GetAndAssert(handler, uri);

            // Check Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetXml();

            var result = Parse<TrajectoryList, Trajectory>(xml);
            Assert.IsNotNull(result);

            // Delete Object
            await DeleteAndAssert(handler, uri);

            // Get Object
            args = await GetAndAssert(handler, uri);

            // Check Data Object doesn't exist
            Assert.AreEqual(0, args?.Message?.DataObject?.Data?.Length ?? 0);
        }
    }
}