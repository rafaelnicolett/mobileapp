using System;
using System.Collections.Generic;

namespace Toggl.Ultrawave.ApiClients
{
    public interface IProjectsApi
    {
        IObservable<List<Project>> GetAll();
        IObservable<Project> Create(Project timeEntry);
    }
}
