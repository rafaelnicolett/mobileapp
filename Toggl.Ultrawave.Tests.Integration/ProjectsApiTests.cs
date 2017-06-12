using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Toggl.Ultrawave.Tests.Integration.BaseTests;
using Xunit;

namespace Toggl.Ultrawave.Tests.Integration
{
    public class ProjectsApiTests
    {
        public class TheGetAllMethod : AuthenticatedGetEndpointBaseTests<List<Project>>
        {
            private readonly string[] projectNames =
            {
                "project1",
                "project2",
                "project3"
            };

            protected override IObservable<List<Project>> CallEndpointWith(ITogglApi togglApi)
                => togglApi.Projects.GetAll();

            [Fact]
            public async Task ReturnsAllProjects()
            {
                var(togglApi, user) = await SetupTestUser();
                foreach (var project in createProjects(user.DefaultWorkspaceId))
                {
                    await togglApi.Projects.Create(project);
                }

                var projects = await togglApi.Projects.GetAll();

                projects.Should().HaveCount(projectNames.Length);
                foreach (var projectName in projectNames)
                {
                    projects.Should().Contain(projectName);
                }
            }

            private List<Project> createProjects(int workspaceId)
            {
                var projects = new List<Project>();
                foreach (var projectName in projectNames)
                {
                    projects.Add(new Project
                    {
                        Name = projectName,
                        WorkspaceId = workspaceId,
                        Billable = false,
                        AutoEstimates = false,
                        EstimatedHours = null,
                        Rate = null
                    });
                }
                return projects;
            }
        }
    }
}
