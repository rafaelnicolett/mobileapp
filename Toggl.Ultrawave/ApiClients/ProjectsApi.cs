using System;
using System.Collections.Generic;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Serialization;

namespace Toggl.Ultrawave.ApiClients
{
    internal sealed class ProjectsApi : BaseApi, IProjectsApi
    {
        private readonly ProjectEndpoints endPoints;

        public ProjectsApi(ProjectEndpoints endPoints, IApiClient apiClient, IJsonSerializer serializer, Credentials credentials)
            : base(apiClient, serializer, credentials)
        {
            this.endPoints = endPoints;
        }

        public IObservable<Project> Create(Project project)
        {
            var endPoint = endPoints.Post(project.WorkspaceId);
            return CreateObservable(endPoint, AuthHeader, project, SerializationReason.Post);
        }

        public IObservable<List<Project>> GetAll()
            => CreateObservable<List<Project>>(endPoints.Get, AuthHeader);
    }
}
