using GraphQLDemo.Schema.Mutation;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public async Task<CourseResult> CourseCreated([EventMessage] CourseResult course)
        {
            return course;
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdate(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_{nameof(CourseUpdate)}";
            return topicEventReceiver.SubscribeAsync<string, CourseResult>(topicName);
        }
    }
}
