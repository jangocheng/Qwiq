﻿using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace Qwiq.WorkItemStore
{

    public abstract class WorkItemContextSpecification<T> : WorkItemStoreContextSpecification<T>
        where T : IWorkItemStore
    {
        private const int Id = 10726528;

        protected IWorkItem Result { get; private set; }

        [TestMethod]
        [TestCategory("localOnly")]
        public void Reading_Id_from_Fields_property_with_ReferenceName_equals_the_property_value()
        {
            Result.Fields[CoreFieldRefNames.Id]?.Value?.ToString().ShouldEqual(Result.Id.ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        [TestCategory("localOnly")]
        public void Reading_Id_from_this_operator_with_ReferenceName_equals_the_property_value()
        {
            Result[CoreFieldRefNames.Id]?.ToString().ShouldEqual(Result.Id.ToString());
        }

        public override void When()
        {
            Result = WorkItemStore.Query(Id);
        }
    }
}