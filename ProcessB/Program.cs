using System;
using fastipc.Bus;
using fastipc.Message;
using Messages;

namespace ProcessB
{
	class Program
	{
		static void Main(string[] args)
		{
			var pipeName = new SimpleStringPipeName(name: "Example");
			var bus = new NamedPipeBus(pipeName: pipeName);
			new ProcessBHost(bus);

			Console.WriteLine($"Process B Host Runing");
			Console.WriteLine("type exit to close");
			Console.WriteLine("type help for command list");
			var exit = false;
			do
			{
				var cmd = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(cmd))
				{
					continue;
				}

				switch (cmd.ToLower())
				{
					case "exit":
						exit = true;
						break;
					case "ping":
						bus.Publish(new Ping());
						break;
					case "help":
					case "?":
						Console.WriteLine("exit: exit program");
						Console.WriteLine("ping: publish ping message");
						Console.WriteLine("cls: Clear Screen");
						break;
					case "cls":
						Console.Clear();
						break;
				}
			} while (!exit);

			bus.Dispose();
		}
	}

	public class ProcessBHost : IHandleMessage
	{
		private readonly IBus _bus;

		public ProcessBHost(IBus bus)
		{
			_bus = bus;
			_bus.Subscribe(this);
		}

		public void Handle(Message msg)
		{
			HandleInternal((dynamic)msg);
		}

		private void HandleInternal(Ping msg)
		{
			Console.WriteLine("Received a ping, sending a pong");
			_bus.Publish(new Pong());
		}

		private void HandleInternal(Pong msg)
		{
			Console.WriteLine("Received a pong");
		}
	}
}
