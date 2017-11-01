using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.Ultrawave.Models;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Serialization;

namespace Toggl.Ultrawave.ApiClients
{
    internal sealed class UserApi : BaseApi, IUserApi
    {
        private readonly UserEndpoints endPoints;
        private readonly IJsonSerializer serializer;

        public UserApi(UserEndpoints endPoints, IApiClient apiClient, IJsonSerializer serializer,
            Credentials credentials)
            : base(apiClient, serializer, credentials)
        {
            this.endPoints = endPoints;
            this.serializer = serializer;
        }

        public IObservable<IUser> Get()
            => CreateObservable<User>(endPoints.Get, AuthHeader);

        public IObservable<IUser> SignUp(Email email, string password)
        {
            var dto = new SignUpDto
            {
                Email = email.ToString(),
                Password = password,
                Workspace = new SignUpDto.WorkspaceDto
                {
                    Name = email.ToString(), // ???
                    InitialPricingPlan = 0 // == Free
                }
            };
            var json = serializer.Serialize(dto, SerializationReason.Post, null);
            return CreateObservable<User>(endPoints.Post, new HttpHeader[0], json);
        }

        private class SignUpDto
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public WorkspaceDto Workspace { get; set; }

            internal class WorkspaceDto
            {
                public string Name { get; set; }

                public int InitialPricingPlan { get; set; }
            }
        }
    }
}
