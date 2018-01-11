using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Microsoft.Qwiq.Client.Rest
{
    internal class Project : Qwiq.Project
    {
        internal Project([NotNull] TeamProjectReference project, [NotNull] WorkItemStore store)
            : base(
                project.Id,
                project.Name,
                new Uri(project.Url),
                new Lazy<IWorkItemTypeCollection>(
                    () =>
                        {
                            var wits = store.NativeWorkItemStore
                                            .Value
                                            .GetWorkItemTypesAsync(project.Name)
                                            .GetAwaiter()
                                            .GetResult();

                            var wits2 = new List<IWorkItemType>(wits.Count);
                            for (var i = 0; i < wits.Count; i++)
                            {
                                var wit = wits[i];
                                wits2.Add(new WorkItemType(wit));
                            }

                            return new WorkItemTypeCollection(wits2);
                        }),
                new Lazy<INodeCollection>(
                    () =>
                        {
                            var result = store.NativeWorkItemStore
                                              .Value
                                              .GetClassificationNodeAsync(
                                                  project.Id,
                                                  TreeStructureGroup.Areas,
                                                  null,
                                                  int.MaxValue)
                                              .GetAwaiter()
                                              .GetResult();

                            // SOAP Client does not return just the root, so return the root's children to match implementation
                            var n = new Node(result).ChildNodes;
                            return n;
                        }),
                new Lazy<INodeCollection>(
                    () =>
                        {
                            var result = store.NativeWorkItemStore
                                              .Value
                                              .GetClassificationNodeAsync(
                                                  project.Name,
                                                  TreeStructureGroup.Iterations,
                                                  null,
                                                  int.MaxValue)
                                              .GetAwaiter()
                                              .GetResult();

                            return new Node(result).ChildNodes;
                        }),
                new Lazy<IQueryFolderCollection>(() =>
                {
                    return new QueryFolderCollection(() =>
                    {
                        var queryHierarchyItemRepo = new QueryHiearchyItemRepository(store.NativeWorkItemStore, project.Id);
                        return queryHierarchyItemRepo.Get().Where(qf => qf != null).Select(qf => new QueryFolder(qf, queryHierarchyItemRepo));
                    });
                }))
        {
        }
    }
}