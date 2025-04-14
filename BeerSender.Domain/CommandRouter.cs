using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerSender.Domain
{
    public class CommandRouter(IEventStore eventStore, IServiceProvider serviceProvider)
    {
        public void HandleCommand(object command)
        {
            var commandType = command.GetType();

            var handleType = typeof(CommandHandler<>).MakeGenericType(commandType);

            var handler = serviceProvider.GetService(handleType);

            var methodInfo = handleType.GetMethod("Handle");

            methodInfo?.Invoke(handler, [command]);

            eventStore.SaveChanges();
        }
    }
}
